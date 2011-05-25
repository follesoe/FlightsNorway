using System;
using System.Windows;
using System.Windows.Controls;
using FlightsNorway.Lib.ViewModels;

namespace FlightsNorway.Views
{
    public partial class AirportsView : UserControl
    {
        public AirportsView()
        {
            InitializeComponent();

            Loaded += AirportsView_Loaded;
        }

        private void AirportsView_Loaded(object sender, RoutedEventArgs e)
        {
            var fileUri = new Uri("FlightsNorway;component/Content/Airports.xml",
                                    UriKind.Relative);

            using (var stream = Application.GetResourceStream(fileUri).Stream)
            {
                DataContext = new AirportsViewModel(stream);
            }
        }

    }
}
