using System;
using System.Windows;

namespace FlightsNorway
{
    public partial class App
    {
        public App()
        {
            UnhandledException += Application_UnhandledException;
            InitializeComponent();
        }

        private static void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
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