using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{
    public class OdsluchaneEuTrackHistoryParser: ITrackHistoryParser
    {
        private TrackHistory _parsedTrackHistory;
        private readonly ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlNode>> _radioStationParser;
        private readonly ISpecifiedDOMParser<DateTime, OdsluchaneEuDateParserArgs> _dateParser;
        private const int _IndexOfTrackBroadcastedTimeElement = 0;

        public OdsluchaneEuTrackHistoryParser(ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlNode>> radioStationParser, ISpecifiedDOMParser<DateTime, OdsluchaneEuDateParserArgs> dateParser)
        {
            _radioStationParser = radioStationParser;
            _dateParser = dateParser;
        }

        #region Implementation of ISpecifiedDOMParser<out TrackHistory,in IEnumerable<HtmlNode>>

        public TrackHistory Parse(IEnumerable<HtmlNode> input)
        {
            _parsedTrackHistory = new TrackHistory();
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

        #region Implementation of ITrackHistoryParser

        public DateTime? DateWhenTrackWasBroadcasted { get; set; }

        #endregion

        private void _ParseRadioStation(IEnumerable<HtmlNode> input)
        {
            _parsedTrackHistory.RadioStation = _radioStationParser.Parse(input);
        }

        private void _ParseBroadcastedDateTime(string broadcastedTime)
        {
            if (DateWhenTrackWasBroadcasted.HasValue == false) return;

            var stringToParse = string.Format("{0} {1}", DateWhenTrackWasBroadcasted.Value.ToString(OdsluchaneEuDateParser.BroadcastedShortDatePattern), broadcastedTime);

            _parsedTrackHistory.Broadcasted = _dateParser.Parse(new OdsluchaneEuDateParserArgs(){ DateFormat = OdsluchaneEuDateParser.DateTimePattern.BroadcastedDateTime, StringToParse = stringToParse});
        }
    }
}