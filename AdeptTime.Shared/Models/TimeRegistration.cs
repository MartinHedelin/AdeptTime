using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace AdeptTime.Shared.Models;

[Table("time_registrations")]
public class TimeRegistration : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("team_id")]
    public Guid? TeamId { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("check_in")]
    public TimeSpan? CheckIn { get; set; }

    [Column("check_out")]
    public TimeSpan? CheckOut { get; set; }

    [Column("total_hours")]
    public decimal TotalHours { get; set; }

    [Column("time_bank")]
    public decimal TimeBank { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Afventer";

    [Column("description")]
    public string? Description { get; set; }

    [Column("approved_by")]
    public Guid? ApprovedBy { get; set; }

    [Column("approved_at")]
    public DateTime? ApprovedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties (not stored in DB, populated via joins or separate queries)
    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public Team? Team { get; set; }

    [JsonIgnore]
    public User? ApprovedByUser { get; set; }

    // Helper properties for UI compatibility
    [JsonIgnore]
    public TimeEntryStatus TimeEntryStatus => Status switch
    {
        "Afventer" => TimeEntryStatus.Afventer,
        "Godkendt" => TimeEntryStatus.Godkendt,
        "Afvist" => TimeEntryStatus.Afvist,
        _ => TimeEntryStatus.Afventer
    };

    [JsonIgnore]
    public TimeSpan TotalHoursTimeSpan => TimeSpan.FromHours((double)TotalHours);

    [JsonIgnore]
    public TimeSpan TimeBankTimeSpan => TimeSpan.FromHours((double)TimeBank);
}
