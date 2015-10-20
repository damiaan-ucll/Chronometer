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
		ISharedPreferences sharedPreferences;

		int time = 0;

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

			sharedPreferences = GetPreferences (FileCreationMode.Private);

			ShowCount ();
		}

		void ShowCount () { timeView.Text = time.ToString(); }

		void Increment() {
			time++;
			ShowCount ();
		}

		void Reset() {
			time = 0;
			ShowCount ();
		}
			
		protected override void OnPause ()
		{
			base.OnPause ();
			ISharedPreferencesEditor editor = sharedPreferences.Edit ();
			editor.PutInt ("time", time);
			editor.Commit ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			time = sharedPreferences.GetInt("time", 0);
			ShowCount ();
		}
	}
}