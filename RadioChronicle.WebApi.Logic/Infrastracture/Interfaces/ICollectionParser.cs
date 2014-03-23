using System.Collections.Generic;
using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface ICollectionParser<TRowType>
    {
        IEnumerable<TRowType> Parse(IList<HtmlNode> rows, IRowParser<TRowType> rowParser);
    }
}