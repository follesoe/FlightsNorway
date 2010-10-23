using Microsoft.Phone.Controls;

namespace FlightsNorway.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
           
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
        }        
    }
}