using AdeptTime.Shared.Models;
using AdeptTime.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AdeptTime.Shared.Services;

public interface ITimeRegistrationService
{
    Task<List<TimeRegistration>> GetAllTimeRegistrationsAsync();
    Task<List<TimeRegistration>> GetTimeRegistrationsByTeamAsync(Guid? teamId);
    Task<List<TimeRegistration>> GetTimeRegistrationsByUserAsync(Guid userId);
    Task<List<TimeRegistration>> GetTimeRegistrationsByDateRangeAsync(DateTime fromDate, DateTime toDate);
    Task<TimeRegistration> CreateTimeRegistrationAsync(TimeRegistration registration);
    Task<TimeRegistration> UpdateTimeRegistrationAsync(TimeRegistration registration);
}

public class TimeRegistrationService : ITimeRegistrationService
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<TimeRegistrationService> _logger;
    private readonly IUserService _userService;
    private readonly ITeamService _teamService;

    // Demo mode: in-memory registration store
    private static readonly List<TimeRegistration> _demoRegistrations = new();

    public TimeRegistrationService(
        ISupabaseService supabaseService, 
        ILogger<TimeRegistrationService> logger,
        IUserService userService,
        ITeamService teamService)
    {
        _supabaseService = supabaseService;
        _logger = logger;
        _userService = userService;
        _teamService = teamService;

        // Initialize demo data if empty
        if (!_demoRegistrations.Any())
        {
            InitializeDemoRegistrations();
        }
    }

    private void InitializeDemoRegistrations()
    {
        // Create demo registrations with fake GUIDs
        var adminUserId = Guid.NewGuid();
        var workerUserId = Guid.NewGuid();
        var johnUserId = Guid.NewGuid();
        var londonTeamId = Guid.NewGuid();
        var dublinTeamId = Guid.NewGuid();
        var copenhagenTeamId = Guid.NewGuid();

        _demoRegistrations.AddRange(new List<TimeRegistration>
        {
            // Admin user entries
            new TimeRegistration
            {
                Id = Guid.NewGuid(),
                UserId = adminUserId,
                TeamId = londonTeamId,
                Date = new DateTime(2024, 1, 22),
                CheckIn = new TimeSpan(8, 0, 0),
                CheckOut = new TimeSpan(17, 15, 0),
                TotalHours = 9.25m,
                TimeBank = 1.25m,
                Status = "Afventer",
                Description = "Regular work day - project management"
            },
            new TimeRegistration
            {
                Id = Guid.NewGuid(),
                UserId = adminUserId,
                TeamId = londonTeamId,
                Date = new DateTime(2024, 1, 23),
                CheckIn = new TimeSpan(8, 0, 0),
                CheckOut = new TimeSpan(17, 0, 0),
                TotalHours = 9.00m,
                TimeBank = 1.00m,
                Status = "Godkendt",
                Description = "Team meetings and code reviews"
            },
            // Worker user entries
            new TimeRegistration
            {
                Id = Guid.NewGuid(),
                UserId = workerUserId,
                TeamId = dublinTeamId,
                Date = new DateTime(2024, 1, 22),
                CheckIn = new TimeSpan(8, 0, 0),
                CheckOut = new TimeSpan(17, 0, 0),
                TotalHours = 9.00m,
                TimeBank = 1.00m,
                Status = "Godkendt",
                Description = "Client support and bug fixes"
            },
            new TimeRegistration
            {
                Id = Guid.NewGuid(),
                UserId = workerUserId,
                TeamId = dublinTeamId,
                Date = new DateTime(2024, 1, 23),
                CheckIn = new TimeSpan(8, 15, 0),
                CheckOut = new TimeSpan(17, 15, 0),
                TotalHours = 9.00m,
                TimeBank = 1.00m,
                Status = "Afventer",
                Description = "Feature development"
            },
            // John user entries
            new TimeRegistration
            {
                Id = Guid.NewGuid(),
                UserId = johnUserId,
                TeamId = copenhagenTeamId,
                Date = new DateTime(2024, 1, 22),
                CheckIn = new TimeSpan(9, 0, 0),
                CheckOut = new TimeSpan(18, 0, 0),
                TotalHours = 9.00m,
                TimeBank = 1.00m,
                Status = "Afventer",
                Description = "Database optimization work"
            },
            new TimeRegistration
            {
                Id = Guid.NewGuid(),
                UserId = johnUserId,
                TeamId = copenhagenTeamId,
                Date = new DateTime(2024, 1, 24),
                CheckIn = new TimeSpan(8, 0, 0),
                CheckOut = new TimeSpan(17, 45, 0),
                TotalHours = 9.75m,
                TimeBank = 1.75m,
                Status = "Afventer",
                Description = "Overtime for project deadline"
            }
        });
    }

    public async Task<List<TimeRegistration>> GetAllTimeRegistrationsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all time registrations from Supabase");

            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<TimeRegistration>().Get();

            if (response?.Models != null && response.Models.Any())
            {
                var registrations = response.Models.ToList();
                
                // Populate navigation properties
                await PopulateNavigationProperties(registrations);
                
                _logger.LogInformation($"✅ Loaded {registrations.Count} time registrations from Supabase");
                return registrations;
            }
            else
            {
                _logger.LogWarning("No time registrations found in Supabase, using demo data");
                return _demoRegistrations.ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch time registrations from Supabase, using demo data");
            return _demoRegistrations.ToList();
        }
    }

    public async Task<List<TimeRegistration>> GetTimeRegistrationsByTeamAsync(Guid? teamId)
    {
        var allRegistrations = await GetAllTimeRegistrationsAsync();
        
        if (teamId == null)
        {
            return allRegistrations;
        }

        return allRegistrations.Where(r => r.TeamId == teamId).ToList();
    }

    public async Task<List<TimeRegistration>> GetTimeRegistrationsByUserAsync(Guid userId)
    {
        var allRegistrations = await GetAllTimeRegistrationsAsync();
        return allRegistrations.Where(r => r.UserId == userId).ToList();
    }

    public async Task<List<TimeRegistration>> GetTimeRegistrationsByDateRangeAsync(DateTime fromDate, DateTime toDate)
    {
        var allRegistrations = await GetAllTimeRegistrationsAsync();
        return allRegistrations.Where(r => r.Date >= fromDate && r.Date <= toDate).ToList();
    }

    public async Task<TimeRegistration> CreateTimeRegistrationAsync(TimeRegistration registration)
    {
        try
        {
            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<TimeRegistration>().Insert(registration);

            if (response?.Models?.FirstOrDefault() != null)
            {
                var created = response.Models.First();
                await PopulateNavigationProperties(new List<TimeRegistration> { created });
                
                _logger.LogInformation($"✅ Time registration created in Supabase for user: {registration.UserId}");
                return created;
            }
            else
            {
                throw new Exception("Failed to create time registration in Supabase");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create time registration in Supabase, adding to demo data");
            
            // Fallback to demo mode
            registration.Id = Guid.NewGuid();
            registration.CreatedAt = DateTime.UtcNow;
            registration.UpdatedAt = DateTime.UtcNow;
            
            _demoRegistrations.Add(registration);
            _logger.LogInformation($"✅ Time registration created in demo mode for user: {registration.UserId}");
            return registration;
        }
    }

    public async Task<TimeRegistration> UpdateTimeRegistrationAsync(TimeRegistration registration)
    {
        try
        {
            registration.UpdatedAt = DateTime.UtcNow;
            
            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<TimeRegistration>().Update(registration);

            if (response?.Models?.FirstOrDefault() != null)
            {
                var updated = response.Models.First();
                await PopulateNavigationProperties(new List<TimeRegistration> { updated });
                
                _logger.LogInformation($"✅ Time registration updated in Supabase: {registration.Id}");
                return updated;
            }
            else
            {
                throw new Exception("Failed to update time registration in Supabase");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update time registration in Supabase, updating demo data");
            
            // Fallback to demo mode
            var existing = _demoRegistrations.FirstOrDefault(r => r.Id == registration.Id);
            if (existing != null)
            {
                var index = _demoRegistrations.IndexOf(existing);
                _demoRegistrations[index] = registration;
            }
            
            _logger.LogInformation($"✅ Time registration updated in demo mode: {registration.Id}");
            return registration;
        }
    }

    private async Task PopulateNavigationProperties(List<TimeRegistration> registrations)
    {
        try
        {
            // Get all unique user and team IDs
            var userIds = registrations.Select(r => r.UserId).Distinct().ToList();
            var teamIds = registrations.Where(r => r.TeamId.HasValue).Select(r => r.TeamId!.Value).Distinct().ToList();
            var approverIds = registrations.Where(r => r.ApprovedBy.HasValue).Select(r => r.ApprovedBy!.Value).Distinct().ToList();

            // Fetch users and teams (this will use demo data if DB is unavailable)
            var users = new List<User>();
            var teams = await _teamService.GetAllTeamsAsync();

            // For users, we need to handle the case where UserService might not have a GetUsersByIds method
            // So we'll fetch them individually for now
            foreach (var userId in userIds.Union(approverIds).Distinct())
            {
                try
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        users.Add(user);
                        _logger.LogInformation($"[TimeRegistrationService] Loaded user: {user.Name} ({user.Email}) - UserType: {user.UserTypeId}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Failed to load user with ID: {userId}");
                }
            }

            // Populate navigation properties
            foreach (var registration in registrations)
            {
                registration.User = users.FirstOrDefault(u => u.Id == registration.UserId);
                registration.Team = teams.FirstOrDefault(t => t.Id == registration.TeamId);
                registration.ApprovedByUser = users.FirstOrDefault(u => u.Id == registration.ApprovedBy);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to populate navigation properties for time registrations");
        }
    }
}
