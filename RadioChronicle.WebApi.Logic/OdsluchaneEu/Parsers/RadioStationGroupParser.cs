using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
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