namespace RadioChronicle.WebApi.Logic.OdsluchaneEu.Interfaces
{
    public interface IXPathSelectorsRepository
    {
        string ListOfTracks { get; }
        string ListOfCurrentlyBroadcastedTracks { get; }
        string ListOfRadioStations { get; }
        string Header { get; }
        string SelectedDate { get; }
    }

    public class OdsluchaneEuSelectorsRepository : IXPathSelectorsRepository
    {
        #region Implementation of IXPathSelectorsRepository

        public string ListOfTracks
        {
            get { return "//table[@class='wyniki']/tr"; }
        }

        public string ListOfCurrentlyBroadcastedTracks
        {
            get { return "//ul[@class='panel_aktualnie']/li"; }
        }

        public string ListOfRadioStations
        {
            get { return "//select[@name='r']/optgroup"; }
        }

        public string Header
        {
            get { return "td[@class='line']"; }
        }

        public string SelectedDate
        {
            get { return "//input[@name='date']"; }
        }

        #endregion

    }
}
