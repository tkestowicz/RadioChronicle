using System.Security.Policy;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEuUrlRepository : IUrlRepository
    {
        private readonly Url _radioStationsPage = new Url("http://www.odsluchane.eu/szukaj.php");
        private readonly Url _currentlyBroadcastedTrack = new Url("http://www.odsluchane.eu/");
        private const string _MostPopularTracksPagePattern = "http://www.odsluchane.eu/top.php?r={0}&m={1}&y={2}";
        private const string _MostRecentTracksPagePattern = "http://www.odsluchane.eu/nowosci.php?r={0}";

        #region Implementation of IUrlRepository

        public Url RadioStationsPage
        {
            get { return _radioStationsPage; }
        }

        public Url CurrentlyBroadcastedTrack
        {
            get { return _currentlyBroadcastedTrack; }
        }

        public Url MostPopularTracksPage(int radioStationId, int month, int year)
        {
            return new Url(string.Format(_MostPopularTracksPagePattern, radioStationId, month, year));
        }

        public Url MostRecentTracksPage(int radioStationId)
        {
            return new Url(string.Format(_MostRecentTracksPagePattern, radioStationId));
        }

        #endregion
    }
}