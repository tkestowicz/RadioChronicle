using System;
using System.Globalization;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{

    public class OdsluchaneEuDateParserArgs
    {
        public OdsluchaneEuDateParser.DateTimePattern DateFormat { get; set; }

        public string StringToParse { get; set; }
    }

    public class OdsluchaneEuDateParser : ISpecifiedDOMParser<DateTime, OdsluchaneEuDateParserArgs>
    {
        public const string BroadcastedShortDatePattern = "dd-MM-yyyy";
        public const string BroadcastedDateTimePattern = "dd-MM-yyyy HH:mm";
        private const string _DefaultDateTimePattern = BroadcastedDateTimePattern;

        public enum DateTimePattern
        {
            BroadcastedShortDate,
            BroadcastedDateTime
        }

        public string GetStringDateTimeFormat(DateTimePattern replaceDateTimePattern)
        {
            switch (replaceDateTimePattern)
            {
                case DateTimePattern.BroadcastedShortDate:
                    return BroadcastedShortDatePattern;

                case DateTimePattern.BroadcastedDateTime:
                    return BroadcastedDateTimePattern;

                default:
                    return _DefaultDateTimePattern;
            }
                
        }

        #region Implementation of ISpecifiedDOMParser<out DateTime,in OdsluchaneEuDateParserArgs>

        public DateTime Parse(OdsluchaneEuDateParserArgs input)
        {
            DateTime outputDateTime;

            DateTime.TryParseExact(input.StringToParse, GetStringDateTimeFormat(input.DateFormat), null, DateTimeStyles.None, out outputDateTime);

            return outputDateTime;
        }

        #endregion
    }
}