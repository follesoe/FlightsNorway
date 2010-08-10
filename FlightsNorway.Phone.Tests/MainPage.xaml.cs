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
            settings.TestAssemblies.Add(typeof(Tags).Assembly);

            settings.SampleTags.Clear();
            settings.SampleTags.Add(Tags.Model);
            settings.SampleTags.Add(Tags.ViewModel);
            settings.SampleTags.Add(Tags.WebService);
            settings.ShowTagExpressionEditor = true;

            Content = UnitTestSystem.CreateTestPage(settings);

            var imtp = Content as IMobileTestPage;

            if (imtp != null)
            {
                BackKeyPress += (x, xe) => xe.Cancel = imtp.NavigateBack();
            }
        }
    }
}