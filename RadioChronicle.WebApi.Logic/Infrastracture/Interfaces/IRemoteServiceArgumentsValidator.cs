using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.POCO;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRemoteServiceArgumentsValidator
    {
        bool IsRadioStationIdValid(IEnumerable<RadioStationGroup> radioStations, int radioStationId);
        bool IsMonthValid(int month);
        bool IsYearValid(int year);
        bool IsHourValid(int hour);
        bool IsRangeValid(int from, int to);
    }
}