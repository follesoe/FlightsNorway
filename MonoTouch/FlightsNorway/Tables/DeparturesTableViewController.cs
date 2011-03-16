using System;
using MonoTouch.UIKit;
using TinyMessenger;
using FlightsNorway.Lib.Messages;
using System.Drawing;

namespace FlightsNorway
{
	public class DeparturesTableViewController : UITableViewController
	{
		private readonly DeparturesDataSource _dataSource;
		private readonly ITinyMessengerHub _messenger;
		
		public DeparturesTableViewController(DeparturesDataSource dataSource, ITinyMessengerHub messenger)
		{
			_dataSource = dataSource;
			_messenger = messenger;
			_messenger.Subscribe<AirportSelectedMessage>(msg => NavigationItem.Title = string.Format("Departures from {0}", msg.Content.Code));			
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