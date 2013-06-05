using System;
using System.Collections.Generic;
using System.Linq;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuRemoteRadioChronicleServiceAdapter : IRemoteRadioChronicleService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly IUrlRepository _urlRepository;
        private readonly IDOMParser _domParser;
        private readonly IRemoteServiceArgumentsValidator _argumentsValidator;

        public OdsluchaneEuRemoteRadioChronicleServiceAdapter(IRequestHelper requestHelper, IUrlRepository urlRepository, IDOMParser domParser, IRemoteServiceArgumentsValidator argumentsValidator)
        {
            _requestHelper = requestHelper;
            _urlRepository = urlRepository;
            _domParser = domParser;
            _argumentsValidator = argumentsValidator;
        }

        public int DefaultYear
        {
            get { return ApplicationTime.Current.Year; }
        }

        public int DefaultMonth
        {
            get{ return ApplicationTime.Current.Month; }
        }

        public RadioStation DefaultRadioStation
        {
            get
            {
                try
                {
                    return GetRadioStations()
                        .Single(g => g.RadioStations.SingleOrDefault(r => r.IsDefault) != null)
                        .RadioStations.Single(r => r.IsDefault);
                }
                catch (Exception)
                {
                    return new RadioStation() {Id = 0, IsDefault = false, Name = ""};
                }
            }
        }

        public IEnumerable<RadioStationGroup> GetRadioStations()
        {
            var doc = _requestHelper.RequestURL(_urlRepository.RadioStationsPage.Value);

            return _domParser.ParseDOMAndSelectRadioStationGroups(doc);
        }

        public IEnumerable<Track> GetMostPopularTracks(int radioStationId, int month, int year)
        {
            VerifyAllAndSetDefaults(ref radioStationId, ref month, ref year);

            var doc = _requestHelper.RequestURL(_urlRepository.MostPopularTracksPage(radioStationId, month, year).Value);

            return _domParser.ParseDOMAndSelectMostPopularTracks(doc).Take(10).ToList();
        }

        public IEnumerable<Track> GetMostRecentTracks(int radioStationId)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStationId);

            var doc = _requestHelper.RequestURL(_urlRepository.MostRecentTracksPage(radioStationId).Value);

            return _domParser.ParseDOMAndSelectMostRecentTracks(doc).Take(10).ToList();
        }

        private void VerifyRadioStationIdAndSetDefault(ref int radioStationId)
        {
            if (!_argumentsValidator.IsRadioStationIdValid(GetRadioStations(), radioStationId)) radioStationId = DefaultRadioStation.Id;
        }

        private void VerifyAllAndSetDefaults(ref int radioStationId, ref int month, ref int year)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStationId);
            if (!_argumentsValidator.IsMonthValid(month)) month = DefaultMonth;
            if (!_argumentsValidator.IsYearValid(year)) year = DefaultYear;
        }


        public IDictionary<RadioStation, Track> GetCurrentlyBroadcastedTracks()
        {
            var doc = _requestHelper.RequestURL(_urlRepository.CurrentlyBroadcastedTrack.Value);

            return _domParser.ParseDOMAndSelectCurrentlyBroadcastedTracks(doc).OrderBy(e => e.Key.Name).ToDictionary(k => k.Key, v => v.Value);
        }
    }
}