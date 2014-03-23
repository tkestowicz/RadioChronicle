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
        private readonly ICollectionParser<KeyValuePair<RadioStation, Track>> currentTracksCollectionParser;
        private readonly ICollectionParser<TrackHistory> trackHistoryCollectionParser;
        private readonly ISelectorHelper<HtmlNode> nodeSelectorHelper;

        private readonly Func<HtmlDocument, string, IEnumerable<HtmlNode>> getListOfNodes = (document, selector) =>
        {
            if (document == null) return new List<HtmlNode>();

            return document.DocumentNode.SelectNodes(selector) ?? new HtmlNodeCollection(null);
        };

        public OdsluchaneEuResponseParser(ICollectionParser<Track> trackCollectionParser, ICollectionParser<TrackHistory> trackHistoryCollectionParser, ISelectorHelper<HtmlNode> nodeSelectorHelper, ICollectionParser<KeyValuePair<RadioStation, Track>> currentTracksCollectionParser)
        {
            this.trackCollectionParser = trackCollectionParser;
            this.trackHistoryCollectionParser = trackHistoryCollectionParser;
            this.nodeSelectorHelper = nodeSelectorHelper;
            this.currentTracksCollectionParser = currentTracksCollectionParser;
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

        private IList<HtmlNode> SelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            return getListOfNodes(document, "//ul[@class='panel_aktualnie']/li").ToList();
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
            var rows = SelectCurrentlyBroadcastedTracks(document);

            return currentTracksCollectionParser.Parse(rows, new CurrentlyBroadcastedTrackParser())
                .ToDictionary(key => key.Key, value => value.Value);
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
    }
}
