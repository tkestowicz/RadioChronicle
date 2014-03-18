using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class MostPopularTrackParser : TrackParser
    {
        public MostPopularTrackParser(ISelectorHelper<HtmlNode> selectorHelper) : base(selectorHelper)
        {
        }

        #region Overrides of TrackParser

        protected override int NumberOfCellsInRow { get { return 4;  } }

        protected override Track ParseAdditionalDetails(Track track, IList<HtmlNode> cellsWithTrackDetails)
        {
            const int trackTimesPlayedElement = 2;

            track.RelativeUrlToTrackDetails = ParseTrackUrl(cellsWithTrackDetails[IndexOfTrackNameElement]);

            int timesPlayed;
            if (int.TryParse(cellsWithTrackDetails[trackTimesPlayedElement].InnerText, out timesPlayed))
                track.TimesPlayed = timesPlayed;

            return track;
        }

        #endregion
    }
}