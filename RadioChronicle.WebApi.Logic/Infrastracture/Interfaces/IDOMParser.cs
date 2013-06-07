using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IDOMParser
    {
        IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document);
        IEnumerable<Track> ParseDOMAndSelectMostPopularTracks(HtmlDocument document);
        IEnumerable<Track> ParseDOMAndSelectNewestTracks(HtmlDocument document);
        IDictionary<RadioStation, Track> ParseDOMAndSelectCurrentlyBroadcastedTracks(HtmlDocument document);
        IEnumerable<Track> ParseDOMAndSelectBroadcastHistory(HtmlDocument document);
        IEnumerable<TrackHistory> ParseDOMAndSelectTrackHistory(HtmlDocument document);
    }
}