namespace parlayrunner.Shared.Models;

public class GeofenceRule
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WorkingHours { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Radius { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } = true;
} 