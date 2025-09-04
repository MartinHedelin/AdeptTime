using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Services;
using AdeptTime.Shared.Models;

namespace AdeptTime.Views;

public partial class KalenderView : ContentPage
{
    private readonly ITimeRegistrationService _timeRegistrationService;
    private readonly ITeamService _teamService;
    private readonly UserRoleService _userRoleService;

    public KalenderView(ITimeRegistrationService timeRegistrationService, ITeamService teamService, UserRoleService userRoleService)
    {
        InitializeComponent();
        _timeRegistrationService = timeRegistrationService;
        _teamService = teamService;
        _userRoleService = userRoleService;
        
        InitializeMap();
    }

    private void InitializeMap()
    {
        // Set map to Copenhagen area to match reference design
        var copenhagenLocation = new Location(55.6761, 12.5683);
        var mapSpan = MapSpan.FromCenterAndRadius(copenhagenLocation, Distance.FromKilometers(10));
        map.MoveToRegion(mapSpan);
    }

    private async void OnAddTimeClicked(object sender, EventArgs e)
    {
        try
        {
            // Simple time registration for demo
            var result = await DisplayPromptAsync(
                "Tilføj Timer", 
                "Hvor mange timer arbejdede du?", 
                "OK", 
                "Annuller", 
                "8.5", 
                keyboard: Keyboard.Numeric);

            if (!string.IsNullOrEmpty(result) && decimal.TryParse(result, out var hours))
            {
                // For demo: create with fixed IDs that exist in database
                // In real app: get actual logged-in user ID and their team ID
                var demoUserIds = new[] 
                {
                    "admin@test.com",
                    "worker@test.com", 
                    "john@test.com"
                };
                
                var randomEmail = demoUserIds[Random.Shared.Next(demoUserIds.Length)];
                Console.WriteLine($"[Mobile] Creating time registration for demo user: {randomEmail}");
                
                // Create time registration with demo data
                var timeRegistration = new TimeRegistration
                {
                    UserId = Guid.NewGuid(), // Demo - will use in-memory fallback
                    TeamId = Guid.NewGuid(), // Demo - will use in-memory fallback
                    Date = DateTime.Today,
                    CheckIn = new TimeSpan(8, 0, 0), // 8:00 AM
                    CheckOut = new TimeSpan(8, 0, 0).Add(TimeSpan.FromHours((double)hours)), // Calculate checkout
                    TotalHours = hours,
                    TimeBank = hours > 8 ? hours - 8 : 0, // Overtime calculation
                    Status = "Afventer", // Pending approval
                    Description = $"Mobile time entry - {hours} timer ({DateTime.Now:HH:mm})",
                    User = new User { Name = "Mobile Worker", Email = randomEmail },
                    Team = new Team { Name = "Team Mobile" }
                };

                // Save to database (will use demo mode if DB unavailable)
                await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistration);
                
                await DisplayAlert("Success", $"Tilføjet {hours} timer til din timebank!\n\nCheck Timeoversigt på web for at se indførslen.", "OK");
                Console.WriteLine($"✅ Mobile time registration created: {hours} hours - should appear in Timeoversigt");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Kunne ikke tilføje timer: {ex.Message}", "OK");
            Console.WriteLine($"❌ Failed to create mobile time registration: {ex.Message}");
        }
    }
}
