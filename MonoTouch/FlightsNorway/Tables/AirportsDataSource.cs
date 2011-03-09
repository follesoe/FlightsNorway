using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Tables;
using System.Collections.ObjectModel;

namespace FlightsNorway
{
	public class AirportsDataSource : ViewModelDataSource<AirportsViewModel, Airport>
	{	
		private NSString _cellID = new NSString("AirportCell");
		
		public override NSString CellID { get { return _cellID; } }			
		public override ObservableCollection<Airport> List { get { return ViewModel.Airports; } }		
		
		public AirportsDataSource(AirportsViewModel viewModel) : base(viewModel)
		{
		}
							
		public void SetSelectedRow()
		{
			if(ViewModel.SelectedAirport != null)
			{
				int index = ViewModel.Airports.IndexOf(ViewModel.SelectedAirport);
				TableView.SelectRow(NSIndexPath.FromRowSection(index, 0), false, UITableViewScrollPosition.None);
			}
		}					
	}
}