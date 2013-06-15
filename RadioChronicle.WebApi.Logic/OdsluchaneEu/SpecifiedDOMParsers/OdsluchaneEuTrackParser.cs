using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{
    public class OdsluchaneEuTrackParser : ITrackParser
    {
        private const int _IndexOfTrackNameElement = 1;
        private const int _IndexOfTrackRelativeUrlElement = _IndexOfTrackNameElement;
        private const int _IndexOfTrackTimesPlayedElement = 2;
        
        private Track _parsedTrack;
        private readonly ITrackHistoryParser _trackHistoryParser;

        public OdsluchaneEuTrackParser(ITrackHistoryParser trackHistoryParser)
        {
            _trackHistoryParser = trackHistoryParser;
        }

        #region Implementation of ISpecifiedDOMParser<Track, IEnumerable<HtmlNode>>

        public Track Parse(IEnumerable<HtmlNode> input)
        {
            _parsedTrack = Track.Empty;

            try
            {
                _ParseName(input.ElementAt(_IndexOfTrackNameElement));
                _ParseRelativeUrl(input.ElementAt(_IndexOfTrackRelativeUrlElement));
                _ParseTimesPlayed(input.ElementAt(_IndexOfTrackTimesPlayedElement));
                _ParseTrackHistory(input);
            }
            catch
            {
            }

            return _parsedTrack;
        }

        #endregion

        private void _ParseTrackHistory(IEnumerable<HtmlNode> input)
        {
            if (DateWhenTrackWasBroadcasted.HasValue)
            {
                _parsedTrack.TrackHistory = new List<TrackHistory>()
                {
                    _trackHistoryParser.Parse(input)
                };
            }
        }

        private void _ParseName(HtmlNode cell)
        {
            _parsedTrack.Name = HttpUtility.HtmlDecode(cell.InnerText) ?? string.Empty;
        }

        private void _ParseRelativeUrl(HtmlNode cell)
        {
            try
            {
                _parsedTrack.RelativeUrlToTrackDetails = cell.ChildNodes.Single().Attributes["href"].Value;
            }
            catch
            {
            }
        }

        private void _ParseTimesPlayed(HtmlNode cell)
        {
            int timesPlayed;
            if (int.TryParse(cell.InnerText, out timesPlayed))
                _parsedTrack.TimesPlayed = timesPlayed;
        }

        #region Implementation of ITrackParser

        public DateTime? DateWhenTrackWasBroadcasted
        {
            get { return _trackHistoryParser.DateWhenTrackWasBroadcasted; }
            set { _trackHistoryParser.DateWhenTrackWasBroadcasted = value; }
        }

        #endregion
    }
}