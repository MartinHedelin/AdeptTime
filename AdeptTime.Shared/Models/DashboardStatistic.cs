namespace AdeptTime.Shared.Models;

public class DashboardStatistic
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public string IconColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
    public StatisticType Type { get; set; }
    public string Description { get; set; } = string.Empty;
}

public enum StatisticType
{
    Attendance,
    Active,
    Users,
    Time,
    General
}

public class DashboardUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Workspace { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsWorkingToday { get; set; }
} 