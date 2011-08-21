using System.Windows;
using Microsoft.Phone.Controls;

namespace FlightsNorway.Views
{
    public partial class MainPage
    {
        public ViewModelLocator Locator
        {
            get { return (ViewModelLocator) Application.Current.Resources["ViewModelLocator"]; }
        }

        public MainPage()
        {
            InitializeComponent();
           
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            Locator.FlightsViewModel.PropertyChanged += (o, e) => {
                if(e.PropertyName.Equals("IsBusy"))
                {
                    _progressBar.IsIndeterminate = Locator.FlightsViewModel.IsBusy;
                }
            };
        }
    }
}