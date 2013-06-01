using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.Infrastracture
{
    /// <summary>
    /// Sends requests to the remote server.
    /// </summary>
    public class ServerRequestHelper : IRequestHelper
    {
        #region Implementation of IRequestHelper

        /// <summary>
        /// Sends HTTP request to the specified URL.
        /// </summary>
        /// <param name="url">URL which has to be requested.</param>
        /// <returns>Document with HTML response.</returns>
        public HtmlDocument RequestURL(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url);
        }

        #endregion
    }
}