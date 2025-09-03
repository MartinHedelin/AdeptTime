using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;

namespace AdeptTime.Shared.Services
{
    public class SaegsService : ISaegsService
    {
        private List<Case> _cases = new();
        private List<Customer> _customers = new();
        private List<Employee> _employees = new();
        private bool _isInitialized = false;

        public event Action? OnCasesChanged;

        public SaegsService()
        {
            InitializeData();
        }

        public async Task<List<Case>> GetCasesAsync()
        {
            await EnsureInitialized();
            return _cases.ToList(); // Return a copy to prevent external modification
        }

        public async Task<Case?> GetCaseByIdAsync(int caseId)
        {
            await EnsureInitialized();
            return _cases.FirstOrDefault(c => c.Id == caseId);
        }

        public async Task<Case> AddCaseAsync(Case newCase)
        {
            await EnsureInitialized();
            
            // Ensure the case has a unique ID
            if (newCase.Id <= 0)
            {
                newCase.Id = _cases.Any() ? _cases.Max(c => c.Id) + 1 : 1;
            }

            // Generate case number if not provided
            if (string.IsNullOrEmpty(newCase.CaseNumber))
            {
                newCase.CaseNumber = $"SAG-{DateTime.Now:yyyy}-{newCase.Id:0000}";
            }

            _cases.Insert(0, newCase); // Add to the beginning of the list
            
            // Notify listeners that cases have changed
            OnCasesChanged?.Invoke();
            
            return newCase;
        }

        public async Task<Case> UpdateCaseAsync(Case updatedCase)
        {
            await EnsureInitialized();
            
            var existingCase = _cases.FirstOrDefault(c => c.Id == updatedCase.Id);
            if (existingCase != null)
            {
                var index = _cases.IndexOf(existingCase);
                _cases[index] = updatedCase;
                
                OnCasesChanged?.Invoke();
            }
            
            return updatedCase;
        }

        public async Task<bool> DeleteCaseAsync(int caseId)
        {
            await EnsureInitialized();
            
            var caseToRemove = _cases.FirstOrDefault(c => c.Id == caseId);
            if (caseToRemove != null)
            {
                _cases.Remove(caseToRemove);
                OnCasesChanged?.Invoke();
                return true;
            }
            
            return false;
        }

        public async Task<List<Case>> GetFilteredCasesAsync(string? statusFilter = null, string? searchQuery = null)
        {
            await EnsureInitialized();
            
            var filtered = _cases.AsEnumerable();

            // Filter by status
            if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse<CaseStatus>(statusFilter, out var status))
            {
                filtered = filtered.Where(c => c.Status == status);
            }

            // Filter by search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filtered = filtered.Where(c => 
                    c.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    c.CaseNumber.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    c.Customer?.Name?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) == true ||
                    c.AssignedEmployee?.Name?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) == true ||
                    c.Department?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) == true);
            }

            return filtered.ToList();
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            await EnsureInitialized();
            return _customers.ToList();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            await EnsureInitialized();
            return _employees.ToList();
        }

        private async Task EnsureInitialized()
        {
            if (!_isInitialized)
            {
                await Task.Delay(1); // Simulate async initialization
                _isInitialized = true;
            }
        }

        private void InitializeData()
        {
            InitializeCustomers();
            InitializeEmployees();
            InitializeCases();
            _isInitialized = true;
        }

        private void InitializeCustomers()
        {
            _customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "VVS Hansen ApS", ContactPerson = "Lars Hansen", Email = "lars@vvshansen.dk", Phone = "+45 12 34 56 78" },
                new Customer { Id = 2, Name = "Elektro Nielsen A/S", ContactPerson = "Peter Nielsen", Email = "peter@elektronielsen.dk", Phone = "+45 87 65 43 21" },
                new Customer { Id = 3, Name = "Byg & Co", ContactPerson = "Mette Andersen", Email = "mette@bygco.dk", Phone = "+45 23 45 67 89" }
            };
        }

        private void InitializeEmployees()
        {
            _employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "James Carter", Team = "Team London", AvatarUrl = "https://plus.unsplash.com/premium_photo-1671656349322-41de944d259b?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8cG9ydHJhaXR8ZW58MHx8MHx8fDA%3D" },
                new Employee { Id = 2, Name = "Liam Anders", Team = "Team London", AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8cG9ydHJhaXR8ZW58MHx8MHx8fDA%3D" },
                new Employee { Id = 3, Name = "Noah Bennett", Team = "Team London", AvatarUrl = "https://images.unsplash.com/photo-1531746020798-e6953c6e8e04?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Nnx8cG9ydHJhaXR8ZW58MHx8MHx8fDA%3D" }
            };
        }

        private void InitializeCases()
        {
            _cases = new List<Case>
            {
                new Case
                {
                    Id = 1,
                    CaseNumber = "SAG-2025-0012",
                    Description = "Renovering af badeværelse - montering af fliser og VVS-arbejde",
                    Comment = "Materialer bestilt - forventet levering fredag.",
                    Customer = _customers[0],
                    AssignedEmployee = _employees[0],
                    Department = "VVS",
                    CompletedHours = 139,
                    TotalHours = 300,
                    StartDate = new DateTime(2022, 1, 22),
                    EndDate = new DateTime(2022, 1, 22),
                    Status = CaseStatus.Badges,
                    AttachmentCount = 2
                },
                new Case
                {
                    Id = 2,
                    CaseNumber = "SAG-2025-0013",
                    Description = "Elektrisk installation i nyt køkken",
                    Comment = "Kunde ønsker LED spotlights.",
                    Customer = _customers[1],
                    AssignedEmployee = _employees[1],
                    Department = "El",
                    CompletedHours = 84,
                    TotalHours = 84,
                    StartDate = new DateTime(2022, 1, 22),
                    EndDate = new DateTime(2022, 1, 22),
                    Status = CaseStatus.Pending,
                    AttachmentCount = 4
                },
                new Case
                {
                    Id = 3,
                    CaseNumber = "SAG-2025-0014",
                    Description = "Reparation af varmepumpe",
                    Comment = "Defekt kompressor - reservedel bestilt.",
                    Customer = _customers[2],
                    AssignedEmployee = _employees[2],
                    Department = "VVS",
                    CompletedHours = 38,
                    TotalHours = 38,
                    StartDate = new DateTime(2022, 1, 22),
                    EndDate = new DateTime(2022, 1, 22),
                    Status = CaseStatus.Review,
                    AttachmentCount = 2
                },
                new Case
                {
                    Id = 4,
                    CaseNumber = "SAG-2025-0015",
                    Description = "Montering af nyt vinduer",
                    Comment = "Specielle mål - måles op tirsdag.",
                    Customer = _customers[0],
                    AssignedEmployee = _employees[0],
                    Department = "Tømrer",
                    CompletedHours = 84,
                    TotalHours = 84,
                    StartDate = new DateTime(2022, 1, 22),
                    EndDate = new DateTime(2022, 1, 22),
                    Status = CaseStatus.Badges,
                    AttachmentCount = 4
                },
                new Case
                {
                    Id = 5,
                    CaseNumber = "SAG-2025-0016",
                    Description = "Service af centralvarme anlæg",
                    Comment = "Årlig service - alt fungerer optimalt.",
                    Customer = _customers[1],
                    AssignedEmployee = _employees[1],
                    Department = "VVS",
                    CompletedHours = 38,
                    TotalHours = 38,
                    StartDate = new DateTime(2022, 1, 22),
                    EndDate = new DateTime(2022, 1, 22),
                    Status = CaseStatus.Badges,
                    AttachmentCount = 2
                }
            };
        }
    }
} 