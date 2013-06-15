using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuDOMParser : IDOMParser
    {
        private readonly IDOMSelector _domSelector;

        public OdsluchaneEuDOMParser(IDOMSelector domSelector)
        {
            _domSelector = domSelector;
        }

        public IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document)
        {
            var result = new List<RadioStationGroup>();

            var radioStationGroups = _domSelector.SelectRadioStationGroups(document);

            foreach (var radioStationGroup in radioStationGroups)
            {
                result.Add(new OdsluchaneEuRadioStationGroupParser(_domSelector).Parse(radioStationGroup));
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectMostPopularTracks(HtmlDocument document)
        {
            const int cellsInRow = 4;

            var result = new List<Track>();
            var mostPopularTracks = _domSelector.SelectSearchResults(document);

            foreach (var mostPopularTrack in mostPopularTracks)
            {
                var track = _ParseRowToObject(mostPopularTrack, cellsInRow, new OdsluchaneEuTrackParser(), Track.Empty);
                if(track.Equals(Track.Empty) == false) result.Add(track);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectNewestTracks(HtmlDocument document)
        {
            const int cellsInRow = 3;

            var result = new List<Track>();

            var results = _domSelector.SelectSearchResults(document);

            var currentGroup = new DateTime();
            foreach (var resultRow in results)
            {
                if (_domSelector.CheckIfRowIsAGroupHeader(resultRow))
                {
                    currentGroup =
                        new OdsluchaneEuDateParser(OdsluchaneEuDateParser.DateTimePattern.BroadcastedShortDate).Parse(
                            _domSelector.SelectGroupHeader(resultRow));
                    continue;
                }


                var track = _ParseRowToObject(resultRow, cellsInRow, new OdsluchaneEuTrackParser(currentGroup), Track.Empty);

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
            const int cellsInRow = 3;

            var result = new List<Track>();

            var retrievedBroadcastHistory = _domSelector.SelectSearchResults(document);

            var trackWasBroadcasted = new OdsluchaneEuDateParser(OdsluchaneEuDateParser.DateTimePattern.BroadcastedShortDate).Parse(_domSelector.SelectSelectedDate(document));
            foreach (var retrievedRow in retrievedBroadcastHistory)
            {
                if (_domSelector.CheckIfRowIsAGroupHeader(retrievedRow))
                    continue;

                var track = _ParseRowToObject(retrievedRow, cellsInRow, new OdsluchaneEuTrackParser(trackWasBroadcasted), Track.Empty);

                if(track.Equals(Track.Empty) == false) result.Add(track);
            }

            return result;
        }

        public IEnumerable<TrackHistory> ParseDOMAndSelectTrackHistory(HtmlDocument document)
        {
            const int cellsInRow = 2;

            var result = new List<TrackHistory>();

            var retrievedTrackHistory = _domSelector.SelectSearchResults(document);
            
            var currentGroup = new DateTime();
            foreach (var retrievedRow in retrievedTrackHistory)
            {
                if (_domSelector.CheckIfRowIsAGroupHeader(retrievedRow))
                {
                    currentGroup =
                        new OdsluchaneEuDateParser(OdsluchaneEuDateParser.DateTimePattern.BroadcastedShortDate).Parse(
                            _domSelector.SelectGroupHeader(retrievedRow));
                    continue;
                }

                var trackHistory = _ParseRowToObject(retrievedRow, cellsInRow, new OdsluchaneEuTrackHistoryParser(currentGroup), new TrackHistory());

                if(new TrackHistory().Equals(trackHistory) == false) result.Add(trackHistory);
            }

            return result;
        }

        private KeyValuePair<RadioStation, Track> _ParseDOMAndReturnCurrentlyBroadcastedTrack(HtmlNode track)
        {
            try
            {
                var key = new OdsluchaneEuRadioStationParser().Parse(_domSelector.SelectChildNodes(track));

                var value = new OdsluchaneEuTrackParser().Parse(_domSelector.SelectChildNodes(_domSelector.SelectUlElements(track).FirstOrDefault()));

                return new KeyValuePair<RadioStation, Track>(key, value);
            }
            catch
            {
                return new KeyValuePair<RadioStation, Track>(new RadioStation(), Track.Empty);
            }
        }

        private TResult _ParseRowToObject<TResult>(HtmlNode row, int expectedCellsInRow,
            ISpecifiedDOMParser<TResult, IEnumerable<HtmlNode>> parser, TResult emptyRecord)
        {
            var tableCells = _domSelector.SelectTableCells(row);

            if (tableCells.HasExpectedNumberOfElements(expectedCellsInRow))
                return parser.Parse(tableCells);

            return emptyRecord;
        }
    }
}
