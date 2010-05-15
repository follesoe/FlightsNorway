using FlightsNorway.Phone.ViewModels;

namespace FlightsNorway.Phone
{
    public class ViewModelLocator
    {
        public FlightsViewModel FlightsViewModel
        {
            get { return new FlightsViewModel(); }
        }
    }
}
