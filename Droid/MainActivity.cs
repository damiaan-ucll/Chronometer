using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

using System;

namespace Chronometer.Droid
{
	[Activity (Label = "Chronometer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		TextView timeView;
		Button startStop, reset;
		ISharedPreferences sharedPreferences;

		private readonly Chronometer chrono = new Chronometer(80);

		protected override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			InitUIReferences ();

			startStop.Click += delegate {
				chrono.StartStop();
				UpdateStartStopButtonText();
			};
			reset.Click += delegate {
				chrono.Reset();
				startStop.Text = Resources.GetText(Resource.String.start);
				UpdateTimeField();
			};

			chrono.Timer.Elapsed += (sender, e) => RunOnUiThread(() => UpdateTimeField());

			sharedPreferences = GetPreferences (FileCreationMode.Private);
		}

		void InitUIReferences () {
			timeView = FindViewById<TextView> (Resource.Id.timeView);
			startStop = FindViewById<Button> (Resource.Id.startStopButton);
			reset = FindViewById<Button> (Resource.Id.resetButton);
		}


		void UpdateTimeField (){
			timeView.Text = chrono.Time.ToString(@"hh\:mm\:ss\:fff");
		}

		void UpdateStartStopButtonText() {
			startStop.Text = chrono.isTicking ? Resources.GetText(Resource.String.stop) : Resources.GetText(Resource.String.start);
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

			UpdateTimeField ();
			UpdateStartStopButtonText ();
		}
	}
}