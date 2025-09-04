using AdeptTime.Shared.Models;
using AdeptTime.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AdeptTime.Shared.Services;

public interface ITeamService
{
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(Guid teamId);
    Task<Team> CreateTeamAsync(Team team);
}

public class TeamService : ITeamService
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<TeamService> _logger;

    // Demo mode: in-memory team store
    private static readonly List<Team> _demoTeams = new();

    public TeamService(ISupabaseService supabaseService, ILogger<TeamService> logger)
    {
        _supabaseService = supabaseService;
        _logger = logger;

        // Initialize demo teams if empty
        if (!_demoTeams.Any())
        {
            InitializeDemoTeams();
        }
    }

    private void InitializeDemoTeams()
    {
        _demoTeams.AddRange(new List<Team>
        {
            new Team { Id = Guid.NewGuid(), Name = "All Teams", Description = "All team members", Color = "#6c757d" },
            new Team { Id = Guid.NewGuid(), Name = "Team London", Description = "London office team", Color = "#007bff" },
            new Team { Id = Guid.NewGuid(), Name = "Team Dublin", Description = "Dublin office team", Color = "#28a745" },
            new Team { Id = Guid.NewGuid(), Name = "Team Copenhagen", Description = "Copenhagen office team", Color = "#dc3545" },
            new Team { Id = Guid.NewGuid(), Name = "Team Berlin", Description = "Berlin office team", Color = "#ffc107" },
            new Team { Id = Guid.NewGuid(), Name = "Team Stockholm", Description = "Stockholm office team", Color = "#17a2b8" }
        });
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all teams from Supabase");

            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<Team>().Get();

            if (response?.Models != null && response.Models.Any())
            {
                _logger.LogInformation($"✅ Loaded {response.Models.Count} teams from Supabase");
                return response.Models.ToList();
            }
            else
            {
                _logger.LogWarning("No teams found in Supabase, using demo data");
                return _demoTeams.ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch teams from Supabase, using demo data");
            return _demoTeams.ToList();
        }
    }

    public async Task<Team?> GetTeamByIdAsync(Guid teamId)
    {
        try
        {
            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<Team>()
                .Where(t => t.Id == teamId)
                .Single();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch team from Supabase, checking demo data");
            return _demoTeams.FirstOrDefault(t => t.Id == teamId);
        }
    }

    public async Task<Team> CreateTeamAsync(Team team)
    {
        try
        {
            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<Team>().Insert(team);

            if (response?.Models?.FirstOrDefault() != null)
            {
                _logger.LogInformation($"✅ Team created in Supabase: {team.Name}");
                return response.Models.First();
            }
            else
            {
                throw new Exception("Failed to create team in Supabase");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create team in Supabase, adding to demo data");
            
            // Fallback to demo mode
            team.Id = Guid.NewGuid();
            team.CreatedAt = DateTime.UtcNow;
            team.UpdatedAt = DateTime.UtcNow;
            
            _demoTeams.Add(team);
            _logger.LogInformation($"✅ Team created in demo mode: {team.Name}");
            return team;
        }
    }
}
