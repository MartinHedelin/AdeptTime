using AdeptTime.Shared.Models;
using AdeptTime.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdeptTime.Shared.Services;

public interface ICaseService
{
    Task<List<CaseModel>> GetAllCasesAsync();
    Task<List<CaseModel>> GetCasesByTeamAsync(Guid? teamId);
    Task<CaseModel> CreateCaseAsync(CaseModel caseModel);
    Task<CaseModel> UpdateCaseAsync(CaseModel caseModel);
    Task<CaseModel?> GetCaseByIdAsync(Guid caseId);
}

public class CaseService : ICaseService
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<CaseService> _logger;
    private readonly IUserService _userService;
    private readonly ITeamService _teamService;

    // Demo mode: in-memory case store
    private static readonly List<CaseModel> _demoCases = new();

    public CaseService(
        ISupabaseService supabaseService, 
        ILogger<CaseService> logger,
        IUserService userService,
        ITeamService teamService)
    {
        _supabaseService = supabaseService;
        _logger = logger;
        _userService = userService;
        _teamService = teamService;

        // Initialize demo cases if empty
        if (!_demoCases.Any())
        {
            InitializeDemoCases();
        }
    }

    private void InitializeDemoCases()
    {
        var demoTeamId = Guid.NewGuid();
        var demoUserId = Guid.NewGuid();

        _demoCases.AddRange(new List<CaseModel>
        {
            new CaseModel
            {
                Id = Guid.NewGuid(),
                CaseNumber = "SAG-2024-0001",
                Title = "VVS Installation",
                Description = "Installation af nyt VVS system i køkken og bad",
                TeamId = demoTeamId,
                CreatedBy = demoUserId,
                Status = "I gang",
                Priority = "Høj",
                StartDate = new DateTime(2024, 1, 22),
                EndDate = new DateTime(2024, 1, 25),
                EstimatedHours = 32,
                CompletedHours = 16,
                GeofenceAddress = "Vesterbro 36, 2300 København S",
                GeofenceRadius = 150
            },
            new CaseModel
            {
                Id = Guid.NewGuid(),
                CaseNumber = "SAG-2024-0002",
                Title = "Elektrisk Vedligeholdelse",
                Description = "Rutine tjek af elektriske installationer",
                TeamId = demoTeamId,
                CreatedBy = demoUserId,
                Status = "Ny",
                Priority = "Medium",
                StartDate = new DateTime(2024, 1, 25),
                EndDate = new DateTime(2024, 1, 27),
                EstimatedHours = 16,
                CompletedHours = 0,
                GeofenceAddress = "Nørrebro 12, 2200 København N",
                GeofenceRadius = 100
            }
        });
    }

    public async Task<List<CaseModel>> GetAllCasesAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all cases from Supabase");

            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<CaseModel>().Get();

            if (response?.Models != null && response.Models.Any())
            {
                var dbCases = response.Models.ToList();
                await PopulateNavigationProperties(dbCases);
                
                // Combine database cases with demo cases
                var allCases = dbCases.Concat(_demoCases).ToList();
                
                _logger.LogInformation($"✅ Loaded {dbCases.Count} cases from Supabase + {_demoCases.Count} demo cases = {allCases.Count} total");
                return allCases;
            }
            else
            {
                _logger.LogWarning("No cases found in Supabase, using demo data only");
                return _demoCases.ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch cases from Supabase, using demo data");
            return _demoCases.ToList();
        }
    }

    public async Task<List<CaseModel>> GetCasesByTeamAsync(Guid? teamId)
    {
        var allCases = await GetAllCasesAsync();
        
        if (teamId == null)
        {
            return allCases;
        }

        return allCases.Where(c => c.TeamId == teamId).ToList();
    }

    public async Task<CaseModel?> GetCaseByIdAsync(Guid caseId)
    {
        try
        {
            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<CaseModel>()
                .Where(c => c.Id == caseId)
                .Single();

            if (response != null)
            {
                await PopulateNavigationProperties(new List<CaseModel> { response });
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch case from Supabase, checking demo data");
            return _demoCases.FirstOrDefault(c => c.Id == caseId);
        }
    }

    public async Task<CaseModel> CreateCaseAsync(CaseModel caseModel)
    {
        try
        {
            _logger.LogInformation($"Attempting to create case in Supabase: {caseModel.CaseNumber}");

            // Use simple HTTP POST with only the required fields
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0");

            var caseData = new
            {
                case_number = caseModel.CaseNumber,
                title = caseModel.Title ?? caseModel.CaseNumber,
                description = caseModel.Description ?? "",
                status = caseModel.Status ?? "Ny",
                priority = caseModel.Priority ?? "Medium"
            };

            var json = System.Text.Json.JsonSerializer.Serialize(caseData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            _logger.LogInformation($"Sending case data: {json}");

            var response = await httpClient.PostAsync("http://127.0.0.1:54321/rest/v1/cases", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"✅ Case created in Supabase database: {caseModel.CaseNumber}");
                _logger.LogInformation($"Response: {responseContent}");
                return caseModel;
            }
            else
            {
                _logger.LogError($"❌ Database error {response.StatusCode}: {responseContent}");
                throw new Exception($"Database save failed: {response.StatusCode} - {responseContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"❌ Failed to save case to database: {ex.Message}");
            throw; // Don't fall back to demo mode - we want to see the actual error
        }
    }

    public async Task<CaseModel> UpdateCaseAsync(CaseModel caseModel)
    {
        try
        {
            caseModel.UpdatedAt = DateTime.UtcNow;
            
            var client = await _supabaseService.GetClientAsync();
            var response = await client.From<CaseModel>().Update(caseModel);

            if (response?.Models?.FirstOrDefault() != null)
            {
                var updated = response.Models.First();
                await PopulateNavigationProperties(new List<CaseModel> { updated });
                
                _logger.LogInformation($"✅ Case updated in Supabase: {caseModel.CaseNumber}");
                return updated;
            }
            else
            {
                throw new Exception("Failed to update case in Supabase");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update case in Supabase, updating demo data");
            
            // Fallback to demo mode
            var existing = _demoCases.FirstOrDefault(c => c.Id == caseModel.Id);
            if (existing != null)
            {
                var index = _demoCases.IndexOf(existing);
                _demoCases[index] = caseModel;
            }
            
            _logger.LogInformation($"✅ Case updated in demo mode: {caseModel.CaseNumber}");
            return caseModel;
        }
    }

    private async Task PopulateNavigationProperties(List<CaseModel> cases)
    {
        try
        {
            // Get all unique user and team IDs
            var userIds = cases.Where(c => c.CreatedBy.HasValue).Select(c => c.CreatedBy!.Value)
                .Union(cases.Where(c => c.AssignedTo.HasValue).Select(c => c.AssignedTo!.Value))
                .Distinct().ToList();
            var teamIds = cases.Where(c => c.TeamId.HasValue).Select(c => c.TeamId!.Value).Distinct().ToList();

            // Fetch users and teams
            var users = new List<User>();
            var teams = await _teamService.GetAllTeamsAsync();

            foreach (var userId in userIds)
            {
                try
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }
                catch
                {
                    // Continue if user not found
                }
            }

            // Populate navigation properties
            foreach (var caseModel in cases)
            {
                caseModel.Team = teams.FirstOrDefault(t => t.Id == caseModel.TeamId);
                caseModel.CreatedByUser = users.FirstOrDefault(u => u.Id == caseModel.CreatedBy);
                caseModel.AssignedToUser = users.FirstOrDefault(u => u.Id == caseModel.AssignedTo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to populate navigation properties for cases");
        }
    }
}
