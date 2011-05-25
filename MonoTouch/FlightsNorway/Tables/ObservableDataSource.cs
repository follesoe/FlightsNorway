using System;
using MonoTouch.UIKit;
using System.Collections.ObjectModel;
using MonoTouch.Foundation;

namespace FlightsNorway
{
	public abstract class ObservableDataSource<T> : UITableViewDataSource
	{
		private ObservableCollection<T> _collection;
		private UITableView _tableView;
			
		public ObservableDataSource (ObservableCollection<T> collection, UITableView tableView)
		{
			_tableView = tableView;			
			_collection = collection;
			_collection.CollectionChanged += (o, e) => {
				tableView.ReloadData();
			};
		}
		
		public abstract NSString CellID { get; }
		
		public override int RowsInSection (UITableView tableView, int section)
		{
			return _collection.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
		    var cell = tableView.DequeueReusableCell(CellID);
		    if (cell == null)
		    {					
		        cell = new UITableViewCell(UITableViewCellStyle.Default, CellID);
				cell.TextLabel.Font = UIFont.FromName("Georgia", 16f);
		    }
		        				
		    cell.TextLabel.Text = _collection[indexPath.Row].ToString();				
		    return cell;
		}
	}
}

