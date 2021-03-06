using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public abstract class TrackParser : IRowParser< Track>
    {
        private readonly IHtmlDocumentHelper htmlDocumentHelper;

        protected abstract int NumberOfCellsInRow { get; }

        protected virtual int IndexOfTrackNameElement { get { return 1; } }

        protected abstract Track ParseAdditionalDetails(Track track, IList<HtmlNode> cellsWithTrackDetails);

        protected TrackParser(IHtmlDocumentHelper htmlDocumentHelper)
        {
            this.htmlDocumentHelper = htmlDocumentHelper;
        }

        protected string ParseTrackName(IList<HtmlNode> parsedCells)
        {
            return htmlDocumentHelper.DecodeHtml(parsedCells[IndexOfTrackNameElement].InnerText);
        }

        protected string ParseTrackUrl(HtmlNode urlCell)
        {
            return urlCell.ChildNodes.Single().Attributes["href"].Value;
        }

        #region Implementation of IParser<out Track>

        public HtmlNode GroupNode { protected get; set; }

        public Track Parse(HtmlNode node)
        {
            try
            {
                var track = Track.Empty;

                var tableCells = htmlDocumentHelper.GetListOfNodes(node, "td");

                if (tableCells.Count != NumberOfCellsInRow) return track;

                track.Name = ParseTrackName(tableCells) ?? track.Name;

                return ParseAdditionalDetails(track, tableCells);
            }
            catch
            {
                return Track.Empty;
            }
        }

        #endregion
    }
}