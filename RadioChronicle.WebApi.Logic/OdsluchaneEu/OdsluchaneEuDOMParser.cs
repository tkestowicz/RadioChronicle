﻿using System.Collections.Generic;
using System.Linq;
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

        private IEnumerable<HtmlNode> SelectListWithMostPopularTracks(HtmlDocument document)
        {
            if(document == null) return new List<HtmlNode>();

            var tableRows = document.DocumentNode.SelectNodes("//table[@class='wyniki']/tr");

            if(tableRows == null) return new List<HtmlNode>();

            // skip first element which is a result header
            return tableRows.Skip(1);
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
            var mostPopularTracks = SelectListWithMostPopularTracks(document);

            if(mostPopularTracks == null) return result;

            foreach (var mostPopularTrack in mostPopularTracks)
            {
                var track = ParseDOMAndReturnMostPopularTrack(mostPopularTrack);
                if(track.Equals(Track.Empty) == false) result.Add(track);
            }

            return result;
        }

        private Track ParseDOMAndReturnMostPopularTrack(HtmlNode mostPopularTrack)
        {
            const int trackNameElement = 1;
            const int trackTimesPlayedElement = 2;
            const int cellsInRow = 4;

            var track = Track.Empty;

            var tableCells = mostPopularTrack.SelectNodes("td");

            if (tableCells == null || tableCells.Count != cellsInRow) return track;

            track.Name = tableCells[trackNameElement].InnerText ?? track.Name;

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
    }
}
