using System.Collections.Generic;
using System.Linq;

namespace RadioChronicle.WebApi.Logic.Model
{
    public class RadioStationGroup
    {
        public string GroupName { get; set; }

        public IEnumerable<RadioStation> RadioStations { get; set; }

        #region Overrides of Object

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if ((obj is RadioStationGroup) == false) return false;

            var toEqual = obj as RadioStationGroup;

            var colEqual = toEqual.RadioStations.Count() == RadioStations.Count();
            for (int i = 0; i < RadioStations.Count(); i++)
            {
                colEqual = RadioStations.ElementAt(i).Equals(toEqual.RadioStations.ElementAt(i));
            }

            return toEqual.GroupName == GroupName && colEqual;
        }

        #region Overrides of Object

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(GroupName) ? 0 : GroupName.GetHashCode() + RadioStations.GetHashCode();
        }

        #endregion

        #endregion
    }
}