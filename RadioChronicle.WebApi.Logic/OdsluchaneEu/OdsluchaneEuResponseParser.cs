using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuResponseParser : IResponseParser
    {

        private readonly ITrackCollectionParser trackCollectionParser;
        private readonly IHtmlDocumentHelper htmlDocumentHelper;
        private readonly IXPathSelectorsRepository selectorsRepository;
        private readonly IComponentContext resolver;

        public OdsluchaneEuResponseParser(IComponentContext resolver)
        {
            this.trackCollectionParser = resolver.Resolve<ITrackCollectionParser>();
            this.htmlDocumentHelper = resolver.Resolve<IHtmlDocumentHelper>();
            this.selectorsRepository = resolver.Resolve<IXPathSelectorsRepository>();
            this.resolver = resolver;
        }

        private TResult ParseAndSelect<TResult, TParser>(HtmlDocument document, string selector, TParser parser)
        {
            var rows = htmlDocumentHelper.GetListOfNodes(document, selector);

            return trackCollectionParser.Parse(rows, parser);
        }

        public IEnumerable<RadioStationGroup> ParseAndSelectRadioStationGroups(HtmlDocument document)
        {
            var parser = resolver.Resolve<IRadioGroupParser>();

            var rows = htmlDocumentHelper.GetListOfNodes(document, selectorsRepository.ListOfRadioStations);

            return rows.Select(parser.Parse);
        }

        public IEnumerable<Track> ParseAndSelectMostPopularTracks(HtmlDocument document)
        {
            var rows = htmlDocumentHelper.GetListOfNodes(document, selectorsRepository.ListOfTracks);

            return trackCollectionParser.Parse(rows, resolver.Resolve<IMostPopularTrackParser>());
        }

        public IEnumerable<Track> ParseAndSelectNewestTracks(HtmlDocument document)
        {
            var rows = htmlDocumentHelper.GetListOfNodes(document, selectorsRepository.ListOfTracks);

            return trackCollectionParser.Parse(rows, resolver.Resolve<INewestTrackParser>());
        }

        public IDictionary<RadioStation, Track> ParseAndSelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            var rows = htmlDocumentHelper.GetListOfNodes(document, selectorsRepository.ListOfCurrentlyBroadcastedTracks);

            return trackCollectionParser.Parse(rows, resolver.Resolve<ICurrentlyBroadcastedTrack>())
                .ToDictionary(key => key.Key, value => value.Value);
        }

        public IEnumerable<Track> ParseAndSelectBroadcastHistory(HtmlDocument document)
        {
            var rows = htmlDocumentHelper.GetListOfNodes(document, selectorsRepository.ListOfTracks);

            return trackCollectionParser.Parse(rows, resolver.Resolve<ITrackBroadcastHistoryParser>());
        }

        public IEnumerable<TrackHistory> ParseAndSelectTrackHistory(HtmlDocument document)
        {
            var rows = htmlDocumentHelper.GetListOfNodes(document, selectorsRepository.ListOfTracks);

            return trackCollectionParser.Parse(rows, resolver.Resolve<ITrackHistoryParser>());
        }
    }
}
