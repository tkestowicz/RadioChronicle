using System.Collections.Generic;
using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IDOMSelector
    {
        IEnumerable<HtmlNode> SelectRadioStationGroups(HtmlDocument document);
        IEnumerable<HtmlNode> SelectRadioStations(HtmlNode radioStationGroup);
        IEnumerable<HtmlNode> SelectSearchResults(HtmlDocument document);
        IEnumerable<HtmlNode> SelectCurrentlyBroadcastedTracks(HtmlDocument document);
        IEnumerable<HtmlNode> SelectTableCells(HtmlNode tableRow);
        IEnumerable<HtmlNode> SelectUlElements(HtmlNode node);
        IEnumerable<HtmlNode> SelectChildNodes(HtmlNode node); 
        string SelectGroupHeader(HtmlNode row);
        string SelectSelectedDate(HtmlDocument document);
        bool CheckIfRowIsAGroupHeader(HtmlNode row);
    }
}
