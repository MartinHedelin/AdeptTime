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
    private readonly ICaseService _caseService;
    private readonly UserRoleService _userRoleService;

    public KalenderView(ITimeRegistrationService timeRegistrationService, ITeamService teamService, ICaseService caseService, UserRoleService userRoleService)
    {
        InitializeComponent();
        _timeRegistrationService = timeRegistrationService;
        _teamService = teamService;
        _caseService = caseService;
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
            await DisplayAlert("Debug", "Button clicked! Loading cases...", "OK");
            
            // Step 1: Load available cases
            var allCases = await _caseService.GetAllCasesAsync();
            await DisplayAlert("Debug", $"Found {allCases?.Count() ?? 0} cases", "OK");
            
            var caseOptions = allCases?.Select(c => $"{c.CaseNumber}: {c.Title}").ToArray() ?? new string[0];
            
            if (!caseOptions.Any())
            {
                await DisplayAlert("Ingen Sager", "Ingen tilgængelige sager fundet. Kontakt din administrator.", "OK");
                return;
            }

            // Step 2: Let user select a case
            var selectedCaseDisplay = await DisplayActionSheet(
                "Vælg Sag", 
                "Annuller", 
                null, 
                caseOptions);

            if (selectedCaseDisplay == "Annuller" || string.IsNullOrEmpty(selectedCaseDisplay))
                return;

            var selectedCase = allCases.FirstOrDefault(c => selectedCaseDisplay.StartsWith(c.CaseNumber));
            if (selectedCase == null)
            {
                await DisplayAlert("Error", "Kunne ikke finde den valgte sag", "OK");
                return;
            }

            // Step 3: Get hours worked
            var hoursResult = await DisplayPromptAsync(
                $"Timer for {selectedCase.CaseNumber}", 
                "Hvor mange timer arbejdede du på denne sag?", 
                "OK", 
                "Annuller", 
                "8.5", 
                keyboard: Keyboard.Numeric);

            if (!string.IsNullOrEmpty(hoursResult) && decimal.TryParse(hoursResult, out var hours))
            {
                // Create time registration linked to the selected case
                var timeRegistration = new TimeRegistration
                {
                    UserId = Guid.NewGuid(), // Demo - will use in-memory fallback
                    TeamId = selectedCase.TeamId ?? Guid.NewGuid(),
                    Date = DateTime.Today,
                    CheckIn = new TimeSpan(8, 0, 0), // 8:00 AM
                    CheckOut = new TimeSpan(8, 0, 0).Add(TimeSpan.FromHours((double)hours)), // Calculate checkout
                    TotalHours = hours,
                    TimeBank = hours > 8 ? hours - 8 : 0, // Overtime calculation
                    Status = "Afventer", // Pending approval
                    Description = $"Mobile: {selectedCase.CaseNumber} - {hours}h ({DateTime.Now:HH:mm})",
                    User = new User { Name = "Mobile Worker", Email = "mobile.worker@test.com" },
                    Team = selectedCase.Team ?? new Team { Name = "Unknown Team" }
                };

                // Save to database (will use demo mode if DB unavailable)
                await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistration);
                
                await DisplayAlert("Success", 
                    $"✅ Tilføjet {hours} timer til {selectedCase.CaseNumber}!\n\n" +
                    $"Check Timeoversigt på web for at se indførslen.", 
                    "OK");
                    
                Console.WriteLine($"✅ Mobile time registration created: {hours}h for case {selectedCase.CaseNumber} - should appear in Timeoversigt");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Kunne ikke tilføje timer: {ex.Message}", "OK");
            Console.WriteLine($"❌ Failed to create mobile time registration: {ex.Message}");
        }
    }
}
