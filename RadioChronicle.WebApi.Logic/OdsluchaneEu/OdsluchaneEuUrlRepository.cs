using System.Security.Policy;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
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
}