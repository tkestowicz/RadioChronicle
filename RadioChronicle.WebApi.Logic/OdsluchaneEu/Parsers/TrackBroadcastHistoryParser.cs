using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class TrackBroadcastHistoryParser : TrackParser
    {
        public TrackBroadcastHistoryParser(ISelectorHelper<HtmlNode> selectorHelper)
            : base(selectorHelper)
        {
        }

        //TODO: refactor
        private bool TryParseDateTimeFromString(string stringToParse, out DateTime outputDateTime)
        {
            return DateTime.TryParse(stringToParse, out outputDateTime);
        }

        //TODO: refactor
        private string SelectSelectedDate(HtmlNode node)
        {
            if (node == null) return string.Empty;

            var items = node.SelectSingleNode("//input[@name='date']");

            if (items == null) return string.Empty;

            return items.Attributes["value"].Value;
        }

        #region Overrides of TrackParser

        protected override int NumberOfCellsInRow { get { return 3; } }

        protected override Track ParseAdditionalDetails(Track track, IList<HtmlNode> cellsWithTrackDetails)
        {
            const int trackBroadcastedTimeElement = 0;

            track.RelativeUrlToTrackDetails = ParseTrackUrl(cellsWithTrackDetails[IndexOfTrackNameElement]);

            DateTime broadcastedDateTime;
            var stringToParse = string.Format("{0} {1}", SelectSelectedDate(GroupNode),
                cellsWithTrackDetails[trackBroadcastedTimeElement].InnerText);

            if (TryParseDateTimeFromString(stringToParse, out broadcastedDateTime))
                track.TrackHistory = new List<TrackHistory> { new TrackHistory() { Broadcasted = broadcastedDateTime } };

            return track;
        }

        #endregion
    }
}