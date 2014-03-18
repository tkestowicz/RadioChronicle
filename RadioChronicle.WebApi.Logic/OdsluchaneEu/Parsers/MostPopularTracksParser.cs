using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class MostPopularTracksParser : IParser<HtmlDocument, IEnumerable<Track>>
    {
        private readonly ISelectorHelper<HtmlDocument> selectorHelper;
        private readonly TrackParser popularTrackParser;

        public MostPopularTracksParser(ISelectorHelper<HtmlDocument> selectorHelper, TrackParser popularTrackParser)
        {
            this.selectorHelper = selectorHelper;
            this.popularTrackParser = popularTrackParser;
        }

        #region Implementation of IParser<out IEnumerable<Track>>

        public IEnumerable<Track> Parse(HtmlDocument node)
        {
            var mostPopularTracks = selectorHelper.GetListOfNodes(node, "//table[@class='wyniki']/tr");

            return mostPopularTracks
                .Select(mostPopularTrack => popularTrackParser.Parse(mostPopularTrack))
                .Where(track => track.Equals(Track.Empty) == false).ToList();
        }

        #endregion
    }
}