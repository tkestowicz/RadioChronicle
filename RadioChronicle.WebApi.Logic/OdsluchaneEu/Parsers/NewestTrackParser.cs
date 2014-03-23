using System;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class NewestTrackParser : TrackParser, INewestTrackParser
    {
        private readonly IOdsluchaneEuResponseHelper responseHelper;

        public NewestTrackParser(IHtmlDocumentHelper htmlDocumentHelper, IOdsluchaneEuResponseHelper responseHelper) : base(htmlDocumentHelper)
        {
            this.responseHelper = responseHelper;
        }

        private DateTime? DateWhenTrackWasBroadcastedFirstTime
        {
            get
            {
                DateTime outputDateTime;
                if (TryParseShortDateFromString(responseHelper.HeaderValue(GroupNode), out outputDateTime))
                    return outputDateTime;

                return new DateTime?();
            }
        }

        private bool TryParseShortDateFromString(string stringAsDate, out DateTime outputDateTime)
        {
            const string ShortDatePattern = "dd-MM-yyyy";

            return DateTime.TryParseExact(stringAsDate, ShortDatePattern, null, DateTimeStyles.None, out outputDateTime);
        }

        #region Overrides of TrackParser

        protected override int NumberOfCellsInRow
        {
            get { return 3; }
        }

        protected override Track ParseAdditionalDetails(Track track, IList<HtmlNode> cellsWithTrackDetails)
        {
            const int trackPlayedFirstTimeElement = 0;

            track.RelativeUrlToTrackDetails = ParseTrackUrl(cellsWithTrackDetails[IndexOfTrackNameElement]);

            if (DateWhenTrackWasBroadcastedFirstTime.HasValue)
            {
                DateTime playedFirstTime;
                var stringToParse = string.Format("{0} {1}", DateWhenTrackWasBroadcastedFirstTime.Value.ToShortDateString(),
                    cellsWithTrackDetails[trackPlayedFirstTimeElement].InnerText);

                if (DateTime.TryParse(stringToParse, out playedFirstTime))
                    track.PlayedFirstTime = playedFirstTime;
            }

            return track;
        }

        #endregion
    }
}