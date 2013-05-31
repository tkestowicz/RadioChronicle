using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture
{
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
}