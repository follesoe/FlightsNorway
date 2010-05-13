using System;
using System.Windows;

namespace FlightsNorway.Phone.Tests
{
    public partial class App
    {
        public App()
        {
            UnhandledException += Application_UnhandledException;

            InitializeComponent();
        }

        // Code to execute on Unhandled Exceptions
        private static void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred, break in the debugger
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                // By default show the error
                e.Handled = true;
                MessageBox.Show(e.ExceptionObject.Message + Environment.NewLine + e.ExceptionObject.StackTrace, "Error", MessageBoxButton.OK);
            }
        }
    }
}