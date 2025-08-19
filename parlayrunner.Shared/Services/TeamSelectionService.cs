namespace parlayrunner.Shared.Services;

public class TeamSelectionService
{
    private string _selectedTeam = "All";
    
    public event Action<string>? OnTeamChanged;

    public string SelectedTeam 
    { 
        get => _selectedTeam;
        private set
        {
            if (_selectedTeam != value)
            {
                _selectedTeam = value;
                OnTeamChanged?.Invoke(value);
            }
        }
    }

    public void SetSelectedTeam(string team)
    {
        SelectedTeam = team;
    }

    public bool IsTeamSelected(string team)
    {
        return _selectedTeam == team;
    }

    public bool IsAllTeamsSelected()
    {
        return _selectedTeam == "All";
    }
}
