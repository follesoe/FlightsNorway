using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;

namespace FlightsNorway.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
           
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            ApplicationBar = new ApplicationBar();
            new ApplicationBarController(ApplicationBar);
        }        
    }
}