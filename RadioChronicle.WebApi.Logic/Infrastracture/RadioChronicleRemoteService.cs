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

        public IEnumerable<Track> GetMostPopularTracks(RadioStation radioStation, int month, int year)
        {
            return _serviceStrategy.GetMostPopularTracks(radioStation, month, year);
        }

        #endregion
    }
}