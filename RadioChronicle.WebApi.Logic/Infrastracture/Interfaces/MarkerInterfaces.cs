using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IMostPopularTrackParser : IRowParser<Track>
    {
    }

    public interface INewestTrackParser : IRowParser<Track>
    {
    }

    public interface ICurrentlyBroadcastedTrack : IRowParser<KeyValuePair<RadioStation, Track>>
    {
    }

    public interface ITrackBroadcastHistoryParser : IRowParser<Track>
    {
    }

    public interface ITrackHistoryParser : IRowParser<TrackHistory>
    {
    }

    public interface IRadioGroupParser : IRowParser<RadioStationGroup>
    {
    }

    public interface IRadioStationParser : IRowParser<RadioStation>
    {
    }

    public interface IRadioStationCollectionParser : ICollectionParser<RadioStationGroup>,
        ICollectionParser<RadioStation>
    {
    }

    public interface ITrackCollectionParser : ICollectionParser<Track>, 
        ICollectionParser<TrackHistory>, ICollectionParser<KeyValuePair<RadioStation, Track>>
    {
    }
}
