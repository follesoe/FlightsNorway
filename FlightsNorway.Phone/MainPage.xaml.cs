using Microsoft.Phone.Controls;

namespace FlightsNorway.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
    }
}