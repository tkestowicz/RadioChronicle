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

            return document.DocumentNode.SelectNodes("//select[@name='r']/optgroup");
        }

        public IEnumerable<HtmlNode> SelectRadioStations(HtmlNode radioStationGroup)
        {
            return radioStationGroup.SelectNodes("option");
        }

        public IEnumerable<HtmlNode> SelectSearchResults(HtmlDocument document)
        {
            if (document == null) return _EmptyListOfNodes;

            var tableRows = document.DocumentNode.SelectNodes("//table[@class='wyniki']/tr");

            if (tableRows == null) return _EmptyListOfNodes;

            // skip first element which is a result header
            return tableRows.Skip(1);
        }

        public IEnumerable<HtmlNode> SelectCurrentlyBroadcastedTracks(HtmlDocument document)
        {
            if (document == null) return _EmptyListOfNodes;

            var items = document.DocumentNode.SelectNodes("//ul[@class='panel_aktualnie']/li");

            if (items == null) return _EmptyListOfNodes;

            return items;
        }

        public string SelectGroupHeader(HtmlNode row)
        {
            var header = row.SelectSingleNode("td[@class='line']");
            return (header == null)? "" : header.InnerText;
        }

        public string SelectSelectedDate(HtmlDocument document)
        {
            if(document == null) return string.Empty;

            var items = document.DocumentNode.SelectSingleNode("//input[@name='date']");

            if (items == null) return string.Empty;

            return items.Attributes["value"].Value;
        }
    }
}