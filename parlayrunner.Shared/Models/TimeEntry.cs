namespace parlayrunner.Shared.Models;

public class TimeEntry
{
    public int Id { get; set; }
    public Employee Employee { get; set; } = new();
    public DateTime Date { get; set; }
    public TimeSpan CheckIn { get; set; }
    public TimeSpan CheckOut { get; set; }
    public TimeSpan TotalHours { get; set; }
    public TimeSpan TimeBank { get; set; }
    public TimeEntryStatus Status { get; set; }
    public Employee? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string Description { get; set; } = string.Empty;
}

public enum TimeEntryStatus
{
    Afventer,    // Pending
    Godkendt,    // Approved  
    Afvist       // Rejected
} 