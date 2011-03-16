using System;
using FlightsNorway.Lib.MVVM;

using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.ObjectModel;

namespace FlightsNorway
{
	public abstract class ViewModelDataSource<TViewModel, TModel> : UITableViewDataSource
																	where TViewModel : ViewModelBase 
																  	where TModel : class
	{
		public UITableView TableView { get; set; }
		
		public TViewModel ViewModel { get; private set; }
		
		public abstract NSString CellID { get; }
		
		public abstract ObservableCollection<TModel> List { get; }
					
		public ViewModelDataSource(TViewModel viewModel)
		{
			ViewModel = viewModel;
			
			List.CollectionChanged += (o, e) => {
				if(TableView != null)
					TableView.ReloadData();
			};
		}
		
		public override int RowsInSection (UITableView tableView, int section)
		{
			return List.Count;
		}		
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
			var cell = tableView.DequeueReusableCell(CellID);
            if (cell == null)
            {					
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellID);
				cell.TextLabel.Font = UIFont.FromName("Georgia", 16f);
            }
        				
            cell.TextLabel.Text = List[indexPath.Row].ToString();				
            return cell;
        }	
	}
}