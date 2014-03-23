using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuServiceAdapter : IRemoteRadioChronicleService
    {
        private readonly IRequestHelper requestHelper;
        private readonly IUrlRepository urlRepository;
        private readonly IResponseParser responseParser;
        private readonly IRemoteServiceArgumentsValidator argumentsValidator;

        public OdsluchaneEuServiceAdapter(IRequestHelper requestHelper, IUrlRepository urlRepository, IResponseParser responseParser, IRemoteServiceArgumentsValidator argumentsValidator, IComponentContext container)
        {
            this.requestHelper = requestHelper;
            this.urlRepository = urlRepository;
            this.responseParser = responseParser;
            this.argumentsValidator = argumentsValidator;
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
            var doc = requestHelper.RequestUrl(urlRepository.RadioStationsPage.Value);

            return responseParser.ParseAndSelectRadioStationGroups(doc);
        }

        public IEnumerable<Track> GetMostPopularTracks(int radioStationId, int month, int year)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStationId);
            VerifyMonthAndYear(ref month, ref year);

            var doc = requestHelper.RequestUrl(urlRepository.MostPopularTracksPage(radioStationId, month, year).Value);

            return responseParser.ParseAndSelectMostPopularTracks(doc).Take(10).ToList();
        }

        public IEnumerable<Track> GetNewestTracks(int radioStationId)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStationId);

            var doc = requestHelper.RequestUrl(urlRepository.NewestTracksPage(radioStationId).Value);

            return responseParser.ParseAndSelectNewestTracks(doc).Take(10).ToList();
        }

        public IDictionary<RadioStation, Track> GetCurrentlyBroadcastedTracks()
        {
            var doc = requestHelper.RequestUrl(urlRepository.CurrentlyBroadcastedTrack.Value);

            return responseParser.ParseAndSelectCurrentlyBroadcastedTracks(doc).OrderBy(e => e.Key.Name).ToDictionary(k => k.Key, v => v.Value);
        }

        public IEnumerable<Track> GetBroadcastHistory(int radioStation, DateTime day, int hourFrom, int hourTo)
        {
            VerifyRadioStationIdAndSetDefault(ref radioStation);
            VerifyHourRangeAndSetDefault(ref hourFrom, ref hourTo);

            var doc =
                requestHelper.RequestUrl(urlRepository.BroadcastHistoryPage(radioStation, day, hourFrom, hourTo).Value);

            return
                responseParser.ParseAndSelectBroadcastHistory(doc)
                    .ToList();
        }

        public IEnumerable<Track> GetMostRecentTracks(int radioStationId)
        {
            return GetBroadcastHistory(radioStationId, ApplicationTime.Current, DefaultHourFrom, DefaultHourTo).Take(10).ToList();
        }

        public IEnumerable<TrackHistory> GetTrackHistory(string relativeUrlToTrackDetails)
        {
            var doc = requestHelper.RequestUrl(urlRepository.TrackDetailsPage(relativeUrlToTrackDetails).Value);

            return responseParser.ParseAndSelectTrackHistory(doc);
        }

        private void VerifyHourRangeAndSetDefault(ref int hourFrom, ref int hourTo)
        {
            if (!argumentsValidator.IsHourValid(hourFrom)) hourFrom = DefaultHourFrom;
            if (!argumentsValidator.IsHourValid(hourTo)) hourTo = DefaultHourTo;
            if (!argumentsValidator.IsRangeValid(hourFrom, hourTo))
            {
                hourFrom = DefaultHourFrom;
                hourTo = DefaultHourTo;
            }
        }

        private void VerifyRadioStationIdAndSetDefault(ref int radioStationId)
        {
            if (!argumentsValidator.IsRadioStationIdValid(GetRadioStations(), radioStationId)) radioStationId = DefaultRadioStation.Id;
        }

        private void VerifyMonthAndYear(ref int month, ref int year)
        {
            if (!argumentsValidator.IsMonthValid(month)) month = DefaultMonth;
            if (!argumentsValidator.IsYearValid(year)) year = DefaultYear;
        }
    }
}