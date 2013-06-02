using System;
using System.Collections.Generic;
using System.Linq;
using RadioChronicle.WebApi.Logic.Infrastracture;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;
using RadioChronicle.WebApi.Logic.Model;

namespace RadioChronicle.WebApi.Logic.OdsluchaneEu
{
    public class OdsluchaneEURemoteServiceArgumentsValidator : IRemoteServiceArgumentsValidator
    {
        const int YearSinceOdsluchaneEuWorks = 2009;

        #region Implementation of IRemoteServiceArgumentsValidator

        public bool IsRadioStationIdValid(IEnumerable<RadioStationGroup> radioStations, int radioStationId)
        {
            try
            {
                return radioStations
                        .Single(g => g.RadioStations.SingleOrDefault(r => r.Id == radioStationId) != null)
                        .RadioStations.Single(r => r.Id == radioStationId) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsMonthValid(int month)
        {
            return month >= 1 && month <= 12;
        }

        public bool IsYearValid(int year)
        {
            return year >= YearSinceOdsluchaneEuWorks && year <= ApplicationTime.Current.Year;
        }

        #endregion
    }
}