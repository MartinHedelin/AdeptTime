using Microsoft.JSInterop;
using parlayrunner.Shared.Interfaces;

namespace parlayrunner.Shared.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string AUTH_COOKIE_NAME = "authToken";
        private const string USER_EMAIL_COOKIE_NAME = "userEmail";
        
        public event Action? OnAuthenticationStateChanged;

        public AuthenticationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                // For now, accept any email/password combination
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
                {
                    // Generate a simple token (in real app, this would come from server)
                    var token = Guid.NewGuid().ToString();
                    
                    // Set cookies that expire in 7 days
                    await SetCookieAsync(AUTH_COOKIE_NAME, token, 7);
                    await SetCookieAsync(USER_EMAIL_COOKIE_NAME, email, 7);
                    
                    // Notify components that authentication state changed
                    OnAuthenticationStateChanged?.Invoke();
                    
                    return true;
                }
                
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                // Clear authentication cookies
                await DeleteCookieAsync(AUTH_COOKIE_NAME);
                await DeleteCookieAsync(USER_EMAIL_COOKIE_NAME);
                
                // Notify components that authentication state changed
                OnAuthenticationStateChanged?.Invoke();
            }
            catch (Exception)
            {
                // Handle silently
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var token = await GetCookieAsync(AUTH_COOKIE_NAME);
                return !string.IsNullOrWhiteSpace(token);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string?> GetCurrentUserEmailAsync()
        {
            try
            {
                return await GetCookieAsync(USER_EMAIL_COOKIE_NAME);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task SetCookieAsync(string name, string value, int expireDays)
        {
            var expires = DateTime.Now.AddDays(expireDays).ToString("ddd, dd MMM yyyy HH:mm:ss GMT");
            await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = '{name}={value}; expires={expires}; path=/; SameSite=Strict'");
        }

        private async Task<string?> GetCookieAsync(string name)
        {
            var cookies = await _jsRuntime.InvokeAsync<string>("eval", "document.cookie");
            
            if (string.IsNullOrWhiteSpace(cookies))
                return null;

            var cookieArray = cookies.Split(';');
            foreach (var cookie in cookieArray)
            {
                var keyValue = cookie.Trim().Split('=', 2);
                if (keyValue.Length == 2 && keyValue[0] == name)
                {
                    return keyValue[1];
                }
            }
            
            return null;
        }

        private async Task DeleteCookieAsync(string name)
        {
            await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = '{name}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; SameSite=Strict'");
        }
    }
} 