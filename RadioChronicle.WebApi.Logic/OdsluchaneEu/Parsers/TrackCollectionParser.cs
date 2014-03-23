using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class TrackCollectionParser : ITrackCollectionParser
    {
        private readonly IOdsluchaneEuResponseHelper responseHelper;

        public TrackCollectionParser(IOdsluchaneEuResponseHelper responseHelper)
        {
            this.responseHelper = responseHelper;
        }

        private IEnumerable<TRow> Parse<TRow>(IEnumerable<HtmlNode> rows, IRowParser<TRow> rowParser, Func<TRow, bool> isEmpty)
            where TRow: class 
        {
            foreach (var row in rows)
            {
                if (responseHelper.IsGroupHeader(row))
                {
                    rowParser.GroupNode = row;
                    continue;
                }

                var result = rowParser.Parse(row);

                if (isEmpty(result)) yield return result;
            }
        } 

        #region Implementation of ICollectionParser<Track>

        public IEnumerable<Track> Parse(IList<HtmlNode> rows, IRowParser<Track> rowParser)
        {
            return Parse(rows, rowParser, track => track.Equals(Track.Empty) == false);
        }

        #endregion

        #region Implementation of ICollectionParser<TrackHistory>

        public IEnumerable<TrackHistory> Parse(IList<HtmlNode> rows, IRowParser<TrackHistory> rowParser)
        {
            return Parse(rows, rowParser, trackHistory => new TrackHistory().Equals(trackHistory) == false);
        }

        #endregion

        #region Implementation of ICollectionParser<IEnumerable<KeyValuePair<RadioStation,Track>>>

        public IEnumerable<KeyValuePair<RadioStation, Track>> Parse(IList<HtmlNode> rows, IRowParser<KeyValuePair<RadioStation, Track>> rowParser)
        {
            return rows.Select(rowParser.Parse)
                .Where(parsedRow => parsedRow.Value.Equals(Track.Empty) == false);
        }

        #endregion
    }
}