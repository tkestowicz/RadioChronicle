using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuRemoteServiceAdapter : IRemoteServiceStrategy
    {
        private readonly IRequestHelper _requestHelper;
        private readonly IUrlRepository _urlRepository;
        private readonly IDOMParser _domParser;

        public OdsluchaneEuRemoteServiceAdapter(IRequestHelper requestHelper, IUrlRepository urlRepository, IDOMParser domParser)
        {
            _requestHelper = requestHelper;
            _urlRepository = urlRepository;
            _domParser = domParser;
        }

        public IEnumerable<RadioStationGroup> GetRadioStations()
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(_requestHelper.RequestURL(_urlRepository.RadioStationsPage.Value));

            return _domParser.ParseDOMAndSelectRadioStationGroups(doc);
        }
    }
}