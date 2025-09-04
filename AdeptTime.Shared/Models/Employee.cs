namespace AdeptTime.Shared.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string UserType { get; set; } = "Worker";
} 