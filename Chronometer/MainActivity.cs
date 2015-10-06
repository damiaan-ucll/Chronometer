using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Chronometer
{
	[Activity (Label = "Chronometer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		TextView timeView;
		Button startStop, reset;

		int count = 0;

		void InitUIReferences ()
		{
			timeView = FindViewById<TextView> (Resource.Id.timeView);
			startStop = FindViewById<Button> (Resource.Id.startStopButton);
			reset = FindViewById<Button> (Resource.Id.resetButton);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			InitUIReferences ();

			startStop.Click += (sender, eventArgs) => Increment ();
			reset.Click += (sender, e) => Reset();

			ShowCount ();
		}

		void ShowCount () { timeView.Text = count.ToString(); }

		void Increment() {
			count++;
			ShowCount ();
		}

		void Reset() {
			count = 0;
			ShowCount ();
		}


	}
}
