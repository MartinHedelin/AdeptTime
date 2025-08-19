namespace parlayrunner.Shared.Services;

public class UserRoleService
{
    public event Action<bool>? OnRoleChanged;
    
    private bool _isAdministrator = false;
    private string _currentUserEmail = "";

    public bool IsAdministrator => _isAdministrator;
    public string CurrentUserEmail => _currentUserEmail;

    public void SetCurrentUser(string email)
    {
        _currentUserEmail = email;
        // Use "admin" instead of "administrator" and ensure case-insensitive comparison
        _isAdministrator = !string.IsNullOrEmpty(email) && 
                          email.ToLowerInvariant().Contains("admin");
        
        // Debug logging
        Console.WriteLine($"[UserRoleService] Setting user: {email}");
        Console.WriteLine($"[UserRoleService] Email ToLowerInvariant: {email.ToLowerInvariant()}");
        Console.WriteLine($"[UserRoleService] Contains 'admin': {email.ToLowerInvariant().Contains("admin")}");
        Console.WriteLine($"[UserRoleService] Is Administrator: {_isAdministrator}");
        Console.WriteLine($"[UserRoleService] OnRoleChanged has listeners: {OnRoleChanged != null}");
        
        OnRoleChanged?.Invoke(_isAdministrator);
    }

    public void Logout()
    {
        _currentUserEmail = "";
        _isAdministrator = false;
        OnRoleChanged?.Invoke(_isAdministrator);
    }
}
