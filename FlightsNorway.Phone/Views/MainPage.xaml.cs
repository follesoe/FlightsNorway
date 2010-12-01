using System.Windows.Navigation;
using FlightsNorway.Extensions;
using Microsoft.Phone.Controls;

namespace FlightsNorway.Views
{
    public partial class MainPage
    {
        private const string SelectedPivotIndexKey = "SelectedPivotIndexKey";

        public MainPage()
        {
            InitializeComponent();
           
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (State.ContainsKey(SelectedPivotIndexKey))
            {
                _mainPivot.SelectedIndex = State.Get<int>(SelectedPivotIndexKey);
            }

            _arrivalsView.LoadTransientState(State);
            _departuresView.LoadTransientState(State);
            _airportsView.LoadTransientState(State);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {            
            State.AddOrReplace(SelectedPivotIndexKey, _mainPivot.SelectedIndex);
            
            _arrivalsView.SaveTransientState(State);
            _departuresView.SaveTransientState(State);
            _airportsView.SaveTransientState(State);
        }
    }
}