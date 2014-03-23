using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class RadioStationGroupParser : IRadioGroupParser
    {
        private readonly IRadioStationCollectionParser radioStationsCollectionParser;
        private readonly IRadioStationParser radioStationParser;

        public RadioStationGroupParser(IRadioStationCollectionParser radioStationsCollectionParser, IRadioStationParser radioStationParser)
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