using System;
using MonoTouch.UIKit;
using TinyMessenger;
using FlightsNorway.Lib.Messages;

namespace FlightsNorway
{
	public class ArrivalsTableViewController : UITableViewController
	{
		private readonly ArrivalsDataSource _dataSource;
		private readonly ITinyMessengerHub _messenger;
		
		public ArrivalsTableViewController(ArrivalsDataSource dataSource, ITinyMessengerHub messenger)
		{
			_dataSource = dataSource;
			
			_messenger = messenger;
			_messenger.Subscribe<AirportSelectedMessage>(msg => NavigationItem.Title = string.Format("Arrivals at {0}", msg.Content.Code));
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();		
			
			Title = "Arrivals";
			
			_dataSource.TableView = this.TableView;
			TableView.DataSource = _dataSource;
		}		
	}
}