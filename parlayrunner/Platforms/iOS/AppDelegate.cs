using Foundation;
using UIKit;

namespace AdeptTime;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		// Set status bar content to light for visibility on dark background
		if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
		{
			var statusBar = new UIView(UIApplication.SharedApplication.KeyWindow?.WindowScene?.StatusBarManager?.StatusBarFrame ?? CoreGraphics.CGRect.Empty);
			statusBar.BackgroundColor = UIColor.FromRGB(74, 78, 124); // #4A4E7C
			UIApplication.SharedApplication.KeyWindow?.AddSubview(statusBar);
			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
		}
		else
		{
			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
		}

		return base.FinishedLaunching(application, launchOptions);
	}
}

