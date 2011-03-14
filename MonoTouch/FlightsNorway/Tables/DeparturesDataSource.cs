using System;
using System.Collections.ObjectModel;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;

using TinyIoC;

namespace FlightsNorway
{
	public class DeparturesDataSource : ViewModelDataSource<FlightsViewModel, Flight>
	{
		private NSString _cellID = new NSString("DepartureCell");
		
		public override NSString CellID { get { return _cellID; } }
				
		public override ObservableCollection<Flight> List { get { return ViewModel.Departures; } }				
		
		public DeparturesDataSource(FlightsViewModel viewModel) : base(viewModel)
		{
		}	
	}
}