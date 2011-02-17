using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Shell;

namespace FlightsNorway
{
    public partial class App
    {
        public App()
        {
            UnhandledException += Application_UnhandledException;            
            InitializeComponent();
        }

        private void Current_Closing(object sender, ClosingEventArgs e)
        {
            Debug.WriteLine("Closing");
        }

        private void Current_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Debug.WriteLine("Deactivated");
        }

        private void Current_Activated(object sender, ActivatedEventArgs e)
        {
            Debug.WriteLine("Activated");
        }

        private void Current_Launching(object sender, LaunchingEventArgs e)
        {
            Debug.WriteLine("Launching");
        }

        private static void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                e.Handled = true;
                MessageBox.Show(e.ExceptionObject.Message + Environment.NewLine + e.ExceptionObject.StackTrace, "Error", MessageBoxButton.OK);
            }
        }
    }
}