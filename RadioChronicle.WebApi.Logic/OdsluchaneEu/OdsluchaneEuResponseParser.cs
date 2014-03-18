using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    //TODO: refactor
    public class OdsluchaneEuResponseParser : IResponseParser
    {

        private readonly ICollectionParser<Track> trackCollectionParser;
        private readonly ICollectionParser<TrackHistory> trackHistoryCollectionParser;
        private readonly ISelectorHelper<HtmlNode> nodeSelectorHelper;

        private readonly Func<HtmlDocument, string, IEnumerable<HtmlNode>> getListOfNodes = (document, selector) =>
        {
            if (document == null) return new List<HtmlNode>();

            return document.DocumentNode.SelectNodes(selector) ?? new HtmlNodeCollection(null);
        };

        public OdsluchaneEuResponseParser(ICollectionParser<Track> trackCollectionParser, ICollectionParser<TrackHistory> trackHistoryCollectionParser, ISelectorHelper<HtmlNode> nodeSelectorHelper)
        {
            this.trackCollectionParser = trackCollectionParser;
            this.trackHistoryCollectionParser = trackHistoryCollectionParser;
            this.nodeSelectorHelper = nodeSelectorHelper;
        }

        private IEnumerable<HtmlNode> SelectListWithGroupedRadioStationsFromHTMLDocument(HtmlDocument document)
        {
            return getListOfNodes(document, "//select[@name='r']/optgroup");
        }

        private IList<HtmlNode> SelectResultsListWithTracks(HtmlDocument document)
        {
            // skip first element which is a result header
            return getListOfNodes(document, "//table[@class='wyniki']/tr").Skip(1).ToList();
        }

        private IEnumerable<HtmlNode> SelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            return getListOfNodes(document, "//ul[@class='panel_aktualnie']/li");
        }

        public IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document)
        {
            var parser = new RadioStationGroupParser(new RadioStationCollectionParser(), new RadioStationParser());

            var radioStationGroups = SelectListWithGroupedRadioStationsFromHTMLDocument(document);

            return radioStationGroups.Select(parser.Parse);
        }

        public IEnumerable<Track> ParseDOMAndSelectMostPopularTracks(HtmlDocument document)
        {
            var rows = SelectResultsListWithTracks(document);

            return trackCollectionParser.Parse(rows, new MostPopularTrackParser(nodeSelectorHelper));
        }

        public IEnumerable<Track> ParseDOMAndSelectNewestTracks(HtmlDocument document)
        {
            var rows = SelectResultsListWithTracks(document);

            return trackCollectionParser.Parse(rows, new NewestTrackParser(nodeSelectorHelper));
        }

        public IDictionary<RadioStation, Track> ParseDOMAndSelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            var result = new Dictionary<RadioStation, Track>();

            var retrievedTracks = SelectCurrentlyBroadcastedTracks(document);

            foreach (var track in retrievedTracks)
            {
                var parsedRow = ParseDOMAndReturnCurrentlyBroadcastedTrack(track);

                if (parsedRow.Value.Equals(Track.Empty) == false) result.Add(parsedRow.Key, parsedRow.Value);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectBroadcastHistory(HtmlDocument document)
        {
            var rows = SelectResultsListWithTracks(document);

            return trackCollectionParser.Parse(rows, new TrackBroadcastHistoryParser(nodeSelectorHelper));
        }

        public IEnumerable<TrackHistory> ParseDOMAndSelectTrackHistory(HtmlDocument document)
        {
            var rows = SelectResultsListWithTracks(document);

            return trackHistoryCollectionParser.Parse(rows, new TrackHistoryParser());
        }
        
        private KeyValuePair<RadioStation, Track> ParseDOMAndReturnCurrentlyBroadcastedTrack(HtmlNode track)
        {
            const int radioNameElementIndex = 0;
            const int trackInfoElementIndex = 1;

            try
            {
                var key = new RadioStation()
                {
                    Id = 0,
                    IsDefault = false,
                    Name = track.ChildNodes[radioNameElementIndex].InnerText.Trim()
                };

                var value = Track.Empty;


                
                var trackInfo = track.ChildNodes[trackInfoElementIndex].ChildNodes[trackInfoElementIndex];

                value.Name = HttpUtility.HtmlDecode(trackInfo.InnerText);
                value.RelativeUrlToTrackDetails = trackInfo.ChildNodes.Single().Attributes["href"].Value;

                return new KeyValuePair<RadioStation, Track>(key, value);
            }
            catch
            {
                return new KeyValuePair<RadioStation, Track>(null, Track.Empty);
            }
        }
    }
}
