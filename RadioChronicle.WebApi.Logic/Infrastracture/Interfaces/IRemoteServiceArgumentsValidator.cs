using System.Collections.Generic;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.Infrastracture.Interfaces
{
    public interface IRemoteServiceArgumentsValidator
    {
        bool IsRadioStationIdValid(IEnumerable<RadioStationGroup> radioStations, int radioStationId);
        bool IsMonthValid(int month);
        bool IsYearValid(int year);
    }
}