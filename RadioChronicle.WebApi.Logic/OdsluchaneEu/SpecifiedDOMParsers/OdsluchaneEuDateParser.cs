using System;
using System.Globalization;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.SpecifiedDOMParsers
{
    public class OdsluchaneEuDateParser : ISpecifiedDOMParser<DateTime, string>
    {
        private const string _BroadcastedShortDatePattern = "dd-MM-yyyy";
        private const string _BroadcastedDateTimePattern = "dd-MM-yyyy HH:mm";
        private const string _DefaultDateTimePattern = _BroadcastedDateTimePattern;

        private readonly string _dateTimePattern;

        internal enum DateTimePattern
        {
            BroadcastedShortDate,
            BroadcastedDateTime
        }

        internal OdsluchaneEuDateParser()
        {
            _dateTimePattern = _DefaultDateTimePattern;
        }

        internal OdsluchaneEuDateParser(DateTimePattern replaceDateTimePattern)
        {
            switch (replaceDateTimePattern)
            {
                case DateTimePattern.BroadcastedShortDate:
                    _dateTimePattern = _BroadcastedShortDatePattern;
                    break;
                case DateTimePattern.BroadcastedDateTime:
                    _dateTimePattern = _BroadcastedDateTimePattern;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("replaceDateTimePattern");
            }
                
        }

        #region Implementation of ISpecifiedDOMParser<out DateTime,in string>

        public DateTime Parse(string input)
        {
            var outputDateTime = new DateTime();

            DateTime.TryParseExact(input, _dateTimePattern, null, DateTimeStyles.None, out outputDateTime);

            return outputDateTime;
        }

        #endregion
    }
}