using Foundation;
using UIKit;

namespace AdeptTime;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		// Set status bar to light content for dark backgrounds
		UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
		
		// Configure tab bar appearance globally
		ConfigureTabBarAppearance();
		
		return base.FinishedLaunching(application, launchOptions);
	}

	private void ConfigureTabBarAppearance()
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
		{
			var appearance = new UITabBarAppearance();
			appearance.ConfigureWithOpaqueBackground();
			appearance.BackgroundColor = UIColor.FromRGB(74, 78, 124); // #4A4E7C
			
			// Configure normal (unselected) state
			var normalState = appearance.StackedLayoutAppearance.Normal;
			normalState.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.FromRGB(200, 200, 200), // #C8C8C8
				Font = UIFont.SystemFontOfSize(10)
			};
			normalState.IconColor = UIColor.FromRGB(200, 200, 200);
			
			// Configure selected state
			var selectedState = appearance.StackedLayoutAppearance.Selected;
			selectedState.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = UIColor.White,
				Font = UIFont.SystemFontOfSize(10)
			};
			selectedState.IconColor = UIColor.White;
			
			// Apply appearance globally
			UITabBar.Appearance.StandardAppearance = appearance;
			UITabBar.Appearance.ScrollEdgeAppearance = appearance;
		}
	}
}

