using System;
using System.Windows;
using FlightsNorway.Shared.Tests;
using Microsoft.Silverlight.Testing;

namespace FlightsNorway.Desktop.Tests
{
    public partial class App
    {
        public App()
        {
            Startup += Application_Startup;
            Exit += Application_Exit;
            UnhandledException += Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var settings = UnitTestSystem.CreateDefaultSettings();
            settings.TestAssemblies.Add(typeof(Tags).Assembly);

            settings.SampleTags.Clear();
            settings.SampleTags.Add(Tags.Model);
            settings.SampleTags.Add(Tags.ViewModel);
            settings.SampleTags.Add(Tags.WebService);
            settings.ShowTagExpressionEditor = true;

            RootVisual = UnitTestSystem.CreateTestPage(settings);
        }

        private static void Application_Exit(object sender, EventArgs e)
        {

        }

        private static void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (System.Diagnostics.Debugger.IsAttached) return;

            // NOTE: This will allow the application to continue running after an exception has been thrown
            // but not handled. 
            // For production applications this error handling should be replaced with something that will 
            // report the error to the website and stop the application.
            e.Handled = true;
            Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
        }

        private static void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
