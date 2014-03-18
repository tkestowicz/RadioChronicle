using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class TrackCollectionParser : ICollectionParser<Track>, ICollectionParser<TrackHistory>
    {
        //TODO: refactor
        private string SelectGroupHeader(HtmlNode row)
        {
            var header = row.SelectSingleNode("td[@class='line']");

            return (header == null) ? string.Empty : header.InnerText;
        }

        private bool CheckIfRowIsAGroupHeader(HtmlNode row)
        {
            return string.IsNullOrEmpty(SelectGroupHeader(row)) == false;
        }

        private IEnumerable<TTest> Parse<TTest>(IEnumerable<HtmlNode> rows, IRowParser<TTest> rowParser, Func<TTest, bool> isEmpty)
            where TTest: class 
        {
            foreach (var row in rows)
            {
                if (CheckIfRowIsAGroupHeader(row))
                {
                    rowParser.GroupNode = row;
                    continue;
                }

                var result = rowParser.Parse(row);

                if (isEmpty(result)) yield return result;
            }
        } 

        #region Implementation of ICollectionParser<Track>

        public IEnumerable<Track> Parse(IList<HtmlNode> rows, IRowParser<Track> rowParser)
        {
            return Parse(rows, rowParser, track => track.Equals(Track.Empty) == false);
        }

        #endregion

        #region Implementation of ICollectionParser<TrackHistory>

        public IEnumerable<TrackHistory> Parse(IList<HtmlNode> rows, IRowParser<TrackHistory> rowParser)
        {
            return Parse(rows, rowParser, trackHistory => new TrackHistory().Equals(trackHistory) == false);
        }

        #endregion
    }

    public class RadioStationCollectionParser : ICollectionParser<RadioStationGroup>, ICollectionParser<RadioStation>
    {
        #region Implementation of ICollectionParser<RadioStationGroup>

        public IEnumerable<RadioStationGroup> Parse(IList<HtmlNode> rows, IRowParser<RadioStationGroup> rowParser)
        {
            return rows.Select(rowParser.Parse);
        }

        #endregion

        #region Implementation of ICollectionParser<RadioStation>

        public IEnumerable<RadioStation> Parse(IList<HtmlNode> rows, IRowParser<RadioStation> rowParser)
        {
            return from row in rows 
                   where row.Attributes.Any() 
                   select rowParser.Parse(row);
        }

        #endregion
    }

    public class RadioStationParser : IRowParser<RadioStation>
    {
        #region Implementation of IRowParser<out RadioStation>

        public HtmlNode GroupNode { set; private get; }

        public RadioStation Parse(HtmlNode node)
        {
            var radioName = node.Attributes.SingleOrDefault(a => a.Name == "label");
            var radioId = node.Attributes.SingleOrDefault(a => a.Name == "value");

            var isSelected = node.Attributes.SingleOrDefault(a => a.Name == "selected");
            var isDefault = isSelected != null && isSelected.Value == "selected";

            return new RadioStation()
            {
                Id = (radioId != null) ? int.Parse(radioId.Value) : 0,
                Name = (radioName != null) ? radioName.Value : "",
                IsDefault = isDefault
            };
        }

        #endregion
    }

    public class RadioStationGroupParser : IRowParser<RadioStationGroup>
    {
        private readonly ICollectionParser<RadioStation> radioStationsCollectionParser;
        private readonly IRowParser<RadioStation> radioStationParser;

        public RadioStationGroupParser(ICollectionParser<RadioStation> radioStationsCollectionParser, IRowParser<RadioStation> radioStationParser)
        {
            this.radioStationsCollectionParser = radioStationsCollectionParser;
            this.radioStationParser = radioStationParser;
        }

        private IEnumerable<HtmlNode> SelectListWithRadioStationsFromHtmlGroup(HtmlNode radioStationGroup)
        {
            return radioStationGroup.SelectNodes("option") as IList<HtmlNode> ?? new HtmlNode[0];
        }

        #region Implementation of IRowParser<out RadioStationGroup>

        public HtmlNode GroupNode { set; private get; }

        public RadioStationGroup Parse(HtmlNode node)
        {
            var radioStations = radioStationsCollectionParser.Parse(SelectListWithRadioStationsFromHtmlGroup(node).ToList(), radioStationParser);

            return new RadioStationGroup()
            {
                GroupName = node.Attributes[0].Value,
                RadioStations = radioStations
            };
        }

        #endregion
    }
}