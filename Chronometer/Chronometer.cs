using System;
using System.Timers;

namespace Chronometer
{

	public delegate TimeSpan duration();

	public class Chronometer
	{

		public readonly Timer Timer;

		private DateTime? startTime = null;
		private DateTime? pauseTime = null;

		public DateTime? StartTime {
			get {
				return startTime;
			}
		}
		public DateTime? PauseTime {
			get {
				return pauseTime;
			}
		}

		private duration time = beginTime;

		public TimeSpan Time {
			get {
				return time ();
			}
		}

		public Chronometer (Double refreshRate)
		{
			Timer = new Timer (refreshRate);
		}

		public Boolean isTicking {
			get {
				return startTime != null && pauseTime == null;
			}
		}

		public void StartStop() {
			if (isTicking) {
				Stop ();
			} else {
				Start ();
			}
		}

		private void Start() {
			if (pauseTime != null) {
				TimeSpan diff = DateTime.Now.Subtract (pauseTime.Value);
				startTime = startTime.Value.Add (diff);
				pauseTime = null;
			} else {
				startTime = DateTime.Now;
			}

			Timer.Start ();

			time = calculateTimeWhenTicking;
		}

		private void Stop() {
			time = calculateTimeWhenPaused;
			pauseTime = DateTime.Now;

			Timer.Stop ();

			time = calculateTimeWhenPaused;
		}

		public void Reset() {
			startTime = null;
			pauseTime = null;
			time = beginTime;

			Timer.Stop ();
		}

		public void importStartPauseTimes(DateTime? start, DateTime? pause) {
			startTime = start;
			pauseTime = pause;

			if (pauseTime != null)
				time = calculateTimeWhenPaused;
			else if (startTime != null) {
				time = calculateTimeWhenTicking;
				Timer.Start ();
			}
			else
				time = beginTime;
		}

		public static TimeSpan beginTime() {
			return new TimeSpan (0);
		}

		private TimeSpan calculateTimeWhenTicking() {
			return DateTime.Now.Subtract (startTime.Value);
		}

		private TimeSpan calculateTimeWhenPaused() {
			return pauseTime.Value.Subtract (startTime.Value);
		}
	}
}