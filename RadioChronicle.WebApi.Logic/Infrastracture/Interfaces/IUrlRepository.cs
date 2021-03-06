using System;
using System.Security.Policy;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IUrlRepository
    {
        Url RadioStationsPage { get; }
        Url CurrentlyBroadcastedTrack { get; }
        Url MostPopularTracksPage(int radioStationId, int month, int year);
        Url NewestTracksPage(int radioStationId);
        Url BroadcastHistoryPage(int radioStationId, DateTime day, int timeFrom, int timeTo);
        Url TrackDetailsPage(string relativeUrlToTrackDetails);
    }
}