using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;

namespace FlightsNorway.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
           
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            // Not sure if an application bar is needed in this ui.
            //ApplicationBar = new ApplicationBar();
            //new ApplicationBarController(ApplicationBar);
        }        
    }
}