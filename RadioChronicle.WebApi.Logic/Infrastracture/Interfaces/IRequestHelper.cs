using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRequestHelper
    {
        /// <summary>
        /// Sends HTTP request to the specified URL.
        /// </summary>
        /// <param name="url">URL which has to be requested.</param>
        /// <returns>Server response as a HTML document.</returns>
        HtmlDocument RequestUrl(string url);
    }
}