namespace AdeptTime.Shared.Models;

public class Case
{
    public int Id { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public Customer Customer { get; set; } = new();
    public Employee AssignedEmployee { get; set; } = new();
    public Employee Manager { get; set; } = new();
    public string Department { get; set; } = string.Empty;
    public int CompletedHours { get; set; }
    public int TotalHours { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public CaseStatus Status { get; set; }
    public int AttachmentCount { get; set; }
}

public enum CaseStatus
{
    Badges,      // Active/In Progress (Green)
    Pending,     // Pending (Orange) 
    Review,      // Under Review (Blue)
    Completed    // Completed (Gray)
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
} 