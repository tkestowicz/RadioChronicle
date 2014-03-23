using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class CurrentlyBroadcastedTrackParser : IRowParser<KeyValuePair<RadioStation, Track>>
    {

        #region Implementation of IRowParser<out KeyValuePair<RadioStation,Track>>

        public HtmlNode GroupNode { set; private get; }

        public KeyValuePair<RadioStation, Track> Parse(HtmlNode node)
        {
            const int radioNameElementIndex = 0;
            const int trackInfoElementIndex = 1;

            try
            {
                var key = new RadioStation()
                {
                    Id = 0,
                    IsDefault = false,
                    Name = node.ChildNodes[radioNameElementIndex].InnerText.Trim()
                };

                var track = Track.Empty;
                
                var trackInfo = node.ChildNodes[trackInfoElementIndex];

                track.Name = HttpUtility.HtmlDecode(trackInfo.InnerText.Trim());
                track.RelativeUrlToTrackDetails = trackInfo.ChildNodes[trackInfoElementIndex].FirstChild.Attributes["href"].Value;

                return new KeyValuePair<RadioStation, Track>(key, track);
            }
            catch
            {
                return new KeyValuePair<RadioStation, Track>(new RadioStation(), Track.Empty);
            }
        }

        #endregion
    }
}