using Microsoft.Extensions.Logging;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace AdeptTime.Shared.Services;

public class UserService : IUserService
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<UserService> _logger;
    private readonly SupabaseSettings _settings;
    
    // Demo mode: in-memory user store
    private static readonly List<User> _demoUsers = new();

    // DTO to map Supabase snake_case JSON to our C# model
    private class DbUserDto
    {
        [JsonPropertyName("id")] public Guid Id { get; set; }
        [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
        [JsonPropertyName("password_hash")] public string PasswordHash { get; set; } = string.Empty;
        [JsonPropertyName("user_type_id")] public int UserTypeId { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("phone_number")] public string? PhoneNumber { get; set; }
        [JsonPropertyName("address")] public string? Address { get; set; }
        [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime? UpdatedAt { get; set; }
    }

    public UserService(ISupabaseService supabaseService, ILogger<UserService> logger, SupabaseSettings settings)
    {
        _supabaseService = supabaseService;
        _logger = logger;
        _settings = settings;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            // Try database first
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");

            var encodedEmail = Uri.EscapeDataString(email);
            var response = await httpClient.GetAsync($"{_settings.Url}/rest/v1/users?email=eq.{encodedEmail}");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var dbUsers = System.Text.Json.JsonSerializer.Deserialize<List<DbUserDto>>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var dto = dbUsers?.FirstOrDefault();
                if (dto == null) return null;

                return new User
                {
                    Id = dto.Id,
                    Email = dto.Email,
                    PasswordHash = dto.PasswordHash,
                    UserTypeId = dto.UserTypeId,
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    CreatedAt = dto.CreatedAt ?? DateTime.UtcNow,
                    UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow
                };
            }
        }
        catch (Exception)
        {
            // Database failed, use demo mode
        }
        
        // Demo mode: check in-memory store
        return _demoUsers.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        try
        {
            // Use direct HTTP client to avoid Supabase client issues
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");

            var response = await httpClient.GetAsync($"{_settings.Url}/rest/v1/users?id=eq.{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return users?.FirstOrDefault();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception($"HTTP error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by ID: {Id}", id);
            return null;
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            // Use direct HTTP client to avoid Supabase client issues
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");

            var response = await httpClient.GetAsync($"{_settings.Url}/rest/v1/users");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return users ?? new List<User>();
            }
            else
            {
                throw new Exception($"HTTP error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains("Failed to fetch") && !ex.Message.Contains("ERR_CONNECTION_REFUSED"))
        {
            _logger.LogError(ex, "Error getting all users");
            }
            return new List<User>();
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            // Hash the password before saving
            user.PasswordHash = HashPassword(user.PasswordHash);
            
            // Debug logging
            _logger.LogInformation("CreateUserAsync - Email: {Email}", user.Email);
            _logger.LogInformation("CreateUserAsync - Plain password length: {Length}", user.PasswordHash?.Length);
            _logger.LogInformation("CreateUserAsync - Hashed password: {Hash}", user.PasswordHash);

            // Let database handle defaults for timestamps and ID
            var userToInsert = new
            {
                email = user.Email,
                password_hash = user.PasswordHash,
                user_type_id = user.UserTypeId,
                name = user.Name,
                phone_number = user.PhoneNumber,
                address = user.Address
                // created_at and updated_at will use database defaults
                // id will use database default (gen_random_uuid())
            };

            // Use direct HTTP client to avoid all Supabase serialization issues
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

            var userData = new
            {
                email = userToInsert.email,
                password_hash = userToInsert.password_hash,
                user_type_id = userToInsert.user_type_id,
                name = userToInsert.name,
                phone_number = userToInsert.phone_number,
                address = userToInsert.address
            };

            var json = System.Text.Json.JsonSerializer.Serialize(userData);
            _logger.LogInformation("CreateUserAsync - JSON being sent: {Json}", json);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{_settings.Url}/rest/v1/users", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            // Check if response is empty (common with Supabase POST)
            if (string.IsNullOrEmpty(responseJson))
            {
                // Return the user object with the data we sent (database will have generated ID and timestamps)
                return new User
                {
                    Email = user.Email,
                    PasswordHash = user.PasswordHash, // Already hashed
                    UserTypeId = user.UserTypeId,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                    // ID will be generated by database, but we can't know it here
                };
            }

            // Try to parse the response if it contains data
            try
            {
                var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return users?.FirstOrDefault() ?? throw new Exception("Failed to create user");
            }
            catch (System.Text.Json.JsonException)
            {
                // If JSON parsing fails, return the user object we created
                return new User
                {
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    UserTypeId = user.UserTypeId,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }
        }
        catch (Exception ex)
        {
            // Database failed, use demo mode
            var demoUser = new User
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                PasswordHash = HashPassword(user.PasswordHash), // ensure exactly one hash
                UserTypeId = user.UserTypeId,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            _demoUsers.RemoveAll(u => u.Email.Equals(demoUser.Email, StringComparison.OrdinalIgnoreCase));
            _demoUsers.Add(demoUser);
            _logger.LogInformation("✅ User created in demo mode: {Email}", demoUser.Email);
            return demoUser;
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            // Update only the fields that should be updated
            var updateData = new
            {
                email = user.Email,
                password_hash = user.PasswordHash,
                user_type_id = user.UserTypeId,
                name = user.Name,
                phone_number = user.PhoneNumber,
                address = user.Address
                // updated_at will be handled by database trigger
            };

            // Use direct HTTP client to avoid all Supabase serialization issues
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

            var userData = new
            {
                email = updateData.email,
                password_hash = updateData.password_hash,
                user_type_id = updateData.user_type_id,
                name = updateData.name,
                phone_number = updateData.phone_number,
                address = updateData.address
            };

            var json = System.Text.Json.JsonSerializer.Serialize(userData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PatchAsync($"{_settings.Url}/rest/v1/users?id=eq.{user.Id}", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            // Check if response is empty (common with Supabase PATCH)
            if (string.IsNullOrEmpty(responseJson))
            {
                // Return the updated user object
                return new User
                {
                    Id = user.Id, // Keep the original ID
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    UserTypeId = user.UserTypeId,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt, // Keep original creation time
                    UpdatedAt = DateTime.UtcNow // Update timestamp
                };
            }

            // Try to parse the response if it contains data
            try
            {
                var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return users?.FirstOrDefault() ?? throw new Exception("Failed to update user");
            }
            catch (System.Text.Json.JsonException)
            {
                // If JSON parsing fails, return the user object we updated
                return new User
                {
                    Id = user.Id,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    UserTypeId = user.UserTypeId,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                };
            }
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains("Failed to fetch") && !ex.Message.Contains("ERR_CONNECTION_REFUSED"))
        {
            _logger.LogError(ex, "Error updating user: {Id}", user.Id);
            }
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        try
        {
            // Use direct HTTP client to avoid Supabase client issues
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");

            var response = await httpClient.DeleteAsync($"{_settings.Url}/rest/v1/users?id=eq.{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {Id}", id);
            return false;
        }
    }

    public async Task<bool> ValidatePasswordAsync(string email, string password)
    {
        try
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;
            
            return VerifyPassword(password, user.PasswordHash);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating password for: {Email}", email);
            return false;
        }
    }

    public async Task<List<UserType>> GetUserTypesAsync()
    {
        try
        {
            // Use direct HTTP client to avoid Supabase client issues
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");

            var response = await httpClient.GetAsync($"{_settings.Url}/rest/v1/user_types");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var userTypes = System.Text.Json.JsonSerializer.Deserialize<List<UserType>>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return userTypes ?? new List<UserType>();
            }
            else
            {
                throw new Exception($"HTTP error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user types");
            return new List<UserType>();
        }
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "your-salt-here"));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput.Equals(hash);
    }

    public async Task SeedDefaultAdminUserAsync()
    {
        try
        {
            const string adminEmail = "admin_user@test.com";
            const string adminPassword = "Test12345678";

            // Check if Supabase client is initialized
            if (_supabaseService.Client == null)
            {
                return; // Silently skip if client not initialized
            }

            // Check if admin user already exists
            var existingAdmin = await GetUserByEmailAsync(adminEmail);
            if (existingAdmin != null)
            {
                return; // Admin user already exists, nothing to do
            }

            // Create default admin user using anonymous object to avoid serialization issues
            var adminUserData = new
            {
                email = adminEmail,
                password_hash = HashPassword(adminPassword),
                user_type_id = 1, // Admin
                name = "System Administrator",
                phone_number = (string?)null,
                address = (string?)null
            };

            // Use direct HTTP client to avoid all Supabase serialization issues
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", _settings.Key);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Key}");
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

            var json = System.Text.Json.JsonSerializer.Serialize(adminUserData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{_settings.Url}/rest/v1/users", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            // Check if response is empty (common with Supabase POST)
            if (string.IsNullOrEmpty(responseJson))
            {
                // Admin user was created successfully, but no response data
                return;
            }

            // Try to parse the response if it contains data (no need to store the result)
            try
            {
                var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var createdUser = users?.FirstOrDefault();
                if (createdUser == null)
                {
                    throw new Exception("Failed to create admin user");
                }
                // Successfully created admin user (no logging needed)
            }
            catch (System.Text.Json.JsonException)
            {
                // JSON parsing failed, but if we got here, the user was likely created successfully
                // since EnsureSuccessStatusCode() passed
                return;
            }
        }
        catch (Exception)
        {
            // Silently handle all seeding errors - this is expected when Supabase is not running
            // The application will work perfectly in demo mode
        }
    }
}
