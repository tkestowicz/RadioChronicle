using System.Collections.Generic;
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
            return document.DocumentNode.SelectNodes("//select[@name='r']/optgroup");
        }

        private IEnumerable<HtmlNode> SelectListWithRadioStationsFromHtmlGroup(HtmlNode radioStationGroup)
        {
            return radioStationGroup.SelectNodes("option");
        }

        public IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document)
        {
            var result = new List<RadioStationGroup>();

            foreach (var radioStationGroup in SelectListWithGroupedRadioStationsFromHTMLDocument(document))
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

            foreach (var radioStation in radioStations)
            {
                var radioName = radioStation.Attributes.SingleOrDefault(a => a.Name == "label");
                var radioId = radioStation.Attributes.SingleOrDefault(a => a.Name == "value");

                result.Add(new RadioStation()
                {
                    Id = (radioId != null) ? int.Parse(radioId.Value) : 0,
                    Name = (radioName != null) ? radioName.Value : ""
                });
            }

            return result;
        }
    }
}
