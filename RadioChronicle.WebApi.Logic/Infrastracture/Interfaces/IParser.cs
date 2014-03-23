using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRowParser<out TResult>
    {
        HtmlNode GroupNode { set; }

        TResult Parse(HtmlNode node);
    }
}