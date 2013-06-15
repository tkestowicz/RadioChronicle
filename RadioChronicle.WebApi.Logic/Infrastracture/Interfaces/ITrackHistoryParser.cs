using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface ITrackHistoryParser : ISpecifiedDOMParser<TrackHistory, IEnumerable<HtmlNode>>
    {
        DateTime? DateWhenTrackWasBroadcasted { get; set; }
    }
}