namespace AdeptTime.Shared.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetCurrentUserEmailAsync();
        event Action OnAuthenticationStateChanged;
    }
} 