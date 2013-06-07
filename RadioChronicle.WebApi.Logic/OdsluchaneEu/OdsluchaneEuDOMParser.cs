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
        public OdsluchaneEuDOMParser()
        {
        }

        private IEnumerable<HtmlNode> SelectListWithGroupedRadioStationsFromHTMLDocument(HtmlDocument document)
        {
            if (document == null) return new List<HtmlNode>();

            return document.DocumentNode.SelectNodes("//select[@name='r']/optgroup");
        }

        private IEnumerable<HtmlNode> SelectListWithRadioStationsFromHtmlGroup(HtmlNode radioStationGroup)
        {
            return radioStationGroup.SelectNodes("option");
        }

        private IEnumerable<HtmlNode> SelectResultsListWithTracks(HtmlDocument document)
        {
            if(document == null) return new List<HtmlNode>();

            var tableRows = document.DocumentNode.SelectNodes("//table[@class='wyniki']/tr");

            if(tableRows == null) return new List<HtmlNode>();

            // skip first element which is a result header
            return tableRows.Skip(1);
        }

        private IEnumerable<HtmlNode> SelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            if (document == null) return new List<HtmlNode>();

            var items = document.DocumentNode.SelectNodes("//ul[@class='panel_aktualnie']/li");

            if (items == null) return new List<HtmlNode>();

            return items;
        }

        private string SelectGroupHeader(HtmlNode row)
        {
            var header = row.SelectSingleNode("td[@class='line']");
            return (header == null)? "" : header.InnerText;
        }

        private IEnumerable<HtmlNode> SelectBroadcastHistoryTracks(HtmlDocument document)
        {
            if(document == null) return new List<HtmlNode>();

            var items = document.DocumentNode.SelectNodes("//table[@class='wyniki']/tr");

            if(items == null) return new List<HtmlNode>();

            // skip first element which is a result header
            return items.Skip(1);
        }

        private string SelectSelectedDate(HtmlDocument document)
        {
            if(document == null) return string.Empty;

            var items = document.DocumentNode.SelectSingleNode("//input[@name='date']");

            if (items == null) return string.Empty;

            return items.Attributes["value"].Value;
        }

        public IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document)
        {
            var result = new List<RadioStationGroup>();

            var radioStationGroups = SelectListWithGroupedRadioStationsFromHTMLDocument(document);

            if (radioStationGroups == null) return result;

            foreach (var radioStationGroup in radioStationGroups)
            {
                var radioStations = ParseDOMAndSelectRadioStations(SelectListWithRadioStationsFromHtmlGroup(radioStationGroup));
                result.Add(new RadioStationGroup()
                {
                    GroupName = radioStationGroup.Attributes[0].Value,
                    RadioStations = radioStations
                });
            }

            return result;
        }

        private IEnumerable<RadioStation> ParseDOMAndSelectRadioStations(IEnumerable<HtmlNode> radioStations)
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

        public IEnumerable<Track> ParseDOMAndSelectMostPopularTracks(HtmlDocument document)
        {
            var result = new List<Track>();
            var mostPopularTracks = SelectResultsListWithTracks(document);

            if(mostPopularTracks == null) return result;

            foreach (var mostPopularTrack in mostPopularTracks)
            {
                var track = ParseDOMAndReturnMostPopularTrack(mostPopularTrack);
                if(track.Equals(Track.Empty) == false) result.Add(track);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectMostRecentTracks(HtmlDocument document)
        {
            var result = new List<Track>();

            var results = SelectResultsListWithTracks(document);

            var currentGroup = new DateTime();
            foreach (var resultRow in results)
            {
                if (CheckIfRowIsAGroupHeader(resultRow) && TryParseShortDateFromString(SelectGroupHeader(resultRow), out currentGroup))
                        continue;
                

                var track = ParseDOMAndReturnMostRecentTrack(resultRow, currentGroup);

                if(track.Equals(Track.Empty) == false) result.Add(track);

            }

            return result;
        }

        public IDictionary<RadioStation, Track> ParseDOMAndSelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            var result = new Dictionary<RadioStation, Track>();

            var retrievedTracks = SelectCurrentlyBroadcastedTracks(document);

            foreach (var track in retrievedTracks)
            {
                var parsedRow = ParseDOMAndReturnCurrentlyBroadcastedTrack(track);

                if (parsedRow.Value.Equals(Track.Empty) == false) result.Add(parsedRow.Key, parsedRow.Value);
            }

            return result;
        }

        public IEnumerable<Track> ParseDOMAndSelectBroadcastHistory(HtmlDocument document)
        {
            var result = new List<Track>();

            var retrievedBroadcastHistory = SelectBroadcastHistoryTracks(document);

            var trackWasBroadcasted = SelectSelectedDate(document);
            foreach (var retrievedRow in retrievedBroadcastHistory)
            {
                if (CheckIfRowIsAGroupHeader(retrievedRow))
                    continue;

                var track = ParseDOMAndReturnTrackFromSearchResults(retrievedRow, trackWasBroadcasted);

                if(track.Equals(Track.Empty) == false) result.Add(track);
            }

            return result;
        }

        private Track ParseDOMAndReturnTrackFromSearchResults(HtmlNode resultRow, string dateWhenTrackWasBroadcasted)
        {
            const int trackBroadcastedTimeElement = 0;
            const int trackNameElement = 1;
            const int cellsInRow = 3;

            try
            {
                var track = Track.Empty;

                var tableCells = resultRow.SelectNodes("td");

                if (tableCells == null || tableCells.Count != cellsInRow) return track;

                track.Name = HttpUtility.HtmlDecode(tableCells[trackNameElement].InnerText) ?? track.Name;

                var trackUrlDetails = tableCells[trackNameElement].ChildNodes.Single().Attributes["href"].Value;

                track.RelativeUrlToTrackDetails = trackUrlDetails;

                DateTime broadcastedDateTime;
                var stringToParse = string.Format("{0} {1}", dateWhenTrackWasBroadcasted,
                    tableCells[trackBroadcastedTimeElement].InnerText);
                if (TryParseDateTimeFromString(stringToParse, out broadcastedDateTime))
                    track.TrackHistory = new List<TrackHistory>{ new TrackHistory(){ Broadcasted = broadcastedDateTime} };

                return track;
            }
            catch
            {
                return Track.Empty;
            }
        }

        private KeyValuePair<RadioStation, Track> ParseDOMAndReturnCurrentlyBroadcastedTrack(HtmlNode track)
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

                var value = Track.Empty;


                
                var trackInfo = track.ChildNodes[trackInfoElementIndex].ChildNodes[trackInfoElementIndex];

                value.Name = HttpUtility.HtmlDecode(trackInfo.InnerText);
                value.RelativeUrlToTrackDetails = trackInfo.ChildNodes.Single().Attributes["href"].Value;

                return new KeyValuePair<RadioStation, Track>(key, value);
            }
            catch
            {
                return new KeyValuePair<RadioStation, Track>(null, Track.Empty);
            }
        }

        private Track ParseDOMAndReturnMostPopularTrack(HtmlNode mostPopularTrack)
        {
            const int trackNameElement = 1;
            const int trackTimesPlayedElement = 2;
            const int cellsInRow = 4;

            var track = Track.Empty;

            var tableCells = mostPopularTrack.SelectNodes("td");

            if (tableCells == null || tableCells.Count != cellsInRow) return track;

            track.Name = HttpUtility.HtmlDecode(tableCells[trackNameElement].InnerText) ?? track.Name;

            try
            {
                var trackUrlDetails =  tableCells[trackNameElement].ChildNodes.Single().Attributes["href"].Value;

                track.RelativeUrlToTrackDetails = trackUrlDetails;
            }
            catch
            {
                
            }

            int timesPlayed;
            if (int.TryParse(tableCells[trackTimesPlayedElement].InnerText, out timesPlayed))
                track.TimesPlayed = timesPlayed;

            return track;
        }

        private Track ParseDOMAndReturnMostRecentTrack(HtmlNode mostRecentTrack, DateTime? dateWhenTrackWasBroadcastedFirstTime = null)
        {
            const int trackPlayedFirstTimeElement = 0;
            const int trackNameElement = 1;
            const int cellsInRow = 3;
            
            try
            {
                var track = Track.Empty;

                var tableCells = mostRecentTrack.SelectNodes("td");

                if (tableCells == null || tableCells.Count != cellsInRow) return track;

                track.Name = HttpUtility.HtmlDecode(tableCells[trackNameElement].InnerText) ?? track.Name;

                var trackUrlDetails = tableCells[trackNameElement].ChildNodes.Single().Attributes["href"].Value;

                track.RelativeUrlToTrackDetails = trackUrlDetails;

                if (dateWhenTrackWasBroadcastedFirstTime.HasValue)
                {
                    DateTime playedFirstTime;
                    var stringToParse = string.Format("{0} {1}", dateWhenTrackWasBroadcastedFirstTime.Value.ToShortDateString(),
                        tableCells[trackPlayedFirstTimeElement].InnerText);
                    if (TryParseDateTimeFromString(stringToParse, out playedFirstTime))
                        track.PlayedFirstTime = playedFirstTime;
                }

                return track;
            }
            catch
            {
                return Track.Empty;
            }
        }

        private bool TryParseShortDateFromString(string stringAsDate, out DateTime outputDateTime)
        {
            const string ShortDatePattern = "dd-MM-yyyy";

            return DateTime.TryParseExact(stringAsDate, ShortDatePattern, null, DateTimeStyles.None, out outputDateTime);
        }

        private bool TryParseDateTimeFromString(string stringToParse, out DateTime outputDateTime)
        {
            //const string ShortDatePattern = "dd-MM-yyyy";

            return DateTime.TryParse(stringToParse, out outputDateTime);
        }

        private bool CheckIfRowIsAGroupHeader(HtmlNode row)
        {
            return string.IsNullOrEmpty(SelectGroupHeader(row)) == false;
        }
    }
}
