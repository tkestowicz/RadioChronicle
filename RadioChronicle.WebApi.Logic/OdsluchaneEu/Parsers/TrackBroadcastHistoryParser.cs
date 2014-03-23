using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class TrackBroadcastHistoryParser : TrackParser, ITrackBroadcastHistoryParser
    {
        private readonly IOdsluchaneEuResponseHelper responseHelper;

        public TrackBroadcastHistoryParser(IHtmlDocumentHelper htmlDocumentHelper, IOdsluchaneEuResponseHelper responseHelper)
            : base(htmlDocumentHelper)
        {
            this.responseHelper = responseHelper;
        }

        #region Overrides of TrackParser

        protected override int NumberOfCellsInRow { get { return 3; } }

        protected override Track ParseAdditionalDetails(Track track, IList<HtmlNode> cellsWithTrackDetails)
        {
            const int trackBroadcastedTimeElement = 0;

            track.RelativeUrlToTrackDetails = ParseTrackUrl(cellsWithTrackDetails[IndexOfTrackNameElement]);

            DateTime broadcastedDateTime;
            var stringToParse = string.Format("{0} {1}", responseHelper.SelectedDate(GroupNode),
                cellsWithTrackDetails[trackBroadcastedTimeElement].InnerText);

            if (DateTime.TryParse(stringToParse, out broadcastedDateTime))
                track.TrackHistory = new List<TrackHistory> { new TrackHistory() { Broadcasted = broadcastedDateTime } };

            return track;
        }

        #endregion
    }
}