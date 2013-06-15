using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface ITrackParser : ISpecifiedDOMParser<Track, IEnumerable<HtmlNode>>
    {
        DateTime? DateWhenTrackWasBroadcasted { get; set; } 
    }
}