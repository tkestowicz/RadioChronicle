using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class RadioStationCollectionParser : IRadioStationCollectionParser
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
}