using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces
{
    public interface IOdsluchaneEuResponseHelper
    {
        string HeaderValue(HtmlNode row);

        bool IsGroupHeader(HtmlNode row);

        string SelectedDate(HtmlNode row);
    }
}
