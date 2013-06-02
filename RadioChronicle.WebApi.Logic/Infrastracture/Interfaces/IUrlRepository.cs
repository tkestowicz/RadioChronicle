using System.Security.Policy;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IUrlRepository
    {
        Url RadioStationsPage { get; }
        Url MostPopularTracksPage(int radioStationId, int month, int year);
        Url MostRecentTracksPage(int radioStationId);
    }
}