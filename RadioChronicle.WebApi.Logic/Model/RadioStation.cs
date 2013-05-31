namespace RadioChronicle.WebApi.Logic.Model
{
    public class RadioStation
    {
        public int Id { get; set; }

        public string Name { get; set; }

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
            if ((obj is RadioStation) == false) return false;

            var toEqual = obj as RadioStation;

            return (toEqual.Id == Id && toEqual.Name == Name);
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
            return string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode() + Id.GetHashCode();
        }

        #endregion

        #endregion
    }
}