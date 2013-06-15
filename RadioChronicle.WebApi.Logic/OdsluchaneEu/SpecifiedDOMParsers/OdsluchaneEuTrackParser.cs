using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{
    public class OdsluchaneEuTrackParser : ISpecifiedDOMParser<Track, IEnumerable<HtmlNode>>
    {
        private readonly DateTime? _dateWhenTrackWasBroadcasted;
        private const int _IndexOfTrackNameElement = 1;
        private const int _IndexOfTrackRelativeUrlElement = _IndexOfTrackNameElement;
        private const int _IndexOfTrackTimesPlayedElement = 2;


        private readonly Track _parsedTrack = Track.Empty;

        internal OdsluchaneEuTrackParser()
        {
        }

        internal OdsluchaneEuTrackParser(DateTime dateWhenTrackWasBroadcasted)
        {
            _dateWhenTrackWasBroadcasted = dateWhenTrackWasBroadcasted;
        }

        #region Implementation of ISpecifiedDOMParser<Track, IEnumerable<HtmlNode>>

        public Track Parse(IEnumerable<HtmlNode> input)
        {
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
            if (_dateWhenTrackWasBroadcasted.HasValue)
            {
                _parsedTrack.TrackHistory = new List<TrackHistory>()
                {
                    new OdsluchaneEuTrackHistoryParser(_dateWhenTrackWasBroadcasted.Value).Parse(input)
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
    }
}