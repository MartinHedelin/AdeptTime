namespace parlayrunner.Shared.Models;

public class GeofenceRule
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Original properties for compatibility
    public string WorkingHours { get; set; } = string.Empty;
    public int Radius { get; set; }
    public DateTime CreatedDate { get; set; }
    
    // New properties for the creation form
    public int RadiusKm { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}