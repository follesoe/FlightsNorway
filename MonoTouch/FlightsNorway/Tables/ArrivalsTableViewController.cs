using System;
using MonoTouch.UIKit;

namespace FlightsNorway
{
	public class ArrivalsTableViewController : UITableViewController
	{
		private readonly ArrivalsDataSource _dataSource;
		
		public ArrivalsTableViewController(ArrivalsDataSource dataSource)
		{
			_dataSource = dataSource;
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();		
			
			_dataSource.Controller = this;
			TableView.DataSource = _dataSource;
		}
	}
}