using System;
using FlightsNorway.Lib;
using MonoTouch.Foundation;

namespace FlightsNorway
{
	public class DispatchAdapter : IDispatchOnUIThread
	{
		private readonly NSObject _owner;
			
		public DispatchAdapter(NSObject owner)
		{
			_owner = owner;
		}
	
		public void Invoke (Action action)
		{
			_owner.BeginInvokeOnMainThread(new NSAction(action));
		}
	}
}

