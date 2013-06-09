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

        internal interface ISpecifiedDOMParser<out TReturned, in TType>
                                                        where TType : class
        {
            TReturned Parse(TType input);
        }

        internal class DateParser : ISpecifiedDOMParser<DateTime, string>
        {
            private const string _BroadcastedShortDatePattern = "dd-MM-yyyy";
            private const string _BroadcastedDateTimePattern = "dd-MM-yyyy HH:mm";
            private const string _DefaultDateTimePattern = _BroadcastedDateTimePattern;

            private readonly string _dateTimePattern;

            internal enum DateTimePattern
            {
                BroadcastedShortDate,
                BroadcastedDateTime
            }

            internal DateParser()
            {
                _dateTimePattern = _DefaultDateTimePattern;
            }

            internal DateParser(DateTimePattern replaceDateTimePattern)
            {
                switch (replaceDateTimePattern)
                {
                    case DateTimePattern.BroadcastedShortDate:
                        _dateTimePattern = _BroadcastedShortDatePattern;
                        break;
                    case DateTimePattern.BroadcastedDateTime:
                        _dateTimePattern = _BroadcastedDateTimePattern;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("replaceDateTimePattern");
                }
                
            }

            #region Implementation of ISpecifiedDOMParser<out DateTime,in string>

            public DateTime Parse(string input)
            {
                var outputDateTime = new DateTime();

                DateTime.TryParseExact(input, _dateTimePattern, null, DateTimeStyles.None, out outputDateTime);

                return outputDateTime;
            }

            #endregion
        }

        internal class TrackParser : ISpecifiedDOMParser<Track, IEnumerable<HtmlNode>>
        {
            private const int _IndexOfTrackNameElement = 1;
            private const int _IndexOfTrackRelativeUrlElement = _IndexOfTrackNameElement;
            private const int _IndexOfTrackTimesPlayedElement = 2;
            

            private readonly Track _parsedTrack = Track.Empty;

            #region Implementation of ISpecifiedDOMParser<Track, IEnumerable<HtmlNode>>

            public Track Parse(IEnumerable<HtmlNode> input)
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

        internal class TrackHistoryParser: ISpecifiedDOMParser<TrackHistory, IEnumerable<HtmlNode>>
        {
            private readonly DateTime _dateWhenTrackWasBroadcasted;
            private readonly TrackHistory _parsedTrackHistory = new TrackHistory();
            private const int _IndexOfTrackBroadcastedTimeElement = 0;
            private const string _BroadcastedShortDatePattern = "dd-MM-yyyy";

            internal TrackHistoryParser(DateTime dateWhenTrackWasBroadcasted)
            {
                _dateWhenTrackWasBroadcasted = dateWhenTrackWasBroadcasted;
            }

            #region Implementation of ISpecifiedDOMParser<out TrackHistory,in IEnumerable<HtmlNode>>

            public TrackHistory Parse(IEnumerable<HtmlNode> input)
            {

                try
                {
                    _ParseBroadcastedDateTime(input.ElementAt(_IndexOfTrackBroadcastedTimeElement).InnerText);
                }
                catch
                {
                }

                return _parsedTrackHistory;
            }

            #endregion

            private void _ParseBroadcastedDateTime(string broadcastedTime)
            {
                var stringToParse = string.Format("{0} {1}", _dateWhenTrackWasBroadcasted.ToString(_BroadcastedShortDatePattern), broadcastedTime);

                _parsedTrackHistory.Broadcasted = new DateParser().Parse(stringToParse);
            }
        }

        internal class RadioStationParser : ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlNode>>
        {
            private readonly RadioStation _parsedRadioStation = new RadioStation();
            private const int _IndexOfRadioStationElementInCurrentlyBroadcastedList = 0;
            private const int _IndexOfRadioStationElementInTrackHistoryList = 1;


            #region Implementation of ISpecifiedDOMParser<out RadioStation,in IEnumerable<HtmlNode>>

            public RadioStation Parse(IEnumerable<HtmlNode> input)
            {
                try
                {
                    _ParseName(input.ElementAt(_DetermineCallContext(input)));
                }
                catch
                {
                }

                return _parsedRadioStation;
            }

            private int _DetermineCallContext(IEnumerable<HtmlNode> htmlNodes)
            {
                const int contextIsTrackHistory = 2;
                
                return htmlNodes.Count() == contextIsTrackHistory ? _IndexOfRadioStationElementInTrackHistoryList : _IndexOfRadioStationElementInCurrentlyBroadcastedList;
            }

            public RadioStation Parse(IEnumerable<HtmlAttribute> input)
            {
                try
                {
                    _ParseId(input);
                    _ParseName(input);
                    _ParseIsDefault(input);
                }
                catch
                {
                }

                return _parsedRadioStation;
            }

            #endregion

            private void _ParseId(IEnumerable<HtmlAttribute> attributes)
            {
                var radioId = attributes.SingleOrDefault(a => a.Name == "value");

                if (radioId != null) _parsedRadioStation.Id = int.Parse(radioId.Value);
            }

            private void _ParseIsDefault(IEnumerable<HtmlAttribute> attributes)
            {
                var isSelected = attributes.SingleOrDefault(a => a.Name == "selected");

                _parsedRadioStation.IsDefault = isSelected != null && isSelected.Value == "selected";
            }

            private void _ParseName(IEnumerable<HtmlAttribute> attributes)
            {
                var radioName = attributes.SingleOrDefault(a => a.Name == "label");

                if(radioName != null) _parsedRadioStation.Name = radioName.Value;
            }

            private void _ParseName(HtmlNode cell)
            {
                _parsedRadioStation.Name = cell.InnerText.Trim();
            }
        } 

        internal class RadioStationGroupParser : ISpecifiedDOMParser<RadioStationGroup, HtmlNode>
        {
            private const int _IndexOfRadioGroupNameElement = 0;

            private readonly IDOMSelector _domSelector;

            private readonly RadioStationGroup _parsedRadioStationGroup = new RadioStationGroup();

            public RadioStationGroupParser(IDOMSelector domSelector)
            {
                _domSelector = domSelector;
            }

            #region Implementation of ISpecifiedDOMParser<out RadioStationGroup,in IEnumerable<HtmlNode>>

            public RadioStationGroup Parse(HtmlNode input)
            {
                try
                {
                    _ParseGroupName(input.Attributes.ElementAt(_IndexOfRadioGroupNameElement));
                    _ParseRadioStations(_domSelector.SelectRadioStations(input));
                }
                catch
                {
                }

                return _parsedRadioStationGroup;
            }

            #endregion

            private void _ParseRadioStations(IEnumerable<HtmlNode> rows)
            {
                var result = new List<RadioStation>();

                if (rows != null)
                {
                    foreach (var radioStation in rows)
                    {
                        if (!radioStation.Attributes.Any()) continue;

                        result.Add(new RadioStationParser().Parse(radioStation.Attributes));
                    }
                }

                _parsedRadioStationGroup.RadioStations = result;
            }

            private void _ParseGroupName(HtmlAttribute attribute)
            {
                _parsedRadioStationGroup.Name = attribute.Value;
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
                result.Add(new RadioStationGroupParser(_domSelector).Parse(radioStationGroup));
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

            var currentGroup = new DateTime();
            foreach (var resultRow in results)
            {
                if (_domSelector.CheckIfRowIsAGroupHeader(resultRow))
                {
                    currentGroup =
                        new DateParser(DateParser.DateTimePattern.BroadcastedShortDate).Parse(
                            _domSelector.SelectGroupHeader(resultRow));
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

            var trackWasBroadcasted = new DateParser(DateParser.DateTimePattern.BroadcastedShortDate).Parse(_domSelector.SelectSelectedDate(document));
            foreach (var retrievedRow in retrievedBroadcastHistory)
            {
                if (_domSelector.CheckIfRowIsAGroupHeader(retrievedRow))
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
                if (_domSelector.CheckIfRowIsAGroupHeader(retrievedRow))
                {
                    currentGroup =
                        new DateParser(DateParser.DateTimePattern.BroadcastedShortDate).Parse(
                            _domSelector.SelectGroupHeader(retrievedRow));
                    continue;
                }

                var trackHistory = _ParseDOMAndReturnTrackHistory(retrievedRow, currentGroup);

                if(new TrackHistory().Equals(trackHistory) == false) result.Add(trackHistory);
            }

            return result;
        }

        private TrackHistory _ParseDOMAndReturnTrackHistory(HtmlNode retrievedRow, DateTime dateWhenTrackWasBroadcasted)
        {
            const int cellsInRow = 2;

            try{

                var tableCells = _domSelector.SelectTableCells(retrievedRow);

                if (tableCells == null || tableCells.Count() != cellsInRow) return new TrackHistory();

                var trackHistory = new TrackHistoryParser(dateWhenTrackWasBroadcasted).Parse(tableCells);

                trackHistory.RadioStation = new RadioStationParser().Parse(tableCells);

                return trackHistory;
            }
            catch
            {
                return new TrackHistory();
            }
        }

        private Track _ParseDOMAndReturnTrackFromSearchResults(HtmlNode resultRow, DateTime dateWhenTrackWasBroadcasted)
        {
            const int cellsInRow = 3;

            try
            {
                var track = Track.Empty;

                var tableCells = _domSelector.SelectTableCells(resultRow);

                if (tableCells == null || tableCells.Count() != cellsInRow) return track;

                track = new TrackParser().Parse(tableCells);

                track.TrackHistory = new List<TrackHistory> { new TrackHistoryParser(dateWhenTrackWasBroadcasted).Parse(tableCells) };


                return track;
            }
            catch
            {
                return Track.Empty;
            }
        }

        private KeyValuePair<RadioStation, Track> _ParseDOMAndReturnCurrentlyBroadcastedTrack(HtmlNode track)
        {
            const int trackInfoElementIndex = 1;

            try
            {
                var key = new RadioStationParser().Parse(track.ChildNodes);

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

            var tableCells = _domSelector.SelectTableCells(mostPopularTrack);

            if (tableCells == null || tableCells.Count() != cellsInRow) return track;

            track = new TrackParser().Parse(tableCells);

            return track;
        }
    }
}
