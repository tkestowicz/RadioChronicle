using System;
using System.Collections.Generic;
using System.Xml.XPath;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IResponseParser
    {
        IEnumerable<RadioStationGroup> ParseDOMAndSelectRadioStationGroups(HtmlDocument document);
        IEnumerable<Track> ParseDOMAndSelectMostPopularTracks(HtmlDocument document);
        IEnumerable<Track> ParseDOMAndSelectNewestTracks(HtmlDocument document);
        IDictionary<RadioStation, Track> ParseDOMAndSelectCurrentlyBroadcastedTracks(HtmlDocument document);
        IEnumerable<Track> ParseDOMAndSelectBroadcastHistory(HtmlDocument document);
        IEnumerable<TrackHistory> ParseDOMAndSelectTrackHistory(HtmlDocument document);
    }
}