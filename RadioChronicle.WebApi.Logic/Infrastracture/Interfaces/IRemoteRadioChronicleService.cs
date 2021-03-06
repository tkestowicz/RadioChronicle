using System;
using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRemoteRadioChronicleService
    {
        RadioStation DefaultRadioStation { get; }
        int DefaultMonth { get; }
        int DefaultYear { get; }

        IEnumerable<RadioStationGroup> GetRadioStations();
        IEnumerable<Track> GetMostPopularTracks(int radioStationId, int month, int year);
        IEnumerable<Track> GetNewestTracks(int radioStationId);
        IDictionary<RadioStation, Track> GetCurrentlyBroadcastedTracks();
        IEnumerable<Track> GetBroadcastHistory(int radioStation, DateTime day, int hourFrom, int hourTo);
        IEnumerable<Track> GetMostRecentTracks(int radioStationId);
        IEnumerable<TrackHistory> GetTrackHistory(string relativeUrlToTrackDetails);
    }
}