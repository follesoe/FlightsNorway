using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;

namespace FlightsNorway.Phone.Tests
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            Content = UnitTestSystem.CreateTestPage();
            var imtp = Content as IMobileTestPage;

            if (imtp != null)
            {
                BackKeyPress += (x, xe) => xe.Cancel = imtp.NavigateBack();
            }
        }
    }
}