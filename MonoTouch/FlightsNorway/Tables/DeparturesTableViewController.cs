using System;
using MonoTouch.UIKit;

namespace FlightsNorway
{
	public class DeparturesTableViewController : UITableViewController
	{
		private readonly DeparturesDataSource _dataSource;
		
		public DeparturesTableViewController(DeparturesDataSource dataSource)
		{
			_dataSource = dataSource;
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
			
			Title = "Departures";
			
			_dataSource.TableView = this.TableView;
			TableView.DataSource = _dataSource;
		}
	}
}