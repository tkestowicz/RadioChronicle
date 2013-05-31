using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IDOMParser
    {
        IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document);
    }
}