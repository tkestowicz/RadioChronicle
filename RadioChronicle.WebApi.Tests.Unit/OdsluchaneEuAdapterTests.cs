using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Core;
using Moq;
using NUnit.Framework;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;
using RadioChronicle.WebApi.Logic.OdsluchaneEu;

namespace RadioChronicle.WebApi.Tests.Unit
{
    [TestFixture]
    public class OdsluchaneEuAdapterTests
    {

        private Mock<IRequestHelper> _requestHelperMock;
        private IRemoteServiceStrategy _remoteService;
        private IUrlRepository _urlRepository;

        [SetUp]
        public void ResolveDependencies()
        {
            var diContainer = Bootstrap.DiContainer();

            _requestHelperMock = diContainer.Resolve<Mock<IRequestHelper>>();
            _urlRepository = diContainer.Resolve<IUrlRepository>();

            _remoteService = diContainer.ResolveKeyed<IRemoteServiceStrategy>(Bootstrap.RemoteServiceStrategy.StrategyContainer,
                        new ResolvedParameter(
                            (p, c) => p.ParameterType == typeof(IRemoteServiceStrategy),
                            (p, c) => c.ResolveKeyed<IRemoteServiceStrategy>(Bootstrap.RemoteServiceStrategy.OdsluchaneEuStrategy,
                                new TypedParameter(typeof(IRequestHelper), _requestHelperMock.Object)))
                        );
        }

        private IEnumerable<RadioStationGroup> _prepareExpectedRadioStationGroups()
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
            
        [Test]
        public void get_radio_stations___response_contains_radio_stations___list_of_radio_stations_grouped_by_radio_family_is_returned()
        {
            _requestHelperMock.Setup(s => s.RequestURL(_urlRepository.RadioStationsPage.Value))
                .Returns(File.ReadAllText("FakeResponses/ResponseWithRadioStationList.txt"));

            var result = _remoteService.GetRadioStations();
           
            var expected = _prepareExpectedRadioStationGroups();

            CollectionAssert.AreEqual(expected, result);
        }
    }

    
}
