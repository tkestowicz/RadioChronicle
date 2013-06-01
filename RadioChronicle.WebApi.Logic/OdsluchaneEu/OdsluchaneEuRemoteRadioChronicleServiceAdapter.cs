using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuRemoteRadioChronicleServiceAdapter : IRemoteRadioChronicleService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly IUrlRepository _urlRepository;
        private readonly IDOMParser _domParser;

        public OdsluchaneEuRemoteRadioChronicleServiceAdapter(IRequestHelper requestHelper, IUrlRepository urlRepository, IDOMParser domParser)
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

        public IEnumerable<Track> GetMostPopularTracks(RadioStation radioStation, int month, int year)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(_requestHelper.RequestURL(_urlRepository.MostPopularTracksPage(radioStation.Id, month, year).Value));

            return _domParser.ParseDOMAndSelectMostPopularTracks(doc).Take(10).ToList();
        }
    }
}