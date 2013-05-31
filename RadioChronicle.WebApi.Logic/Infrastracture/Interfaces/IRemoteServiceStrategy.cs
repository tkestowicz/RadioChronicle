using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRemoteServiceStrategy
    {
        IEnumerable<RadioStationGroup> GetRadioStations();
    }
}