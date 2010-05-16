using FlightsNorway.Phone.ViewModels;

namespace FlightsNorway.Phone
{
    public class ViewModelLocator
    {
        public AirportsViewModel AirportsViewModel
        {
            get { return new AirportsViewModel(); }
        }
    }
}
