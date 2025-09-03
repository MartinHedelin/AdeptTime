using AdeptTime.Shared.Models;

namespace AdeptTime.Shared.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> ValidatePasswordAsync(string email, string password);
    Task<List<UserType>> GetUserTypesAsync();
}