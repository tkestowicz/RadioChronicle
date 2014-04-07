using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RadioChronicle.WebApi.Tests.Unit.OdsluchaneEuHelpers
{
    public class OdsluchaneEuFakeResponsesService
    {
        public enum ResponseKeys
        {
            Empty,
            WithRadioStations,
            WithOneRadioGroupAndNoRadioStations,
            WithMostPopularTracks,
            WithMostPopularTracksWhereTrackRowHas3Columns,
            WithMostPopularTracksWhereTrackRowHas5Columns,
            WithMostRecentTracks,
            WithMostRecentTracksWhereTrackRowHas2Columns,
            WithMostRecentTracksWhereTrackRowHas5Columns,
            WithCurrentlyBroadcastedTracks,
            NoneRadioStationIsBroadcasting,
            WithNewestTracks,
            WithNewestTracksWhereTrackRowHas2Columns,
            WithNewestTracksWhereTrackRowHas5Columns,
            WithBroadcastHistory,
            WithTrackHistory,
            TrackDetailsPageDoesNotExists,
            WithTrackHistoryWhereHistoryRowHas5Columns
        }

        private const string RelativePathToFilesWithResponses = "OdsluchaneEuHelpers/FakeResponses/";

        public static HtmlDocument GetFakeResponse(ResponseKeys responseKey)
        {
            var document = new HtmlDocument();
            switch (responseKey)
            {
                case ResponseKeys.WithRadioStations:
                    document.LoadHtml(File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithRadioStationList.txt"));
                    break;

                case ResponseKeys.WithOneRadioGroupAndNoRadioStations:
                    document.LoadHtml(File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithOneGroupAndNoRadioStations.txt"));
                    break;

                case ResponseKeys.WithMostPopularTracks:
                    document.LoadHtml(File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithMostPopularTracksOnRMFFMInMay2013.txt"));
                    break;

                case ResponseKeys.WithMostPopularTracksWhereTrackRowHas3Columns:
                    document.LoadHtml(
                        File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithMostPopularTracksWhereTrackRowHas3Columns.txt"));
                    break;

                case ResponseKeys.WithNewestTracksWhereTrackRowHas5Columns:
                case ResponseKeys.WithMostRecentTracksWhereTrackRowHas5Columns:
                case ResponseKeys.WithMostPopularTracksWhereTrackRowHas5Columns:
                case ResponseKeys.WithTrackHistoryWhereHistoryRowHas5Columns:
                    document.LoadHtml(
                        File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithMostPopularTracksWhereTrackRowHas5Columns.txt"));
                    break;

                case ResponseKeys.WithMostRecentTracks:
                    document.LoadHtml(File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithMostRecentTracksOnRMFFMInMay2013.txt"));
                    break;

                case ResponseKeys.WithNewestTracks:
                    document.LoadHtml(File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithNewestTracksOnRMFFMInMay2013.txt"));
                    break;


                case ResponseKeys.WithNewestTracksWhereTrackRowHas2Columns:
                case ResponseKeys.WithMostRecentTracksWhereTrackRowHas2Columns:
                    document.LoadHtml(
                        File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithMostRecentTracksWhereTrackRowHas2Columns.txt"));
                    break;

                case ResponseKeys.WithCurrentlyBroadcastedTracks:
                    document.LoadHtml(
                        File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithCurrentlyBroadcastedTracksIn_2013.06.05_20.34.txt"));
                    break;

                case ResponseKeys.NoneRadioStationIsBroadcasting:
                    document.LoadHtml(File.ReadAllText(RelativePathToFilesWithResponses + "ResponseNoneRadioStationIsBroadcasting.txt"));
                    break;

                case ResponseKeys.WithBroadcastHistory:
                    document.LoadHtml(
                        File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithBroadcastHistoryInRMFFM_06.06.2013_9_11.txt"));
                    break;

                case ResponseKeys.WithTrackHistory:
                    document.LoadHtml(
                        File.ReadAllText(RelativePathToFilesWithResponses + "ResponseWithTrackHistory.txt"));
                    break;

                case ResponseKeys.TrackDetailsPageDoesNotExists:
                    document.LoadHtml(
                        File.ReadAllText(RelativePathToFilesWithResponses + "ResponseTrackDetailsPageDoesNotExists.txt"));
                    break;


                case ResponseKeys.Empty:
                default:
                    document.LoadHtml("");
                    break;
            }

            return document;
        }
    }
}
