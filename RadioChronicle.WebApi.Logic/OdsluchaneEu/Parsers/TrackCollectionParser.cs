using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Parsers
{
    public class TrackCollectionParser : ICollectionParser<Track>, ICollectionParser<TrackHistory>
    {
        //TODO: refactor
        private string SelectGroupHeader(HtmlNode row)
        {
            var header = row.SelectSingleNode("td[@class='line']");

            return (header == null) ? string.Empty : header.InnerText;
        }

        private bool CheckIfRowIsAGroupHeader(HtmlNode row)
        {
            return string.IsNullOrEmpty(SelectGroupHeader(row)) == false;
        }

        private IEnumerable<TTest> Parse<TTest>(IEnumerable<HtmlNode> rows, IRowParser<TTest> rowParser, Func<TTest, bool> isEmpty)
            where TTest: class 
        {
            foreach (var row in rows)
            {
                if (CheckIfRowIsAGroupHeader(row))
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
    }
}