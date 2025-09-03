using Microsoft.Extensions.Logging;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;
using System.Security.Cryptography;
using System.Text;

namespace AdeptTime.Shared.Services;

public class UserService : IUserService
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<UserService> _logger;

    public UserService(ISupabaseService supabaseService, ILogger<UserService> logger)
    {
        _supabaseService = supabaseService;
        _logger = logger;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            var result = await _supabaseService.Client
                .From<User>()
                .Where(u => u.Email == email)
                .Single();
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by email: {Email}", email);
            return null;
        }
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        try
        {
            var result = await _supabaseService.Client
                .From<User>()
                .Where(u => u.Id == id)
                .Single();
            
            return result;
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
            var result = await _supabaseService.Client
                .From<User>()
                .Get();
            
            return result.Models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return new List<User>();
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            // Hash the password before saving
            user.PasswordHash = HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _supabaseService.Client
                .From<User>()
                .Insert(user);
            
            return result.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", user.Email);
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _supabaseService.Client
                .From<User>()
                .Where(u => u.Id == user.Id)
                .Update(user);
            
            return result.Models.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {Id}", user.Id);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        try
        {
            await _supabaseService.Client
                .From<User>()
                .Where(u => u.Id == id)
                .Delete();
            
            return true;
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
            var result = await _supabaseService.Client
                .From<UserType>()
                .Get();
            
            return result.Models;
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
}
