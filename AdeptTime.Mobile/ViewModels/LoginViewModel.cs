using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Services;

namespace AdeptTime.Mobile.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;
    private readonly UserRoleService _userRoleService;

    private string _email = "";
    private string _password = "";
    private string _errorMessage = "";
    private bool _isLoading = false;

    public LoginViewModel(IUserService userService, IAuthenticationService authService, UserRoleService userRoleService)
    {
        _userService = userService;
        _authService = authService;
        _userRoleService = userRoleService;
        
        LoginCommand = new Command(async () => await LoginAsync(), () => CanLogin);
        
        // Auto-fill for testing
#if DEBUG
        Email = "worker@test.com";
        Password = "Test12345678";
#endif
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasError));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNotLoading));
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool IsNotLoading => !IsLoading;
    public bool CanLogin => !IsLoading && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);

    public ICommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = "";

            // Validate user exists
            var user = await _userService.GetUserByEmailAsync(Email);
            if (user == null)
            {
                ErrorMessage = "User not found with that email address";
                return;
            }

            // Check if user is administrator
            if (user.IsAdmin)
            {
                ErrorMessage = "Administrators cannot access the mobile app. Please use the web application.";
                return;
            }

            // Validate password
            bool isValid = await _userService.ValidatePasswordAsync(Email, Password);
            if (!isValid)
            {
                ErrorMessage = "Invalid password";
                return;
            }

            // Set user role and authenticate
            _userRoleService.SetCurrentUser(Email, user.IsAdmin);
            await _authService.LoginAsync(Email, Password);

            // Navigate to main app
            await Shell.Current.GoToAsync("//main");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Value converter for loading text
public class LoadingTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return (bool)value ? "Signing in..." : "Sign In";
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
