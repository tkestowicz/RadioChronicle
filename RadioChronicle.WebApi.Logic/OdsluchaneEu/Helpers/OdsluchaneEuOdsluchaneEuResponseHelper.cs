using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Helpers
{
    public class OdsluchaneEuOdsluchaneEuResponseHelper : IOdsluchaneEuResponseHelper
    {
        private readonly IXPathSelectorsRepository pathSelectorsRepository;

        public OdsluchaneEuOdsluchaneEuResponseHelper(IXPathSelectorsRepository pathSelectorsRepository)
        {
            this.pathSelectorsRepository = pathSelectorsRepository;
        }

        #region Implementation of IOdsluchaneEuResponseHelper

        public string HeaderValue(HtmlNode row)
        {
            var header = row.SelectSingleNode(pathSelectorsRepository.Header);

            return (header == null) ? string.Empty : header.InnerText;
        }

        public bool IsGroupHeader(HtmlNode row)
        {
            return string.IsNullOrEmpty(HeaderValue(row)) == false;
        }

        public string SelectedDate(HtmlNode row)
        {
            if (row == null) return string.Empty;

            var items = row.SelectSingleNode(pathSelectorsRepository.SelectedDate);

            if (items == null) return string.Empty;

            return items.Attributes["value"].Value;
        }

        #endregion
    }
}