using System;
using System.Globalization;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class TrackHistoryParser : IRowParser<TrackHistory>
    {
        //TODO: refactor
        private bool TryParseDateTimeFromString(string stringToParse, out DateTime outputDateTime)
        {
            const string ShortDatePattern = "dd-MM-yyyy H:mm";

            return DateTime.TryParseExact(stringToParse, ShortDatePattern, null, DateTimeStyles.None, out outputDateTime);
        }

        //TODO: refactor
        private string SelectGroupHeader(HtmlNode row)
        {
            var header = row.SelectSingleNode("td[@class='line']");

            return (header == null) ? string.Empty : header.InnerText;
        }

        #region Overrides of TrackParser

        protected int NumberOfCellsInRow { get { return 2; } }

        #endregion

        #region Implementation of IRowParser<out TrackHistory>

        public HtmlNode GroupNode { set; private get; }

        //TODO: refactor if possible
        public TrackHistory Parse(HtmlNode node)
        {
            const int trackBroadcastedTimeElement = 0;
            const int radioStationElement = 1;

            try
            {
                var trackHistory = new TrackHistory();

                var tableCells = node.SelectNodes("td");

                if (tableCells == null || tableCells.Count != NumberOfCellsInRow) return trackHistory;

                trackHistory.RadioStation = new RadioStation() { Name = tableCells[radioStationElement].InnerText };

                DateTime broadcastedDateTime;
                var stringToParse = string.Format("{0} {1}", SelectGroupHeader(GroupNode),
                    tableCells[trackBroadcastedTimeElement].InnerText);
                if (TryParseDateTimeFromString(stringToParse, out broadcastedDateTime))
                    trackHistory.Broadcasted = broadcastedDateTime;

                return trackHistory;
            }
            catch
            {
                return new TrackHistory();
            }
        }

        #endregion
    }
}