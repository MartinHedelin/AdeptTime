namespace parlayrunner.Shared.Models;

public class ScheduleEntry
{
    public int Id { get; set; }
    public Employee Employee { get; set; } = new();
    public Customer Customer { get; set; } = new();
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ScheduleStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public enum ScheduleStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled
}

public class EmployeeGroup
{
    public string Name { get; set; } = string.Empty;
    public List<Employee> Employees { get; set; } = new();
    public bool IsExpanded { get; set; } = true;
} 