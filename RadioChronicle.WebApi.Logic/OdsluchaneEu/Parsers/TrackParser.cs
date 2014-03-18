using System.Collections.Generic;
using System.Web;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public abstract class TrackParser : IParser<HtmlNode, Track>
    {
        private readonly ISelectorHelper<HtmlNode> selectorHelper;

        protected abstract int NumberOfCellsInRow { get; }

        protected abstract int IndexOfTrackNameElement { get; }

        protected abstract Track ParseAdditionalDetails(Track track, IList<HtmlNode> cellsWithTrackDetails);

        protected TrackParser(ISelectorHelper<HtmlNode> selectorHelper)
        {
            this.selectorHelper = selectorHelper;
        }

        #region Implementation of IParser<out Track>

        public Track Parse(HtmlNode node)
        {
            var track = Track.Empty;

            var tableCells = selectorHelper.GetListOfNodes(node, "td");

            if (tableCells.Count != NumberOfCellsInRow) return track;

            track.Name = HttpUtility.HtmlDecode(tableCells[IndexOfTrackNameElement].InnerText) ?? track.Name;

            return ParseAdditionalDetails(track, tableCells);
        }

        #endregion
    }
}