using System;
using System.Windows;
using FlightsNorway.Lib;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using Microsoft.Phone.Controls;

namespace FlightsNorway
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ServiceLocator.Dispatcher = new DispatchAdapter();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new FlightsViewModel();
            _arrivalsView.DataContext = viewModel;
            _departuresView.DataContext = viewModel;
        }
    }
}