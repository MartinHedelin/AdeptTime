using AdeptTime.Shared.Models;

namespace AdeptTime.Shared.Interfaces
{
    public interface ISaegsService
    {
        // Events
        event Action OnCasesChanged;

        // Case management
        Task<List<Case>> GetCasesAsync();
        Task<Case?> GetCaseByIdAsync(int caseId);
        Task<Case> AddCaseAsync(Case newCase);
        Task<Case> UpdateCaseAsync(Case updatedCase);
        Task<bool> DeleteCaseAsync(int caseId);

        // Filter and search
        Task<List<Case>> GetFilteredCasesAsync(string? statusFilter = null, string? searchQuery = null);

        // Supporting data
        Task<List<Customer>> GetCustomersAsync();
        Task<List<Employee>> GetEmployeesAsync();
    }
} 