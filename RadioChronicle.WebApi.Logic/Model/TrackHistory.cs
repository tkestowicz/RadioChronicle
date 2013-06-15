using System;
using RadioChronicle.WebApi.Logic.Infrastracture.Interfaces;

namespace RadioChronicle.WebApi.Logic.Model
{
    public class TrackHistory : IModel
    {
        public DateTime? Broadcasted { get; set; }

        public RadioStation RadioStation { get; set; }

        public static TrackHistory Empty
        {
            get
            {
                return new TrackHistory()
                {
                    Broadcasted = null,
                    RadioStation = RadioStation.Empty
                };
            }
        }

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
            var toEqual = obj as TrackHistory;

            if (toEqual == null) return false;

            return Equals(toEqual);
        }

        #region Equality members

        protected bool Equals(TrackHistory other)
        {
            return Broadcasted.Equals(other.Broadcasted) && Equals(RadioStation, other.RadioStation);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Broadcasted.GetHashCode() * 397) ^ (RadioStation != null ? RadioStation.GetHashCode() : 0);
            }
        }

        #endregion

        #endregion

        public bool IsNotEmpty()
        {
            return Equals(Empty) == false;
        }
    }
}