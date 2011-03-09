using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using System.Collections.ObjectModel;

namespace FlightsNorway
{
	public class ArrivalsDataSource : ViewModelDataSource<FlightsViewModel, Flight>
	{
		private NSString _cellID = new NSString("ArrivalCell");
		
		public override NSString CellID { get { return _cellID; } }
				
		public override ObservableCollection<Flight> List { get { return ViewModel.Arrivals; } }				
		
		public ArrivalsDataSource(FlightsViewModel viewModel) : base(viewModel)
		{
		}
	}
}