using System.Security.Policy;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuUrlRepository : IUrlRepository
    {
        private readonly Url _radioStationsPage = new Url("http://www.odsluchane.eu/szukaj.php");
        private const string _MostPopularTracksPagePattern = "http://www.odsluchane.eu/top.php?r={0}&m={1}&y={2}";

        #region Implementation of IUrlRepository

        public Url RadioStationsPage
        {
            get { return _radioStationsPage; }
        }

        public Url MostPopularTracksPage(int radioStationId, int month, int year)
        {
            return new Url(string.Format(_MostPopularTracksPagePattern, radioStationId, month, year));
        }

        #endregion
    }
}