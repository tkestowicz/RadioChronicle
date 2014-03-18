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
        private readonly IResponseParser responseParser;
        private readonly IRemoteServiceArgumentsValidator _argumentsValidator;

        public OdsluchaneEuRemoteRadioChronicleServiceAdapter(IRequestHelper requestHelper, IUrlRepository urlRepository, IResponseParser responseParser, IRemoteServiceArgumentsValidator argumentsValidator)
        {
            _requestHelper = requestHelper;
            _urlRepository = urlRepository;
            this.responseParser = responseParser;
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

        public int DefaultHourFrom
        {
            get { return DefaultHourTo - 2; }
        }

        public int DefaultHourTo
        {
            get { return ApplicationTime.Current.Hour; }
        }

        public IEnumerable<RadioStationGroup> GetRadioStations()
        {
            var doc = _requestHelper.RequestURL(_urlRepository.RadioStationsPage.Value);

            return responseParser.ParseDOMAndSelectRadioStationGroups(doc);
        }

        public IEnumerable<Track> GetMostPopularTracks(int radioStationId, int month, int year)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStationId);
            VerifyMonthAndYear(ref month, ref year);

            var doc = _requestHelper.RequestURL(_urlRepository.MostPopularTracksPage(radioStationId, month, year).Value);

            return responseParser.ParseDOMAndSelectMostPopularTracks(doc).Take(10).ToList();
        }

        public IEnumerable<Track> GetNewestTracks(int radioStationId)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStationId);

            var doc = _requestHelper.RequestURL(_urlRepository.NewestTracksPage(radioStationId).Value);

            return responseParser.ParseDOMAndSelectNewestTracks(doc).Take(10).ToList();
        }

        public IDictionary<RadioStation, Track> GetCurrentlyBroadcastedTracks()
        {
            var doc = _requestHelper.RequestURL(_urlRepository.CurrentlyBroadcastedTrack.Value);

            return responseParser.ParseDOMAndSelectCurrentlyBroadcastedTracks(doc).OrderBy(e => e.Key.Name).ToDictionary(k => k.Key, v => v.Value);
        }

        public IEnumerable<Track> GetBroadcastHistory(int radioStation, DateTime day, int hourFrom, int hourTo)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStation);
            VerifyHourRangeAndSetDefault(ref hourFrom, ref hourTo);

            var doc =
                _requestHelper.RequestURL(_urlRepository.BroadcastHistoryPage(radioStation, day, hourFrom, hourTo).Value);

            return
                responseParser.ParseDOMAndSelectBroadcastHistory(doc)
                    .ToList();
        }

        public IEnumerable<Track> GetMostRecentTracks(int radioStationId)
        {
            return GetBroadcastHistory(radioStationId, ApplicationTime.Current, DefaultHourFrom, DefaultHourTo).Take(10).ToList();
        }

        public IEnumerable<TrackHistory> GetTrackHistory(string relativeUrlToTrackDetails)
        {
            var doc = _requestHelper.RequestURL(_urlRepository.TrackDetailsPage(relativeUrlToTrackDetails).Value);

            return responseParser.ParseDOMAndSelectTrackHistory(doc);
        }

        private void VerifyHourRangeAndSetDefault(ref int hourFrom, ref int hourTo)
        {
            if (!_argumentsValidator.IsHourValid(hourFrom)) hourFrom = DefaultHourFrom;
            if (!_argumentsValidator.IsHourValid(hourTo)) hourTo = DefaultHourTo;
            if (!_argumentsValidator.IsRangeValid(hourFrom, hourTo))
            {
                hourFrom = DefaultHourFrom;
                hourTo = DefaultHourTo;
            }
        }

        private void VerifyRadioStationIdAndSetDefault(ref int radioStationId)
        {
            if (!_argumentsValidator.IsRadioStationIdValid(GetRadioStations(), radioStationId)) radioStationId = DefaultRadioStation.Id;
        }

        private void VerifyMonthAndYear(ref int month, ref int year)
        {
            if (!_argumentsValidator.IsMonthValid(month)) month = DefaultMonth;
            if (!_argumentsValidator.IsYearValid(year)) year = DefaultYear;
        }
    }
}