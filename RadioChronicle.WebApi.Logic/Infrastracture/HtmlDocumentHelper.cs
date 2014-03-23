using System.Collections.Generic;
using System.Web;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.Infrastracture
{
    public class HtmlDocumentHelper : IHtmlDocumentHelper
    {
        private static readonly IList<HtmlNode> DefaultCollection = new List<HtmlNode>(0); 

        #region Implementation of IHtmlDocumentHelper<in HtmlDocument>

        public IList<HtmlNode> GetListOfNodes(HtmlDocument node, string selector)
        {
            if (node == null) return DefaultCollection;

            return node.DocumentNode.SelectNodes(selector) ?? DefaultCollection;
        }

        public string DecodeHtml(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        #endregion

        #region Implementation of IHtmlDocumentHelper<in HtmlNode>

        public IList<HtmlNode> GetListOfNodes(HtmlNode node, string selector)
        {
            return node.SelectNodes(selector) ?? DefaultCollection;
        }

        #endregion
    }
}