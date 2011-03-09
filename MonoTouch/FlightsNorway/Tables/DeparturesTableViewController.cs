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
			
			_dataSource.Controller = this;
			TableView.DataSource = _dataSource;
		}
	}
}