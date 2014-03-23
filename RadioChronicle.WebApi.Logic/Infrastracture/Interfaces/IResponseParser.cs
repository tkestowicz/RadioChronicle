using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IResponseParser
    {
        IEnumerable<RadioStationGroup> ParseAndSelectRadioStationGroups(HtmlDocument document);

        IEnumerable<Track> ParseAndSelectMostPopularTracks(HtmlDocument document);
        
        IEnumerable<Track> ParseAndSelectNewestTracks(HtmlDocument document);
        
        IDictionary<RadioStation, Track> ParseAndSelectCurrentlyBroadcastedTracks(HtmlDocument document);
        
        IEnumerable<Track> ParseAndSelectBroadcastHistory(HtmlDocument document);
        
        IEnumerable<TrackHistory> ParseAndSelectTrackHistory(HtmlDocument document);
    }
}