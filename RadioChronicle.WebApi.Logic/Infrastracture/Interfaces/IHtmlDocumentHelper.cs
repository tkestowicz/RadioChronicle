using System.Collections.Generic;
using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IHtmlDocumentHelper
    {
        IList<HtmlNode> GetListOfNodes(HtmlNode node, string selector);

        IList<HtmlNode> GetListOfNodes(HtmlDocument node, string selector);

        string DecodeHtml(string input);
    }
}