using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRemoteRadioChronicleService
    {
        IEnumerable<RadioStationGroup> GetRadioStations();
        IEnumerable<Track> GetMostPopularTracks(RadioStation radioStation, int month, int year);
    }
}