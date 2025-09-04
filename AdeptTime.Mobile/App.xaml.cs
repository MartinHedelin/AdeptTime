using Microsoft.Extensions.DependencyInjection;
using AdeptTime.Shared.Services;

namespace AdeptTime;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		// Check authentication status and show appropriate page
		var userRoleService = serviceProvider.GetService<UserRoleService>();
		var isAuthenticated = !string.IsNullOrEmpty(userRoleService?.CurrentUserEmail);

		if (isAuthenticated)
		{
			MainPage = serviceProvider.GetRequiredService<MainShell>();
		}
		else
		{
			MainPage = new NavigationPage(serviceProvider.GetRequiredService<AdeptTime.Views.LoginView>());
		}
	}
}