using System;

using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

using Chronometer.Core;

namespace Chronometer.Droid
{
	[Activity (Label = "Chronometer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		TextView timeView;
		Button startStop, reset;
		ISharedPreferences sharedPreferences;

		private readonly Core.Chronometer chrono = new Core.Chronometer(100);

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			InitUIReferences ();

			startStop.Click += delegate {
				chrono.StartStop();
				startStop.Text = chrono.isTicking ? "Stop" : "Start";
			};
			reset.Click += delegate {
				chrono.Reset();
				startStop.Text = "Start";
				UpdateTextField();
			};

			chrono.Timer.Elapsed += (sender, e) => RunOnUiThread(() => UpdateTextField());

			sharedPreferences = GetPreferences (FileCreationMode.Private);
		}

		void InitUIReferences () {
			timeView = FindViewById<TextView> (Resource.Id.timeView);
			startStop = FindViewById<Button> (Resource.Id.startStopButton);
			reset = FindViewById<Button> (Resource.Id.resetButton);
		}


		void UpdateTextField ()
		{
			timeView.Text = chrono.Time.ToString(@"hh\:mm\:ss\:fff");
		}
			
		protected override void OnPause () {
			base.OnPause ();
			ISharedPreferencesEditor editor = sharedPreferences.Edit ();
			if (chrono.StartTime == null) {
				editor.Remove ("start");
			} else {
				editor.PutLong ("start", chrono.StartTime.Value.ToBinary());
			}

			if (chrono.PauseTime == null) {
				editor.Remove ("pause");
			} else {
				editor.PutLong ("pause", chrono.PauseTime.Value.ToBinary());
			}
			editor.Commit ();
			chrono.Timer.Stop ();
		}

		protected override void OnResume () {
			base.OnResume ();

			DateTime? start = null, pause = null;

			if (sharedPreferences.Contains ("start"))
				start = DateTime.FromBinary (sharedPreferences.GetLong ("start", DateTime.Now.ToBinary ()));
			if (sharedPreferences.Contains ("pause"))
				pause = DateTime.FromBinary (sharedPreferences.GetLong ("pause", DateTime.Now.ToBinary ()));

			chrono.importStartPauseTimes (start, pause);

			UpdateTextField ();
		}
	}
}