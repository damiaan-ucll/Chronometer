using UIKit;

namespace Chronometer.iOS {
	public class Application {
		static void Main (string[] args) {
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey);
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
