using System;
using System.Collections.Generic;

using MonoTouch.UIKit;

using MonoTouch.Foundation;
using TinyIoC;

namespace FlightsNorway.Tables
{
	public class AirportsTableViewController : UITableViewController
	{				
		public AirportsTableViewController()
		{		
			
		}				
				
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();		
			
			var dataSource = TinyIoCContainer.Current.Resolve<AirportsDataSource>();
			dataSource.Controller = this;
			
			TableView.DataSource = dataSource;
		}
	}
}