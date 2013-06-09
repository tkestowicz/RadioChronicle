using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuDOMParser : IDOMParser
    {

        internal interface IDOMParser<out TReturned, in TType> where TReturned : class
                                                        where TType : class
        {
            TReturned Parse(TType input);
        }

        internal class TrackParser : IDOMParser<Track, ICollection<HtmlNode>>
        {
            private const int _IndexOfTrackNameElement = 1;
            private const int _IndexOfTrackRelativeUrlElement = _IndexOfTrackNameElement;
            private const int _IndexOfTrackTimesPlayedElement = 2;

            private readonly Track _parsedTrack = Track.Empty;

            #region Implementation of IDOMParser<Track,HtmlNode>

            public Track Parse(ICollection<HtmlNode> input)
            {     
                _ParseName(input.ElementAt(_IndexOfTrackNameElement));
                _ParseRelativeUrl(input.ElementAt(_IndexOfTrackRelativeUrlElement));
                _ParseTimesPlayed(input.ElementAt(_IndexOfTrackTimesPlayedElement));

                return _parsedTrack;
           }

            #endregion

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

        private readonly IDOMSelector _domSelector;

        public OdsluchaneEuDOMParser(IDOMSelector domSelector)
        {
            _domSelector = domSelector;
        }

        public IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document)
        {
            var result = new List<RadioStationGroup>();

            var radioStationGroups = _domSelector.SelectRadioStationGroups(document);

            if (radioStationGroups == null) return new List<RadioStationGroup>();

            foreach (var radioStationGroup in radioStationGroups)
            {
                var radioStations = _ParseDOMAndSelectRadioStations(_domSelector.SelectRadioStations(radioStationGroup));
                result.Add(new RadioStationGroup()
                {
                    GroupName = radioStationGroup.Attributes[0].Value,
                    RadioStations = radioStations
                });
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectMostPopularTracks(HtmlDocument document)
        {
            var result = new List<Track>();
            var mostPopularTracks = _domSelector.SelectSearchResults(document);

            if(mostPopularTracks == null) return result;

            foreach (var mostPopularTrack in mostPopularTracks)
            {
                var track = _ParseDOMAndReturnMostPopularTrack(mostPopularTrack);
                if(track.Equals(Track.Empty) == false) result.Add(track);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectNewestTracks(HtmlDocument document)
        {
            var result = new List<Track>();

            var results = _domSelector.SelectSearchResults(document);

            var currentGroup = string.Empty;
            foreach (var resultRow in results)
            {
                if (_CheckIfRowIsAGroupHeader(resultRow))
                {
                    currentGroup = _domSelector.SelectGroupHeader(resultRow);
                    continue;
                }
                

                var track = _ParseDOMAndReturnTrackFromSearchResults(resultRow, currentGroup);

                if(track.Equals(Track.Empty) == false) result.Add(track);

            }

            return result;
        }

        public IDictionary<RadioStation, Track> ParseDOMAndSelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            var result = new Dictionary<RadioStation, Track>();

            var retrievedTracks = _domSelector.SelectCurrentlyBroadcastedTracks(document);

            foreach (var track in retrievedTracks)
            {
                var parsedRow = _ParseDOMAndReturnCurrentlyBroadcastedTrack(track);

                if (parsedRow.Value.Equals(Track.Empty) == false) result.Add(parsedRow.Key, parsedRow.Value);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectBroadcastHistory(HtmlDocument document)
        {
            var result = new List<Track>();

            var retrievedBroadcastHistory = _domSelector.SelectSearchResults(document);

            var trackWasBroadcasted = _domSelector.SelectSelectedDate(document);
            foreach (var retrievedRow in retrievedBroadcastHistory)
            {
                if (_CheckIfRowIsAGroupHeader(retrievedRow))
                    continue;

                var track = _ParseDOMAndReturnTrackFromSearchResults(retrievedRow, trackWasBroadcasted);

                if(track.Equals(Track.Empty) == false) result.Add(track);
            }

            return result;
        }

        public IEnumerable<TrackHistory> ParseDOMAndSelectTrackHistory(HtmlDocument document)
        {
            var result = new List<TrackHistory>();

            var retrievedTrackHistory = _domSelector.SelectSearchResults(document);
            
            var currentGroup = new DateTime();
            foreach (var retrievedRow in retrievedTrackHistory)
            {
                if (_CheckIfRowIsAGroupHeader(retrievedRow) && TryParseShortDateFromString(_domSelector.SelectGroupHeader(retrievedRow), out currentGroup))
                        continue;

                var trackHistory = _ParseDOMAndReturnTrackHistory(retrievedRow, currentGroup);

                if(new TrackHistory().Equals(trackHistory) == false) result.Add(trackHistory);
            }

            return result;
        }

        private TrackHistory _ParseDOMAndReturnTrackHistory(HtmlNode retrievedRow, DateTime dateWhenTrackWasBroadcasted)
        {
            const int trackBroadcastedTimeElement = 0;
            const int radioStationElement = 1;
            const int cellsInRow = 2;

            try
            {
                var trackHistory = new TrackHistory();

                var tableCells = retrievedRow.SelectNodes("td");

                if (tableCells == null || tableCells.Count != cellsInRow) return trackHistory;

                trackHistory.RadioStation = new RadioStation() { Name = tableCells[radioStationElement].InnerText };

                DateTime broadcastedDateTime;
                var stringToParse = string.Format("{0} {1}", dateWhenTrackWasBroadcasted.ToShortDateString(),
                    tableCells[trackBroadcastedTimeElement].InnerText);
                if (TryParseDateTimeFromString(stringToParse, out broadcastedDateTime, "yyyy-MM-dd HH:mm"))
                    trackHistory.Broadcasted = broadcastedDateTime;

                return trackHistory;
            }
            catch
            {
                return new TrackHistory();
            }
        }

        private IEnumerable<RadioStation> _ParseDOMAndSelectRadioStations(IEnumerable<HtmlNode> radioStations)
        {
            var result = new List<RadioStation>();

            if (radioStations == null) return result;

            foreach (var radioStation in radioStations)
            {
                if (!radioStation.Attributes.Any()) continue;

                var radioName = radioStation.Attributes.SingleOrDefault(a => a.Name == "label");
                var radioId = radioStation.Attributes.SingleOrDefault(a => a.Name == "value");

                var isSelected = radioStation.Attributes.SingleOrDefault(a => a.Name == "selected");
                var isDefault = isSelected != null && isSelected.Value == "selected";

                result.Add(new RadioStation()
                {
                    Id = (radioId != null) ? int.Parse(radioId.Value) : 0,
                    Name = (radioName != null) ? radioName.Value : "",
                    IsDefault = isDefault
                });
            }

            return result;
        }

        private Track _ParseDOMAndReturnTrackFromSearchResults(HtmlNode resultRow, string dateWhenTrackWasBroadcasted)
        {
            const int trackBroadcastedTimeElement = 0;
            const int cellsInRow = 3;

            try
            {
                var track = Track.Empty;

                var tableCells = resultRow.SelectNodes("td");

                if (tableCells == null || tableCells.Count != cellsInRow) return track;

                track = new TrackParser().Parse(tableCells);

                DateTime broadcastedDateTime;
                var stringToParse = string.Format("{0} {1}", dateWhenTrackWasBroadcasted,
                    tableCells[trackBroadcastedTimeElement].InnerText);

                if (TryParseDateTimeFromString(stringToParse, out broadcastedDateTime, "dd-MM-yyyy HH:mm"))
                {
                    track.TrackHistory = new List<TrackHistory>{ new TrackHistory(){ Broadcasted = broadcastedDateTime} };
                    track.PlayedFirstTime = broadcastedDateTime;
                }

                return track;
            }
            catch
            {
                return Track.Empty;
            }
        }

        private KeyValuePair<RadioStation, Track> _ParseDOMAndReturnCurrentlyBroadcastedTrack(HtmlNode track)
        {
            const int radioNameElementIndex = 0;
            const int trackInfoElementIndex = 1;

            try
            {
                var key = new RadioStation()
                {
                    Id = 0,
                    IsDefault = false,
                    Name = track.ChildNodes[radioNameElementIndex].InnerText.Trim()
                };

                var value = new TrackParser().Parse(track.ChildNodes[trackInfoElementIndex].ChildNodes);

                return new KeyValuePair<RadioStation, Track>(key, value);
            }
            catch
            {
                return new KeyValuePair<RadioStation, Track>(null, Track.Empty);
            }
        }

        private Track _ParseDOMAndReturnMostPopularTrack(HtmlNode mostPopularTrack)
        {
            const int cellsInRow = 4;

            var track = Track.Empty;

            var tableCells = mostPopularTrack.SelectNodes("td");

            if (tableCells == null || tableCells.Count != cellsInRow) return track;

            track = new TrackParser().Parse(tableCells);

            return track;
        }

        private bool TryParseShortDateFromString(string stringAsDate, out DateTime outputDateTime)
        {
            const string ShortDatePattern = "dd-MM-yyyy";

            return DateTime.TryParseExact(stringAsDate, ShortDatePattern, null, DateTimeStyles.None, out outputDateTime);
        }

        private bool TryParseDateTimeFromString(string stringToParse, out DateTime outputDateTime, string dateTimePattern)
        {
            return DateTime.TryParseExact(stringToParse, dateTimePattern, null, DateTimeStyles.None, out outputDateTime);
        }

        private bool _CheckIfRowIsAGroupHeader(HtmlNode row)
        {
            return string.IsNullOrEmpty(_domSelector.SelectGroupHeader(row)) == false;
        }
    }
}
