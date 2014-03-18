using System.Collections.Generic;
using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface ISelectorHelper<in TNodeType>
    {
        IList<HtmlNode> GetListOfNodes(TNodeType node, string selector);
    }
}