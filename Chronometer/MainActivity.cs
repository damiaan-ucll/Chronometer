using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

public delegate TimeSpan duration();

namespace Chronometer
{
	[Activity (Label = "Chronometer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		public static duration zeroDuration = () => new TimeSpan(0);

		TextView timeView;
		Button startStop, reset;
		ISharedPreferences sharedPreferences;

		private DateTime? startTime = null;
		private DateTime? pauseTime = null;
		private Handler handler;
		private Action tickAction;
		private duration calculateDuration = zeroDuration;

		private Boolean isTicking {
			get {
				return startTime != null && pauseTime == null;
			}
		}

		void InitUIReferences () {
			timeView = FindViewById<TextView> (Resource.Id.timeView);
			startStop = FindViewById<Button> (Resource.Id.startStopButton);
			reset = FindViewById<Button> (Resource.Id.resetButton);

			handler = new Handler ();
		}

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			InitUIReferences ();
			tickAction = OnTick;

			startStop.Click += (sender, e) => toggleState();
			reset.Click += (sender, e) => ResetTimer();

			sharedPreferences = GetPreferences (FileCreationMode.Private);
		}

		void toggleState ()
		{
			if (isTicking) {
				stopCounting ();
			} else {
				startCounting ();
			}
		}

		private void startCounting() {
			if (pauseTime != null) {
				TimeSpan diff = DateTime.Now.Subtract (pauseTime.Value);
				startTime = startTime.Value.Add (diff);
				pauseTime = null;
			} else {
				startTime = DateTime.Now;
			}
			calculateDuration = durationWhenTicking;
			GenerateDelayedTick ();

			startStop.Text = Resources.GetText(Resource.String.stop);
		}

		private void stopCounting() {
			calculateDuration = durationWhenPaused;
			pauseTime = DateTime.Now;

			startStop.Text = Resources.GetText(Resource.String.start);
		}

		private void ResetTimer() {
			startTime = null;
			pauseTime = null;
			calculateDuration = zeroDuration;
			UpdateTextField ();

			startStop.Text = Resources.GetText(Resource.String.start);
		}

		void UpdateTextField ()
		{
			TimeSpan interval = calculateDuration();
			timeView.Text = $"{interval.Hours.ToString()}:{interval.Minutes.ToString()}:{interval.Seconds.ToString()}:{interval.Milliseconds.ToString()}";
		}

		private void GenerateDelayedTick() {
			handler.PostDelayed(tickAction, 100);
		}

		private void OnTick()
		{
			UpdateTextField();

			// Have the next tick generated in 100ms
			if (isTicking)
				GenerateDelayedTick();
		}

		private TimeSpan durationWhenTicking() {
			return DateTime.Now.Subtract (startTime.Value);
		}

		private TimeSpan durationWhenPaused() {
			return pauseTime.Value.Subtract (startTime.Value);
		}

		protected override void OnPause () {
			base.OnPause ();
			ISharedPreferencesEditor editor = sharedPreferences.Edit ();
			if (startTime == null) {
				editor.Remove ("start");
			} else {
				editor.PutLong ("start", startTime.Value.ToBinary());
			}

			if (pauseTime == null) {
				editor.Remove ("pause");
			} else {
				editor.PutLong ("pause", pauseTime.Value.ToBinary());
			}
			editor.Commit ();
		}

		protected override void OnResume () {
			base.OnResume ();
			if (sharedPreferences.Contains ("start"))
				startTime = DateTime.FromBinary (sharedPreferences.GetLong ("start", DateTime.Now.ToBinary ()));
			else
				startTime = null;
			if (sharedPreferences.Contains ("pause"))
				pauseTime = DateTime.FromBinary (sharedPreferences.GetLong ("pause", DateTime.Now.ToBinary ()));
			else
				pauseTime = null;
			GenerateDelayedTick ();

			if (pauseTime != null)
				calculateDuration = durationWhenPaused;
			else if (startTime != null)
				calculateDuration = durationWhenTicking;
			else
				calculateDuration = zeroDuration;
		}
	}
}