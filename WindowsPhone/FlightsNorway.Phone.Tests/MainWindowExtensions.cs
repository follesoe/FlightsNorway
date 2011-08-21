using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Silverlight.Testing;

namespace FlightsNorway.Tests
{
    public static class MainWindowExtensions
    {
        public static void StartTestRunner(this PhoneApplicationPage mainPage)
        {
            SystemTray.IsVisible = false;
            var testPage = UnitTestSystem.CreateTestPage() as IMobileTestPage;
            mainPage.BackKeyPress += (x, xe) => xe.Cancel = testPage.NavigateBack();
            (Application.Current.RootVisual as PhoneApplicationFrame).Content = testPage;
        }
    }
}
