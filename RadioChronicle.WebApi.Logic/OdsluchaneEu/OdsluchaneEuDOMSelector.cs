using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuDOMSelector : IDOMSelector
    {
        public OdsluchaneEuDOMSelector()
        {
        }

        private static IEnumerable<HtmlNode> _EmptyListOfNodes
        {
            get { return new List<HtmlNode>(); }
        }

        public IEnumerable<HtmlNode> SelectRadioStationGroups(HtmlDocument document)
        {
            if (document == null) return _EmptyListOfNodes;

            var items = document.DocumentNode.SelectNodes("//select[@name='r']/optgroup");

            return items ?? _EmptyListOfNodes;
        }

        public IEnumerable<HtmlNode> SelectRadioStations(HtmlNode radioStationGroup)
        {
            if (radioStationGroup == null) return _EmptyListOfNodes;

            return radioStationGroup.SelectNodes("option") ?? _EmptyListOfNodes;
        }

        public IEnumerable<HtmlNode> SelectSearchResults(HtmlDocument document)
        {
            if (document == null) return _EmptyListOfNodes;

            var tableRows = document.DocumentNode.SelectNodes("//table[@class='wyniki']/tr");

            // skip first element which is a result header
            return tableRows == null ? _EmptyListOfNodes : tableRows.Skip(1);
        }

        public IEnumerable<HtmlNode> SelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            if (document == null) return _EmptyListOfNodes;

            var items = document.DocumentNode.SelectNodes("//ul[@class='panel_aktualnie']/li");

            return items ?? _EmptyListOfNodes;
        }

        public IEnumerable<HtmlNode> SelectTableCells(HtmlNode tableRow)
        {
            if (tableRow == null) return _EmptyListOfNodes;

            var cells = tableRow.SelectNodes("td");

            return cells ?? _EmptyListOfNodes;
        }

        public string SelectGroupHeader(HtmlNode row)
        {
            var header = row.SelectSingleNode("td[@class='line']");
            return (header == null)? string.Empty : header.InnerText;
        }

        public string SelectSelectedDate(HtmlDocument document)
        {
            if(document == null) return string.Empty;

            var items = document.DocumentNode.SelectSingleNode("//input[@name='date']");

            return items == null ? string.Empty : items.Attributes["value"].Value;
        }

        public bool CheckIfRowIsAGroupHeader(HtmlNode row)
        {
            return string.IsNullOrEmpty(SelectGroupHeader(row)) == false;
        }
    }
}