using FlightsNorway.Phone.ViewModels;

namespace FlightsNorway.Phone
{
    public class ViewModelLocator
    {
        private readonly MicroContainer _container;

        public AirportsViewModel AirportsViewModel
        {
            get { return _container.Resolve<AirportsViewModel>(); }
        }

        public FlightsViewModel FlightsViewModel
        {
            get { return _container.Resolve<FlightsViewModel>(); }
        }

        public ViewModelLocator()
        {
            _container = new MicroContainer();
            _container.RegisterInstance(new AirportsViewModel());
            _container.RegisterInstance(new FlightsViewModel());
        }
    }
}