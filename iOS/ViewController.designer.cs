// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Chronometer.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton Button { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Reset { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton StartStop { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel Time { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Reset != null) {
				Reset.Dispose ();
				Reset = null;
			}
			if (StartStop != null) {
				StartStop.Dispose ();
				StartStop = null;
			}
			if (Time != null) {
				Time.Dispose ();
				Time = null;
			}
		}
	}
}
