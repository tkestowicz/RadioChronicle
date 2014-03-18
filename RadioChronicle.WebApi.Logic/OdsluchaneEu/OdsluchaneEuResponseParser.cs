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

        private IEnumerable<HtmlNode> SelectListWithRadioStationsFromHtmlGroup(HtmlNode radioStationGroup)
        {
            return radioStationGroup.SelectNodes("option");
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
            var result = new List<RadioStationGroup>();

            var radioStationGroups = SelectListWithGroupedRadioStationsFromHTMLDocument(document);

            if (radioStationGroups == null) return result;

            foreach (var radioStationGroup in radioStationGroups)
            {
                var radioStations = ParseDOMAndSelectRadioStations(SelectListWithRadioStationsFromHtmlGroup(radioStationGroup));
                result.Add(new RadioStationGroup()
                {
                    GroupName = radioStationGroup.Attributes[0].Value,
                    RadioStations = radioStations
                });
            }

            return result;
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

        private IEnumerable<RadioStation> ParseDOMAndSelectRadioStations(IEnumerable<HtmlNode> radioStations)
        {
            var result = new List<RadioStation>();

            if (radioStations == null) return result;

            foreach (var radioStation in radioStations)
            {
                if (!radioStation.Attributes.Any()) continue;

                var radioName = radioStation.Attributes.SingleOrDefault(a => a.Name == "label");
                var radioId = radioStation.Attributes.SingleOrDefault(a => a.Name == "value");

                var isSelected = radioStation.Attributes.SingleOrDefault(a => a.Name == "selected");
                var isDefault = isSelected != null && isSelected.Value == "selected";

                result.Add(new RadioStation()
                {
                    Id = (radioId != null) ? int.Parse(radioId.Value) : 0,
                    Name = (radioName != null) ? radioName.Value : "",
                    IsDefault = isDefault
                });
            }

            return result;
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
