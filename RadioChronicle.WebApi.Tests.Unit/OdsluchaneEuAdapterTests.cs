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
using RadioChronicle.WebApi.Logic.Model;
using Should;

namespace RadioChronicle.WebApi.Tests.Unit
{
    [TestFixture(Category = "OdsluchaneEU")]
    public class OdsluchaneEuAdapterTests
    {

        private Mock<IRequestHelper> _requestHelperMock;
        private IRemoteRadioChronicleService _remoteRadioChronicleService;
        private IUrlRepository _urlRepository;

        private static IEnumerable<Track> _ExpectedMostPopularTracksOnRMFFMRadioStationInMay2013
        {
            get
            {
                return new List<Track>()
                {
                    new Track()
                    {
                        Name = "One Direction - One Way Or Another",
                        RelativeUrlToTrackDetails = "/utwor/141032/one_direction_-_one_way_or_another",
                        TimesPlayed = 122,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Pink / Nate Ruess - Just Give Me A Reason",
                        RelativeUrlToTrackDetails = "/utwor/137317/pink_nate_ruess_-_just_give_me_a_reason",
                        TimesPlayed = 116,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Rihanna - Stay",
                        RelativeUrlToTrackDetails = "/utwor/123861/rihanna_-_stay",
                        TimesPlayed = 112,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Dido - No Freedom",
                        RelativeUrlToTrackDetails = "/utwor/134632/dido_-_no_freedom",
                        TimesPlayed = 110,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Lana Del Rey - Dark Paradise",
                        RelativeUrlToTrackDetails = "/utwor/115431/lana_del_rey_-_dark_paradise",
                        TimesPlayed = 105,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Justin Timberlake - Mirrors",
                        RelativeUrlToTrackDetails = "/utwor/141039/justin_timberlake_-_mirrors",
                        TimesPlayed = 101,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Lemon - Napraw",
                        RelativeUrlToTrackDetails = "/utwor/124264/lemon_-_napraw",
                        TimesPlayed = 96,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Passenger - Let Her Go",
                        RelativeUrlToTrackDetails = "/utwor/123001/passenger_-_let_her_go",
                        TimesPlayed = 95,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Bastille - Pompeii",
                        RelativeUrlToTrackDetails = "/utwor/136911/bastille_-_pompeii",
                        TimesPlayed = 94,
                        TrackHistory = new List<TrackHistory>()
                    },
                    new Track()
                    {
                        Name = "Honorata Skarbek Honey - Nie Powiem Jak",
                        RelativeUrlToTrackDetails = "/utwor/145876/honorata_skarbek_honey_-_nie_powiem_jak",
                        TimesPlayed = 93,
                        TrackHistory = new List<TrackHistory>()
                    }
                };
            }
        }

        private static IEnumerable<RadioStationGroup> _ExpectedRadioStationGroups
        {
            get
            {
                return new List<RadioStationGroup>()
                {
                    new RadioStationGroup()
                    {
                        GroupName = "Eurozet",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 1,
                                Name = "Radio ZET"
                            },
                            new RadioStation()
                            {
                                Id = 40,
                                Name = "Chilli ZET"
                            },
                            new RadioStation()
                            {
                                Id = 7,
                                Name = "Planeta FM (Warszawa)"
                            },
                            new RadioStation()
                            {
                                Id = 5,
                                Name = "Antyradio"
                            },
                            new RadioStation()
                            {
                                Id = 8,
                                Name = "Radio Plus"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "RMF",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 2,
                                Name = "RMF FM"
                            },
                            new RadioStation()
                            {
                                Id = 4,
                                Name = "RMF MAXXX"
                            },
                            new RadioStation()
                            {
                                Id = 6,
                                Name = "RMF Classic"
                            },
                            new RadioStation()
                            {
                                Id = 11,
                                Name = "RMF Dance"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "ESKA",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 3,
                                Name = "Radio Eska"
                            },
                            new RadioStation()
                            {
                                Id = 10,
                                Name = "Eska Rock"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Grupa Radiowa Agory",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 9,
                                Name = "Złote Przeboje"
                            },
                            new RadioStation()
                            {
                                Id = 13,
                                Name = "Roxy FM"
                            },
                            new RadioStation()
                            {
                                Id = 17,
                                Name = "Blue FM (Poznań)"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Polskie Radio",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 46,
                                Name = "Jedynka"
                            },
                            new RadioStation()
                            {
                                Id = 47,
                                Name = "Dwójka"
                            },
                            new RadioStation()
                            {
                                Id = 48,
                                Name = "Trójka"
                            },
                            new RadioStation()
                            {
                                Id = 49,
                                Name = "Czwórka"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Radio FaMa",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 19,
                                Name = "Radio FaMa (Kielce)"
                            },
                            new RadioStation()
                            {
                                Id = 18,
                                Name = "Radio FaMa (Tomaszów Maz.)"
                            }
                        }
                    },
                    new RadioStationGroup()
                    {
                        GroupName = "Pozostałe",

                        RadioStations = new List<RadioStation>()
                        {
                            new RadioStation()
                            {
                                Id = 23,
                                Name = "Muzyczne Radio"
                            },
                            new RadioStation()
                            {
                                Id = 16,
                                Name = "Radio Express"
                            },
                            new RadioStation()
                            {
                                Id = 14,
                                Name = "Radio PiN"
                            },
                            new RadioStation()
                            {
                                Id = 20,
                                Name = "Radio GRA"
                            },
                            new RadioStation()
                            {
                                Id = 15,
                                Name = "MC Radio"
                            },
                            new RadioStation()
                            {
                                Id = 12,
                                Name = "Polskie Radio Londyn"
                            },
                            new RadioStation()
                            {
                                Id = 22,
                                Name = "Radio eM"
                            },
                            new RadioStation()
                            {
                                Id = 21,
                                Name = "Radio RSC"
                            },
                            new RadioStation()
                            {
                                Id = 24,
                                Name = "Radio Elka"
                            },
                            new RadioStation()
                            {
                                Id = 25,
                                Name = "Radio Kolor"
                            },
                            new RadioStation()
                            {
                                Id = 26,
                                Name = "Radio Kaszebe"
                            },
                            new RadioStation()
                            {
                                Id = 27,
                                Name = "Radio Merkury"
                            },
                            new RadioStation()
                            {
                                Id = 28,
                                Name = "Radio Park"
                            },
                            new RadioStation()
                            {
                                Id = 29,
                                Name = "Radio Centrum (Kalisz)"
                            },
                            new RadioStation()
                            {
                                Id = 30,
                                Name = "VAX FM"
                            },
                            new RadioStation()
                            {
                                Id = 31,
                                Name = "Radio WAWA"
                            },
                            new RadioStation()
                            {
                                Id = 32,
                                Name = "Radio 90"
                            },
                            new RadioStation()
                            {
                                Id = 35,
                                Name = "Radio Vanessa"
                            },
                            new RadioStation()
                            {
                                Id = 41,
                                Name = "Radio Leliwa"
                            },
                            new RadioStation()
                            {
                                Id = 42,
                                Name = "Radio Freee"
                            },
                            new RadioStation()
                            {
                                Id = 43,
                                Name = "Radio Traffic"
                            },
                            new RadioStation()
                            {
                                Id = 44,
                                Name = "Radio Bielsko"
                            },
                            new RadioStation()
                            {
                                Id = 45,
                                Name = "Radio RAM"
                            },
                        }
                    }
                };
            }
        }

        private HtmlDocument _getFakeResponse(ResponseKeys responseKey)
        {
            var document = new HtmlDocument();
            switch (responseKey)
            {
                case ResponseKeys.WithRadioStations:
                    document.LoadHtml(File.ReadAllText("FakeResponses/ResponseWithRadioStationList.txt"));
                    break;

                case ResponseKeys.WithOneRadioGroupAndNoRadioStations:
                    document.LoadHtml(File.ReadAllText("FakeResponses/ResponseWithOneGroupAndNoRadioStations.txt"));
                    break;

                case ResponseKeys.WithMostPopularTracks:
                    document.LoadHtml(File.ReadAllText("FakeResponses/ResponseWithMostPopularTracksOnRMFFMInMay2013.txt"));
                    break;

                case ResponseKeys.WithMostPopularTracksWhereTrackRowHas3Columns:
                    document.LoadHtml(File.ReadAllText("FakeResponses/ResponseWithMostPopularTracksWhereTrackRowHas3Columns.txt"));
                    break;

                case ResponseKeys.WithMostPopularTracksWhereTrackRowHas5Columns:
                    document.LoadHtml(File.ReadAllText("FakeResponses/ResponseWithMostPopularTracksWhereTrackRowHas5Columns.txt"));
                    break;

                case ResponseKeys.Empty:
                default:
                    document.LoadHtml("");
                    break;
            }

            return document;
        }

        private static int _DefaultYear
        {
            get
            {
                // represents current year
                return ApplicationTime.Current.Year;
            }
        }

        private static int _DefaultMonth
        {
            get
            {
                // represents May
                return ApplicationTime.Current.Month;
            }
        }

        private static RadioStation _DefaultRadioStation
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

        public enum ResponseKeys
        {
            Empty,
            WithRadioStations,
            WithOneRadioGroupAndNoRadioStations,
            WithMostPopularTracks,
            WithMostPopularTracksWhereTrackRowHas3Columns,
            WithMostPopularTracksWhereTrackRowHas5Columns
        }

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
        public void get_radio_stations___response_contains_radio_stations___list_of_radio_stations_grouped_by_radio_family_is_returned()
        {
            _requestHelperMock.Setup(s => s.RequestURL(_urlRepository.RadioStationsPage.Value))
                .Returns(_getFakeResponse(ResponseKeys.WithRadioStations));

            var result = _remoteRadioChronicleService.GetRadioStations();

            result.ShouldEqual(_ExpectedRadioStationGroups);
        }

        [Test]
        [Category("Get radio stations")]
        public void get_radio_stations___response_is_empty___list_of_radio_stations_should_be_empty()
        {
            _requestHelperMock.Setup(s => s.RequestURL(_urlRepository.RadioStationsPage.Value))
                .Returns(_getFakeResponse(ResponseKeys.Empty));

            var result = _remoteRadioChronicleService.GetRadioStations();

            const int expectedElementsInCollection = 0;

            result.Count().ShouldEqual(expectedElementsInCollection);
        }

        [Test]
        [Category("Get radio stations")]
        public void get_radio_stations___response_has_one_radio_group_with_no_radio_stations___returns_radio_station_group_with_empty_radio_stations()
        {
            _requestHelperMock.Setup(s => s.RequestURL(_urlRepository.RadioStationsPage.Value))
                .Returns(_getFakeResponse(ResponseKeys.WithOneRadioGroupAndNoRadioStations));

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

        [Test]
        [Category("Get most popular tracks")]
        public void get_most_popular_tracks___default_criteria_set_and_response_contains_most_popular_tracks__returns_top_10_tracks_ordered_by_played_times_descending()
        {
            _requestHelperMock.Setup(r => r.RequestURL(_urlRepository.RadioStationsPage.Value)).Returns(_getFakeResponse(ResponseKeys.WithRadioStations));
            _requestHelperMock.Setup(s => s.RequestURL(_urlRepository.MostPopularTracksPage(_DefaultRadioStation.Id, _DefaultMonth, _DefaultYear).Value))
                .Returns(_getFakeResponse(ResponseKeys.WithMostPopularTracks));

            var result = _remoteRadioChronicleService.GetMostPopularTracks(_DefaultRadioStation.Id, _DefaultMonth, _DefaultYear);

            result.ShouldEqual(_ExpectedMostPopularTracksOnRMFFMRadioStationInMay2013);
        }

        [TestCase(ResponseKeys.Empty, Category = "Get most popular tracks", Description = "Response is empty")]
        [TestCase(ResponseKeys.WithMostPopularTracksWhereTrackRowHas3Columns, Category = "Get most popular tracks", Description = "Response has changed and track row has less columns")]
        [TestCase(ResponseKeys.WithMostPopularTracksWhereTrackRowHas5Columns, Category = "Get most popular tracks", Description = "Response has changed and track row has more columns")]
        public void get_most_popular_tracks___response_body_is_different_than_expected___return_empty_list(ResponseKeys responseKey)
        {
            var radioStation = _DefaultRadioStation;
            var month = _DefaultMonth;
            var year = _DefaultYear;
            _requestHelperMock.Setup(s => s.RequestURL(string.Format(_urlRepository.MostPopularTracksPage(radioStation.Id, month, year).Value)))
                .Returns(_getFakeResponse(responseKey));

            var result = _remoteRadioChronicleService.GetMostPopularTracks(radioStation.Id, month, year);

            const int expectedNumberOfItems = 0;

            result.Count().ShouldEqual(expectedNumberOfItems);
        }

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
        public void get_most_popular_tracks___criteria_are_not_set___default_value_is_set(int? radioStationId, int? month, int? year)
        {
            if(radioStationId.HasValue == false) radioStationId = _DefaultRadioStation.Id;
            if(month.HasValue == false) month = _DefaultMonth;
            if(year.HasValue == false) year = _DefaultYear;

            _requestHelperMock.Setup(r => r.RequestURL(_urlRepository.RadioStationsPage.Value)).Returns(_getFakeResponse(ResponseKeys.WithRadioStations));
            _requestHelperMock.Setup(r => r.RequestURL(_urlRepository.MostPopularTracksPage(_DefaultRadioStation.Id, _DefaultMonth, _DefaultYear).Value)).Verifiable();

            _remoteRadioChronicleService.GetMostPopularTracks(radioStationId.Value, month.Value, year.Value);

            _requestHelperMock.VerifyAll();
        }
    }
}
