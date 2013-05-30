using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;

namespace RadioChronicle.WebApi.Tests.Unit
{
    [TestFixture]
    public class OdsluchaneEuAdapterTests
    {

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
            var urlRepository = new OdsluchaneEuUrlRepository();

            var concreteHtmlRequestHelper = new ConcreteHtmlRequestHelper();
            var requestHelper = new Mock<IRequestHelper>();

            requestHelper.Setup(s => s.RequestURL(urlRepository.RadioStationsPage.Value))
                .Returns(File.ReadAllText("FakeResponses/RadioStationListsResponse.txt"));

            var adapter = new OdsluchaneEuAdapter(requestHelper.Object, urlRepository);
            //var adapter = new OdsluchaneEuAdapter(concreteHtmlRequestHelper, urlRepository);

            var remoteService = new RadioChronicleRemoteService(adapter);

            var result = remoteService.GetRadioStations();
            var exptected = _prepareExpectedRadioStationGroups();

            CollectionAssert.AreEqual(exptected, result);
        }
    }

    public class ConcreteHtmlRequestHelper : IRequestHelper
    {
        #region Implementation of IRequestHelper

        public string RequestURL(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url).DocumentNode.InnerHtml;
        }

        #endregion
    }

    public class OdsluchaneEuUrlRepository : IUrlRepository
    {
        private readonly Url _radioStationsPage = new Url("http://www.odsluchane.eu/szukaj.php");

        #region Implementation of IUrlRepository

        public Url RadioStationsPage
        {
            get { return _radioStationsPage; }
        }

        #endregion
    }

    public interface IUrlRepository
    {
        Url RadioStationsPage { get; }

    }

    public class RadioChronicleRemoteService : IRemoteServiceStrategy
    {
        private readonly IRemoteServiceStrategy _serviceStrategy;

        public RadioChronicleRemoteService(IRemoteServiceStrategy serviceStrategy)
        {
            _serviceStrategy = serviceStrategy;
        }

        #region Implementation of IRemoteServiceStrategy

        public IEnumerable<RadioStationGroup> GetRadioStations()
        {
            return _serviceStrategy.GetRadioStations();
        }

        #endregion
    }

    public interface IRemoteServiceStrategy
    {
        IEnumerable<RadioStationGroup> GetRadioStations();
    }

    public class OdsluchaneEuAdapter : IRemoteServiceStrategy
    {
        private readonly IRequestHelper _requestHelper;
        private readonly OdsluchaneEuUrlRepository _urlRepository;

        public OdsluchaneEuAdapter(IRequestHelper requestHelper, OdsluchaneEuUrlRepository urlRepository)
        {
            _requestHelper = requestHelper;
            _urlRepository = urlRepository;
        }

        public IEnumerable<RadioStationGroup> GetRadioStations()
        {
            
            var doc = new HtmlDocument();

            doc.LoadHtml(_requestHelper.RequestURL(_urlRepository.RadioStationsPage.Value));

            return _ParseRadioStationSelectList(_RetrieveRadioStationGroupListFromHTMLDocument(doc));
        }

        private static IEnumerable<HtmlNode> _RetrieveRadioStationGroupListFromHTMLDocument(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//select[@name='r']/optgroup");
        }

        private static IEnumerable<HtmlNode> _RetrieveRadioStationListFromGroup(HtmlNode radioStationGroup)
        {
            return radioStationGroup.SelectNodes("option");
        }

        private IEnumerable<RadioStationGroup> _ParseRadioStationSelectList(IEnumerable<HtmlNode> radioStationSelectList)
        {
            var result = new List<RadioStationGroup>();

            foreach (var radioStationGroup in radioStationSelectList)
            {
                var radioStations = _ParseRadioStations(_RetrieveRadioStationListFromGroup(radioStationGroup));
                result.Add(new RadioStationGroup()
                {
                    GroupName = radioStationGroup.Attributes[0].Value,
                    RadioStations = radioStations
                });        
            }

            return result;
        }

        private IEnumerable<RadioStation> _ParseRadioStations(IEnumerable<HtmlNode> radioStations)
        {
            var result = new List<RadioStation>();

            foreach (var radioStation in radioStations)
            {
                var radioName = radioStation.Attributes.SingleOrDefault(a => a.Name == "label");
                var radioId = radioStation.Attributes.SingleOrDefault(a => a.Name == "value");

                result.Add(new RadioStation()
                {
                    Id = (radioId != null)? int.Parse(radioId.Value) : 0,
                    Name = (radioName != null)? radioName.Value : ""
                });
            }

            return result;
        }
    }

    public class RadioStationGroup
    {
        public string GroupName { get; set; }

        public IEnumerable<RadioStation> RadioStations { get; set; }

        #region Overrides of Object

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if ((obj is RadioStationGroup) == false) return false;

            var toEqual = obj as RadioStationGroup;

            var colEqual = false;
            for (int i = 0; i < RadioStations.Count(); i++)
            {
                colEqual = RadioStations.ElementAt(i).Equals(toEqual.RadioStations.ElementAt(i));
            }

            return toEqual.GroupName == GroupName && colEqual;
        }

        #region Overrides of Object

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(GroupName) ? 0 : GroupName.GetHashCode() + RadioStations.GetHashCode();
        }

        #endregion

        #endregion
    }

    public class RadioStation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        #region Overrides of Object

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if ((obj is RadioStation) == false) return false;

            var toEqual = obj as RadioStation;

            return (toEqual.Id == Id && toEqual.Name == Name);
        }

        #region Overrides of Object

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode() + Id.GetHashCode();
        }

        #endregion

        #endregion
    }

    public interface IRequestHelper
    {
        string RequestURL(string url);
    }
}
