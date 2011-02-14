using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;

namespace FlightsNorway.Tests
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
                       
            var settings = UnitTestSystem.CreateDefaultSettings();
                        
            Content = UnitTestSystem.CreateTestPage(settings);
            var imtp = Content as IMobileTestPage;

            if (imtp != null)
            {
                BackKeyPress += (x, xe) => xe.Cancel = imtp.NavigateBack();
            }
        }
    }
}