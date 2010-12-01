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

        void Current_Closing(object sender, ClosingEventArgs e)
        {
            Debug.WriteLine("Closing");
        }

        void Current_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Debug.WriteLine("Deactivated");
        }

        void Current_Activated(object sender, ActivatedEventArgs e)
        {
            Debug.WriteLine("Activated");
        }

        void Current_Launching(object sender, LaunchingEventArgs e)
        {
            Debug.WriteLine("Launching");
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