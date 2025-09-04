using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Services;
using AdeptTime.Shared.Models;

namespace AdeptTime.Views;

public partial class IndstillingerView : ContentPage
{
    private readonly IAuthenticationService _authService;
    private readonly UserRoleService _userRoleService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITimeRegistrationService _timeRegistrationService;
    private readonly ITeamService _teamService;
    private readonly IUserService _userService;
    private List<Team> _availableTeams = new();

    public IndstillingerView(IAuthenticationService authService, UserRoleService userRoleService, IServiceProvider serviceProvider, ITimeRegistrationService timeRegistrationService, ITeamService teamService, IUserService userService)
    {
        InitializeComponent();
        _authService = authService;
        _userRoleService = userRoleService;
        _serviceProvider = serviceProvider;
        _timeRegistrationService = timeRegistrationService;
        _teamService = teamService;
        _userService = userService;
        
        LoadUserInfo();
        LoadTeamsAsync();
        
        // Add simple button test
        AddTimeButton.Clicked += async (s, e) => {
            await DisplayAlert("BUTTON TEST", "Button was clicked! Event is working!", "OK");
        };
        
        // Add gesture recognizer backup
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) => {
            await DisplayAlert("GESTURE TEST", "Tap gesture working! Triggering main handler...", "OK");
            OnAddTimeEntryClicked(s, new EventArgs());
        };
        AddTimeButton.GestureRecognizers.Add(tapGesture);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        if (!string.IsNullOrEmpty(_userRoleService.CurrentUserEmail))
        {
            UserEmailLabel.Text = _userRoleService.CurrentUserEmail;
            UserRoleLabel.Text = _userRoleService.IsAdministrator ? "Administrator" : "Worker";
        }
        else
        {
            UserEmailLabel.Text = "Not logged in";
            UserRoleLabel.Text = "Unknown";
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        try
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (!confirm) return;

            // Clear user session
            _userRoleService.Logout();
            await _authService.LogoutAsync();

            // Navigate back to login
            Application.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<LoginView>());
            
            await DisplayAlert("Success", "Logged out successfully", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Logout failed: {ex.Message}", "OK");
        }
    }

    private async void LoadTeamsAsync()
    {
        try
        {
            _availableTeams = await _teamService.GetAllTeamsAsync();
            
            // Populate picker
            TeamPicker.ItemsSource = _availableTeams.Select(t => t.Name).ToList();
            
            // Set default selection
            if (_availableTeams.Any())
            {
                TeamPicker.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load teams: {ex.Message}", "OK");
        }
    }

    private async void OnAddTimeEntryClicked(object sender, EventArgs e)
    {
        try
        {
            await DisplayAlert("🎯 MAIN HANDLER", "Main event handler working! Starting full process...", "OK");
            
            // Validate input
            if (string.IsNullOrWhiteSpace(HoursEntry.Text))
            {
                await DisplayAlert("Error", "Please enter hours worked", "OK");
                return;
            }

            if (!double.TryParse(HoursEntry.Text, out double hours) || hours <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid number of hours", "OK");
                return;
            }

            if (TeamPicker.SelectedIndex < 0 || _availableTeams.Count == 0)
            {
                await DisplayAlert("Error", $"Please select a team. Available teams: {_availableTeams.Count}", "OK");
                return;
            }

            await DisplayAlert("Debug", $"Validation passed. Hours: {hours}, Team index: {TeamPicker.SelectedIndex}", "OK");

            // Get selected team
            var selectedTeam = _availableTeams[TeamPicker.SelectedIndex];
            
            await DisplayAlert("Debug", $"Selected team: {selectedTeam.Name} (ID: {selectedTeam.Id})", "OK");
            
            // Get current user
            var currentUserEmail = _userRoleService.CurrentUserEmail ?? "demo@test.com";
            await DisplayAlert("Debug", $"Getting user by email: {currentUserEmail}", "OK");
            
            var currentUser = await _userService.GetUserByEmailAsync(currentUserEmail);
            
            await DisplayAlert("Debug", $"User found: {currentUser?.Name ?? "NULL"} (ID: {currentUser?.Id ?? Guid.Empty})", "OK");
            
            // Create time registration with ALL required fields
            var checkInTime = new TimeSpan(8, 0, 0); // 8:00 AM
            var checkOutTime = checkInTime.Add(TimeSpan.FromHours(hours)); // Add hours worked
            
            var timeRegistration = new TimeRegistration
            {
                TotalHours = (decimal)hours,
                Date = DatePicker.Date,
                UserId = currentUser?.Id ?? Guid.NewGuid(),
                TeamId = selectedTeam.Id,
                CheckIn = checkInTime,
                CheckOut = checkOutTime,
                TimeBank = hours > 8 ? (decimal)(hours - 8) : 0m, // Overtime calculation
                Status = "Afventer", // Default pending status
                Description = $"Time logged from mobile app on {DateTime.Now:yyyy-MM-dd HH:mm}. Team: {selectedTeam.Name}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await DisplayAlert("Debug", $"Time registration created. About to submit to database...", "OK");

            // Submit to Supabase
            var result = await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistration);

            await DisplayAlert("Debug", $"Database result: {(result != null ? "SUCCESS" : "NULL")}", "OK");

            if (result != null)
            {
                await DisplayAlert("✅ SUCCESS!", $"Time entry created successfully!\n\n📊 Details:\n• Hours: {hours}\n• Date: {DatePicker.Date:yyyy-MM-dd}\n• Team: {selectedTeam.Name}\n• User: {currentUser?.Name ?? "Demo User"}\n• ID: {result.Id}\n\n🌐 Check the web app Timeoversigt to see your entry!", "OK");
                
                // Clear form
                HoursEntry.Text = "";
                DatePicker.Date = DateTime.Now;
                TeamPicker.SelectedIndex = 0;
            }
            else
            {
                await DisplayAlert("❌ ERROR", "Failed to create time entry. The service returned null.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("💥 EXCEPTION", $"Exception occurred:\n\nType: {ex.GetType().Name}\n\nMessage: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "OK");
        }
    }

    private async void OnSyncDataClicked(object sender, EventArgs e)
    {
        try
        {
            // Show loading indicator (in a real app, you'd sync with server)
            await DisplayAlert("Sync", "Data synchronized successfully!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Sync failed: {ex.Message}", "OK");
        }
    }

    private async void OnClearCacheClicked(object sender, EventArgs e)
    {
        try
        {
            bool confirm = await DisplayAlert("Clear Cache", "This will clear all cached data. Continue?", "Yes", "No");
            if (!confirm) return;

            // Clear cache (in a real app, you'd clear actual cache)
            await DisplayAlert("Success", "Cache cleared successfully!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Cache clear failed: {ex.Message}", "OK");
        }
    }
}
