using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{
    public class OdsluchaneEuTrackHistoryParser: ISpecifiedDOMParser<TrackHistory, IEnumerable<HtmlNode>>
    {
        private readonly DateTime _dateWhenTrackWasBroadcasted;
        private readonly TrackHistory _parsedTrackHistory = new TrackHistory();
        private const int _IndexOfTrackBroadcastedTimeElement = 0;
        private const string _BroadcastedShortDatePattern = "dd-MM-yyyy";

        internal OdsluchaneEuTrackHistoryParser(DateTime dateWhenTrackWasBroadcasted)
        {
            _dateWhenTrackWasBroadcasted = dateWhenTrackWasBroadcasted;
        }

        #region Implementation of ISpecifiedDOMParser<out TrackHistory,in IEnumerable<HtmlNode>>

        public TrackHistory Parse(IEnumerable<HtmlNode> input)
        {

            try
            {
                _ParseBroadcastedDateTime(input.ElementAt(_IndexOfTrackBroadcastedTimeElement).InnerText);
                _ParseRadioStation(input);
            }
            catch
            {
            }

            return _parsedTrackHistory;
        }

        #endregion

        private void _ParseRadioStation(IEnumerable<HtmlNode> input)
        {
            _parsedTrackHistory.RadioStation = new OdsluchaneEuRadioStationParser().Parse(input);
        }

        private void _ParseBroadcastedDateTime(string broadcastedTime)
        {
            var stringToParse = string.Format("{0} {1}", _dateWhenTrackWasBroadcasted.ToString(_BroadcastedShortDatePattern), broadcastedTime);

            _parsedTrackHistory.Broadcasted = new OdsluchaneEuDateParser().Parse(stringToParse);
        }
    }
}