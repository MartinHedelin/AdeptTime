using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Services;

namespace AdeptTime;

public partial class AppShell : Shell
{
    private readonly IAuthenticationService _authService;
    private readonly UserRoleService _userRoleService;

    public AppShell(IAuthenticationService authService, UserRoleService userRoleService)
    {
        InitializeComponent();
        
        _authService = authService;
        _userRoleService = userRoleService;
        
        // Check authentication on startup
        CheckAuthenticationAsync();
    }

    private async void CheckAuthenticationAsync()
    {
        try
        {
            var isAuthenticated = await _authService.IsAuthenticatedAsync();
            
            if (isAuthenticated)
            {
                // User is logged in, go to main app
                await GoToAsync("//main");
            }
            else
            {
                // User not logged in, go to login
                await GoToAsync("//login");
            }
        }
        catch
        {
            // If there's an error, default to login
            await GoToAsync("//login");
        }
    }
}
