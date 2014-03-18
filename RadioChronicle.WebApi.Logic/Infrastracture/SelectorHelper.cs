using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.Infrastracture
{
    public class SelectorHelper : ISelectorHelper<HtmlNode>, ISelectorHelper<HtmlDocument>
    {
        private static readonly IList<HtmlNode> DefaultCollection = new List<HtmlNode>(0); 

        #region Implementation of ISelectorHelper<in HtmlDocument>

        public IList<HtmlNode> GetListOfNodes(HtmlDocument node, string selector)
        {
            if (node == null) return DefaultCollection;

            return node.DocumentNode.SelectNodes(selector) ?? DefaultCollection;
        }

        #endregion

        #region Implementation of ISelectorHelper<in HtmlNode>

        public IList<HtmlNode> GetListOfNodes(HtmlNode node, string selector)
        {
            return node.SelectNodes(selector) ?? DefaultCollection;
        }

        #endregion
    }
}