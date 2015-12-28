using System;
		
using UIKit;

namespace Chronometer.iOS
{
	public partial class ViewController : UIViewController
	{
		Chronometer chrono = new Chronometer (80);

		public ViewController (IntPtr handle) : base (handle)
		{		
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Code to start the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
			#endif

			chrono.Timer.Elapsed += (sender, e) => InvokeOnMainThread(() => Time.Text = chrono.Time.ToString(@"hh\:mm\:ss\:fff"));

			StartStop.TouchUpInside += delegate {
				chrono.StartStop();
				StartStop.SetTitle(chrono.isTicking ? "Stop" : "Start", UIControlState.Normal);
			};

			Reset.TouchUpInside += delegate {
				chrono.Reset();
				StartStop.SetTitle("Start", UIControlState.Normal);
				Time.Text = chrono.Time.ToString(@"hh\:mm\:ss\:fff");
			};
		}


	}
}
