namespace AdeptTime.Shared.Services;

public class UserRoleService
{
    public event Action<bool>? OnRoleChanged;
    
    private bool _isAdministrator = false;
    private string _currentUserEmail = "";

    public bool IsAdministrator => _isAdministrator;
    public string CurrentUserEmail => _currentUserEmail;

    public void SetCurrentUser(string email, bool isAdministrator = false)
    {
        _currentUserEmail = email;
        _isAdministrator = isAdministrator;
        
        // Debug logging
        Console.WriteLine($"[UserRoleService] Setting user: {email}");
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
