using System.Collections.Generic;
using System.Linq;

namespace RadioChronicle.WebApi.Logic.Model
{
    public class Track
    {
        public string Name { get; set; }

        public string RelativeUrlToTrackDetails { get; set; }

        public int TimesPlayed { get; set; }

        public IEnumerable<TrackHistory> TrackHistory { get; set; }

        public static Track Empty
        {
            get
            {
                return new Track()
                {
                    Name = "",
                    RelativeUrlToTrackDetails = "",
                    TimesPlayed = 0,
                    TrackHistory = new List<TrackHistory>()

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
            var toEqual = obj as Track;

            if (toEqual == null) return false;

            return Equals(toEqual);
        }

        #region Equality members

        protected bool Equals(Track otherTrack)
        {
            return Name == otherTrack.Name && RelativeUrlToTrackDetails == otherTrack.RelativeUrlToTrackDetails && TimesPlayed == otherTrack.TimesPlayed && Equals(otherTrack.TrackHistory);
        }

        protected bool Equals(IEnumerable<TrackHistory> otherHistory)
        {
            var colEqual = otherHistory.Count() == TrackHistory.Count();
            for (int i = 0; i < TrackHistory.Count(); i++)
            {
                colEqual = TrackHistory.ElementAt(i).Equals(otherHistory.ElementAt(i));
            }

            return colEqual;
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
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RelativeUrlToTrackDetails != null ? RelativeUrlToTrackDetails.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TimesPlayed;
                hashCode = (hashCode * 397) ^ (TrackHistory != null ? TrackHistory.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion

        #endregion
    }
}