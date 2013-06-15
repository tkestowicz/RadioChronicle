using System;
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
        private readonly ISpecifiedDOMParser<RadioStationGroup, HtmlNode> _radioStationGroupParser;
        private readonly ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlNode>> _radioStationParser;
        private readonly ISpecifiedDOMParser<DateTime, OdsluchaneEuDateParserArgs> _dateParser;
        private readonly ITrackHistoryParser _trackHistoryParser;
        private readonly ITrackParser _trackParser;

        public OdsluchaneEuDOMParser(IDOMSelector domSelector, ISpecifiedDOMParser<RadioStationGroup, HtmlNode> radioStationGroupParser, ISpecifiedDOMParser<RadioStation, IEnumerable<HtmlNode>> radioStationParser, ISpecifiedDOMParser<DateTime, OdsluchaneEuDateParserArgs> dateParser, ITrackHistoryParser trackHistoryParser, ITrackParser trackParser)
        {
            _domSelector = domSelector;
            _radioStationGroupParser = radioStationGroupParser;
            _radioStationParser = radioStationParser;
            _dateParser = dateParser;
            _trackHistoryParser = trackHistoryParser;
            _trackParser = trackParser;
        }

        public IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document)
        {
            var result = new List<RadioStationGroup>();

            var radioStationGroups = _domSelector.SelectRadioStationGroups(document);

            foreach (var radioStationGroup in radioStationGroups)
            {
                var parsedRadioStationGroup = _radioStationGroupParser.Parse(radioStationGroup);
                if(parsedRadioStationGroup.IsNotEmpty()) result.Add(parsedRadioStationGroup);
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
                var track = _ParseRowToObject(mostPopularTrack, cellsInRow, _trackParser, Track.Empty);
                if(track.IsNotEmpty()) result.Add(track);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectNewestTracks(HtmlDocument document)
        {
            const int cellsInRow = 3;

            var items = _ParseSearchResultsToCollection(_trackParser, document, Track.Empty, cellsInRow);

            return items.Select(item => item as Track).ToList();
        }

        public IDictionary<RadioStation, Track> ParseDOMAndSelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            var result = new Dictionary<RadioStation, Track>();

            var retrievedTracks = _domSelector.SelectCurrentlyBroadcastedTracks(document);

            foreach (var track in retrievedTracks)
            {
                var parsedRow = _ParseDOMAndReturnCurrentlyBroadcastedTrack(track);

                if (parsedRow.Value.IsNotEmpty()) result.Add(parsedRow.Key, parsedRow.Value);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectBroadcastHistory(HtmlDocument document)
        {
            const int cellsInRow = 3;

            var result = new List<Track>();

            var retrievedBroadcastHistory = _domSelector.SelectSearchResults(document);

            var trackWasBroadcasted = _dateParser.Parse(new OdsluchaneEuDateParserArgs()
            {
                DateFormat = OdsluchaneEuDateParser.DateTimePattern.BroadcastedShortDate,
                StringToParse = _domSelector.SelectSelectedDate(document)
            });

            foreach (var retrievedRow in retrievedBroadcastHistory)
            {
                if (_domSelector.CheckIfRowIsAGroupHeader(retrievedRow))
                    continue;

                _trackParser.DateWhenTrackWasBroadcasted = trackWasBroadcasted;
                var track = _ParseRowToObject(retrievedRow, cellsInRow, _trackParser, Track.Empty);

                if(track.IsNotEmpty()) result.Add(track);
            }

            return result;
        }

        public IEnumerable<TrackHistory> ParseDOMAndSelectTrackHistory(HtmlDocument document)
        {
            const int cellsInRow = 2;

            var items = _ParseSearchResultsToCollection(_trackHistoryParser, document, TrackHistory.Empty, cellsInRow);

            return items.Select(item => item as TrackHistory).ToList();
        }

        private IEnumerable<IModel> _ParseSearchResultsToCollection(ISpecifiedDOMParser<IModel, IEnumerable<HtmlNode>> parser, HtmlDocument document, IModel emptyEntity, int expectedCellsInRow)
        {
            var allRows = _domSelector.SelectSearchResults(document);

            var result = new List<IModel>();

            var dateWhenTrackWasBroadcasted = new DateTime();
            foreach (var row in allRows)
            {
                if (_domSelector.CheckIfRowIsAGroupHeader(row))
                {
                    _SetBroadcastedDate(parser, _dateParser.Parse(new OdsluchaneEuDateParserArgs()
                    {
                        DateFormat = OdsluchaneEuDateParser.DateTimePattern.BroadcastedShortDate,
                        StringToParse = _domSelector.SelectGroupHeader(row)
                    }));
                    continue;
                }

                

                var record = _ParseRowToObject(row, expectedCellsInRow, parser, emptyEntity);

                if (record.IsNotEmpty()) result.Add(record);
            }

            return result;
        }

        private void _SetBroadcastedDate(ISpecifiedDOMParser<IModel, IEnumerable<HtmlNode>> parser, DateTime dateWhenTrackWasBroadcasted)
        {
            if (parser is ITrackHistoryParser)
                (parser as ITrackHistoryParser).DateWhenTrackWasBroadcasted = dateWhenTrackWasBroadcasted;

            else if (parser is ITrackParser)
                (parser as ITrackParser).DateWhenTrackWasBroadcasted = dateWhenTrackWasBroadcasted;
        }

        private KeyValuePair<RadioStation, Track> _ParseDOMAndReturnCurrentlyBroadcastedTrack(HtmlNode track)
        {
            try
            {
                var key = _radioStationParser.Parse(_domSelector.SelectChildNodes(track));

                var value = _trackParser.Parse(_domSelector.SelectChildNodes(_domSelector.SelectUlElements(track).FirstOrDefault()));

                return new KeyValuePair<RadioStation, Track>(key, value);
            }
            catch
            {
                return new KeyValuePair<RadioStation, Track>(RadioStation.Empty, Track.Empty);
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
