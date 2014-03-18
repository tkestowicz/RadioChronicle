using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRowParser<out TResult>
        where TResult : class
    {
        HtmlNode GroupNode { set; }

        TResult Parse(HtmlNode node);
    }
}