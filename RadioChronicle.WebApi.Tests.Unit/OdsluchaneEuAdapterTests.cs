using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;
using RadioChronicle.WebApi.Tests.Unit.OdsluchaneEuHelpers;
using Should;

namespace RadioChronicle.WebApi.Tests.Unit
{
    [TestFixture(Category = "OdsluchaneEU")]
    public class OdsluchaneEuAdapterTests
    {
        private Mock<IRequestHelper> _requestHelperMock;

        private IRemoteRadioChronicleService _remoteRadioChronicleService;

        private IUrlRepository _urlRepository;

        #region Default values

        private static int DefaultYear
        {
            get
            {
                // represents current year
                return ApplicationTime.Current.Year;
            }
        }

        private static int DefaultMonth
        {
            get
            {
                // represents May
                return ApplicationTime.Current.Month;
            }
        }

        private static RadioStation DefaultRadioStation
        {
            get
            {
                return new RadioStation()
                {
                    Id = 2,
                    Name = "RMF FM"
                };
            }
        }

        private static DateTime DefaultDay
        {
            get { return new DateTime(2013, 6, 6); }
        }

        private const int DefaultHourFrom = 9;

        private const int DefaultHourTo = 11;
       
        private const string DefaultRelativeUrlToTrackDetails = "/utwor/157092/alice_russel_-_let_go_breakdown";

       #endregion

        [SetUp]
        public void ResolveDependencies()
        {
            var diContainer = Bootstrap.DiContainer();

            _requestHelperMock = diContainer.Resolve<Mock<IRequestHelper>>();
            _urlRepository = diContainer.Resolve<IUrlRepository>();

            _remoteRadioChronicleService = diContainer.Resolve<IRemoteRadioChronicleService>(
                                new TypedParameter(typeof(IRequestHelper), _requestHelperMock.Object)
                        );

            // Set current time to 1'st May of 2013 for test purposes
            ApplicationTime._replaceCurrentTimeLogic(() => new DateTime(2013, 5, 1));
        }

        [Test]
        [Category("Get radio stations")]
        [Description("Happy path")]
        public void get_radio_stations___response_contains_radio_stations___returns_list_of_radio_stations_grouped_by_radio_family()
        {
            _requestHelperMock.Setup(s => s.RequestUrl(_urlRepository.RadioStationsPage.Value))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithRadioStations));

            var result = _remoteRadioChronicleService.GetRadioStations();

            result.ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedRadioStationGroups);
        }

        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.Empty, Category ="Get radio stations", Description = "Response is empty.")]
        public void get_radio_stations___response_is_different_than_expected___returns_empty_list(OdsluchaneEuFakeResponsesService.ResponseKeys response)
        {
            _requestHelperMock.Setup(s => s.RequestUrl(_urlRepository.RadioStationsPage.Value))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.Empty));

            var result = _remoteRadioChronicleService.GetRadioStations();

            const int expectedElementsInCollection = 0;

            result.Count().ShouldEqual(expectedElementsInCollection);
        }

        [Test]
        [Category("Get radio stations")]
        public void get_radio_stations___response_has_one_radio_group_with_no_radio_stations___returns_radio_station_group_with_empty_radio_stations()
        {
            _requestHelperMock.Setup(s => s.RequestUrl(_urlRepository.RadioStationsPage.Value))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithOneRadioGroupAndNoRadioStations));

            var result = _remoteRadioChronicleService.GetRadioStations();

            var expectedCollection = new List<RadioStationGroup>()
            {
                new RadioStationGroup()
                {
                    GroupName = "Eurozet",
                    RadioStations = new List<RadioStation>()
                }
            };

            result.ShouldEqual(expectedCollection);
        }

        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.Empty, Category = "Get most popular tracks", Description = "Response is empty.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.WithMostPopularTracksWhereTrackRowHas3Columns, Category = "Get most popular tracks", Description = "Response has changed and track row has less columns.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.WithMostPopularTracksWhereTrackRowHas5Columns, Category = "Get most popular tracks", Description = "Response has changed and track row has more columns.")]
        public void get_most_popular_tracks___response_is_different_than_expected___return_empty_list(OdsluchaneEuFakeResponsesService.ResponseKeys responseKey)
        {
            var radioStation = DefaultRadioStation;
            var month = DefaultMonth;
            var year = DefaultYear;
            _requestHelperMock.Setup(s => s.RequestUrl(string.Format(_urlRepository.MostPopularTracksPage(radioStation.Id, month, year).Value)))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(responseKey));

            var result = _remoteRadioChronicleService.GetMostPopularTracks(radioStation.Id, month, year);

            const int expectedNumberOfItems = 0;

            result.Count().ShouldEqual(expectedNumberOfItems);
        }

        [TestCase(null, null, null, Category = "Get most popular tracks", Description = "Happy path.")]
        [TestCase(0, null, null, Category = "Get most popular tracks", Description = "Radio station id is not set.")]
        [TestCase(-1, null, null, Category = "Get most popular tracks", Description = "Radio station id negative.")]
        [TestCase(1000, null, null, Category = "Get most popular tracks", Description = "Radio station id does not exist.")]
        [TestCase(null, -1, null, Category = "Get most popular tracks", Description = "Month is negative.")]
        [TestCase(null, 0, null, Category = "Get most popular tracks", Description = "Month is out of range (left boundary).")]
        [TestCase(null, 13, null, Category = "Get most popular tracks", Description = "Month is out of range (right boundary).")]
        [TestCase(null, null, -1, Category = "Get most popular tracks", Description = "Year is negative.")]
        [TestCase(null, null, 2008, Category = "Get most popular tracks", Description = "Year is out of range (left boundary).")]
        [TestCase(null, null, 2020, Category = "Get most popular tracks", Description = "Year is out of range (right boundary).")]
        [TestCase(0, 0, 0, Category = "Get most popular tracks", Description = "All parameters are not set correctly.")]
        [TestCase(null, 0, 0, Category = "Get most popular tracks", Description = "Month and year are not set correctly.")]
        [TestCase(0, null, 0, Category = "Get most popular tracks", Description = "Radio station and year are not set correctly.")]
        public void get_most_popular_tracks___response_contains_tracks__returns_top_10_tracks_ordered_by_played_times_descending(int? radioStationId, int? month, int? year)
        {
            if(radioStationId.HasValue == false) radioStationId = DefaultRadioStation.Id;
            if(month.HasValue == false) month = DefaultMonth;
            if(year.HasValue == false) year = DefaultYear;

            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.RadioStationsPage.Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithRadioStations));
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.MostPopularTracksPage(DefaultRadioStation.Id, DefaultMonth, DefaultYear).Value))
                                    .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithMostPopularTracks));

            var result = _remoteRadioChronicleService.GetMostPopularTracks(radioStationId.Value, month.Value, year.Value);

            result.ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedMostPopularTracksOnRMFFMRadioStationInMay2013);
        }

        [TestCase(null, Category = "Get newest tracks", Description = "Happy path.")]
        [TestCase(0, Category = "Get newest tracks", Description = "Radio station id is not set.")]
        [TestCase(-1, Category = "Get newest tracks", Description = "Radio station id negative.")]
        [TestCase(1000, Category = "Get newest tracks", Description = "Radio station id does not exist.")]
        public void get_newest_tracks___response_contains_tracks___returns_10_newest_tracks_grouped_by_date_descending(int? radioStationId)
        {
            if (radioStationId.HasValue == false) radioStationId = DefaultRadioStation.Id;

            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.RadioStationsPage.Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithRadioStations));
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.NewestTracksPage(DefaultRadioStation.Id).Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithNewestTracks));

            var result = _remoteRadioChronicleService.GetNewestTracks(radioStationId.Value);

            var grouped =
                result.GroupBy(t => t.PlayedFirstTime.Value.ToShortDateString())
                    .ToDictionary(k => DateTime.Parse(k.Key), v => v.ToList() as IEnumerable<Track>) as IDictionary<DateTime, IEnumerable<Track>>;

            grouped.Keys.ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedNewestTracksOnRMFFm.Keys);
            grouped.Values.ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedNewestTracksOnRMFFm.Values);
        }

        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.Empty, Category = "Get newest tracks", Description = "Response is empty.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.WithNewestTracksWhereTrackRowHas2Columns, Category = "Get newest tracks", Description = "Response has changed and track row has less columns.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.WithNewestTracksWhereTrackRowHas5Columns, Category = "Get newest tracks", Description = "Response has changed and track row has more columns.")]
        public void get_newest_tracks___response_is_different_than_expected___returns_empty_list(OdsluchaneEuFakeResponsesService.ResponseKeys response)
        {

            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.RadioStationsPage.Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithRadioStations));
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.NewestTracksPage(DefaultRadioStation.Id).Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(response));

            var result = _remoteRadioChronicleService.GetNewestTracks(DefaultRadioStation.Id);

            const int expectedNumberOfItems = 0;

            result.Count().ShouldEqual(expectedNumberOfItems);
        }

        [Test]
        [Category("Currently broadcasted")]
        [Description("Happy path")]
        public void currently_broadcasted___response_contains_tracks___returns_currently_broadcasted_tracks_ordered_by_radio_station_ascending()
        {
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.CurrentlyBroadcastedTrack.Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithCurrentlyBroadcastedTracks));

            var result = _remoteRadioChronicleService.GetCurrentlyBroadcastedTracks();

            result.ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedCurrentlyBroadcastedTracksOrderedByRadioStationAscending);
        }

        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.Empty, Category = "Currently broadcasted", Description = "Response is empty.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.NoneRadioStationIsBroadcasting, Category = "Currently broadcasted", Description = "None radio station is broadcasting.")]
        public void currently_broadcasted___response_is_different_than_expected___returns_empty_list(OdsluchaneEuFakeResponsesService.ResponseKeys response)
        {
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.CurrentlyBroadcastedTrack.Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(response));

            var result = _remoteRadioChronicleService.GetCurrentlyBroadcastedTracks();

            const int expectedNumberOfRecords = 0;

            result.Count.ShouldEqual(expectedNumberOfRecords);
        }

        [TestCase(null, null, null, Category = "Broadcast history", Description = "Happy path.")]
        [TestCase(1000, null, null, Category = "Broadcast history", Description = "Radio station id does not exist.")]
        [TestCase(-1, null, null, Category = "Broadcast history", Description = "Radio station id is negative.")]
        [TestCase(-1, -1, null, Category = "Broadcast history", Description = "Radio station id and start hour are negative.")]
        [TestCase(-1, -1, -1, Category = "Broadcast history", Description = "Radio station id, start and end hour are negative.")]
        [TestCase(null, -1, -1, Category = "Broadcast history", Description = "Radio station id is set, start hour is grater than end hour.")]
        [TestCase(null, 24, null, Category = "Broadcast history", Description = "Radio station id and end time are set, start hour is out of range.")]
        [TestCase(null, null, 50, Category = "Broadcast history", Description = "Radio station id and start time are set, end hour is out of range.")]
        public void broadcast_history___response_contains_tracks__returns_all_broadcasted_tracks_ordered_by_broadcast_date_descending(int? radioStationId, int? hourFrom, int? hourTo)
        {
            ApplicationTime._replaceCurrentTimeLogic(() => new DateTime(DefaultDay.Year, DefaultDay.Month, DefaultDay.Day, DefaultHourTo, 0, 0));

            if (radioStationId.HasValue == false) radioStationId = DefaultRadioStation.Id;
            if (hourFrom.HasValue == false) hourFrom = DefaultHourFrom;
            if (hourTo.HasValue == false) hourTo = DefaultHourTo;

            _requestHelperMock.Setup(s => s.RequestUrl(_urlRepository.RadioStationsPage.Value))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithRadioStations));

            _requestHelperMock.Setup(s => s.RequestUrl(_urlRepository.BroadcastHistoryPage(DefaultRadioStation.Id, DefaultDay, DefaultHourFrom, DefaultHourTo).Value))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithBroadcastHistory));

            var result = _remoteRadioChronicleService.GetBroadcastHistory(radioStationId.Value, DefaultDay, hourFrom.Value, hourTo.Value);

            result.ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedBrodcastHistoryIn_6_6_2013_from_9_to_11);
        }

        [TestCase(null, Category = "Get most recent tracks", Description = "Happy path")]
        [TestCase(0, Category = "Get most recent tracks", Description = "Radio station does not exist.")]
        public void get_most_recent_tracks___response_contains_tracks___returns_10_recent_tracks_ordered_by_date_descending(int? radioStationId)
        {
            ApplicationTime._replaceCurrentTimeLogic(() => new DateTime(DefaultDay.Year, DefaultDay.Month, DefaultDay.Day, DefaultHourTo, 0, 0));

            if (radioStationId.HasValue == false) radioStationId = DefaultRadioStation.Id;

            _requestHelperMock.Setup(s => s.RequestUrl(_urlRepository.RadioStationsPage.Value))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithRadioStations));

            _requestHelperMock.Setup(s => s.RequestUrl(_urlRepository.BroadcastHistoryPage(DefaultRadioStation.Id, DefaultDay, DefaultHourFrom, DefaultHourTo).Value))
                .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithBroadcastHistory));

            var result = _remoteRadioChronicleService.GetMostRecentTracks(radioStationId.Value);

            result.ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedBrodcastHistoryIn_6_6_2013_from_9_to_11.Take(10).ToList());
        }

        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.Empty, Category = "Get most recent tracks", Description = "Response is empty.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.WithMostRecentTracksWhereTrackRowHas2Columns, Category = "Get most recent tracks", Description = "Response has changed and track row has less columns.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.WithMostRecentTracksWhereTrackRowHas5Columns, Category = "Get most recent tracks", Description = "Response has changed and track row has more columns.")]
        public void get_most_recent_tracks___response_is_different_than_expected___returns_empty_list(OdsluchaneEuFakeResponsesService.ResponseKeys response)
        {

            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.RadioStationsPage.Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithRadioStations));
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.NewestTracksPage(DefaultRadioStation.Id).Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(response));

            var result = _remoteRadioChronicleService.GetMostRecentTracks(DefaultRadioStation.Id);

            const int expectedNumberOfItems = 0;

            result.Count().ShouldEqual(expectedNumberOfItems);
        }

        [Test]
        [Category("Get track history")]
        [Description("Happy path.")]
        public void get_track_history___response_contains_history___returns_all_broadcasts_of_the_track_ordered_by_date_descending()
        {
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.TrackDetailsPage(DefaultRelativeUrlToTrackDetails).Value)).Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(OdsluchaneEuFakeResponsesService.ResponseKeys.WithTrackHistory));

            var result = _remoteRadioChronicleService.GetTrackHistory(DefaultRelativeUrlToTrackDetails);

            var takeOnlyAFewFirstRecords = OdsluhaneEuTestDataRepository.ExpectedTrackHistory.Count();

            result.Take(takeOnlyAFewFirstRecords).ToList().ShouldEqual(OdsluhaneEuTestDataRepository.ExpectedTrackHistory);
        }

        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.TrackDetailsPageDoesNotExists, null, Category = "Get track history", Description = "Relative url is null.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.TrackDetailsPageDoesNotExists, "this_is_not_existing_relative_url", Category = "Get track history", Description = "Relative url is wrong.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.Empty, DefaultRelativeUrlToTrackDetails, Category = "Get track history", Description = "Relative url is correct but response is empty.")]
        [TestCase(OdsluchaneEuFakeResponsesService.ResponseKeys.WithTrackHistoryWhereHistoryRowHas5Columns, DefaultRelativeUrlToTrackDetails, Category = "Get track history", Description = "Relative url is correct but response has more columns than expected.")]
        public void get_track_history___response_is_different_than_expected___returns_empty_list(OdsluchaneEuFakeResponsesService.ResponseKeys response, string relativeUrlToTrackDetails)
        {
            _requestHelperMock.Setup(r => r.RequestUrl(_urlRepository.TrackDetailsPage(relativeUrlToTrackDetails).Value))
                                            .Returns(OdsluchaneEuFakeResponsesService.GetFakeResponse(response));

            var result = _remoteRadioChronicleService.GetTrackHistory(relativeUrlToTrackDetails);

            const int expectedNumberOfElements = 0;

            result.Count().ShouldEqual(expectedNumberOfElements);
        }
    }
}
