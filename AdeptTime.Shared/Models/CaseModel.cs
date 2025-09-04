using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace AdeptTime.Shared.Models;

[Table("cases")]
public class CaseModel : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("case_number")]
    public string CaseNumber { get; set; } = string.Empty;

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("team_id")]
    public Guid? TeamId { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("assigned_to")]
    public Guid? AssignedTo { get; set; }

    [Column("customer_id")]
    public int? CustomerId { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Ny";

    [Column("priority")]
    public string Priority { get; set; } = "Medium";

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("estimated_hours")]
    public int? EstimatedHours { get; set; }

    [Column("completed_hours")]
    public int CompletedHours { get; set; } = 0;

    // Geofence properties
    [Column("geofence_address")]
    public string? GeofenceAddress { get; set; }

    [Column("geofence_latitude")]
    public decimal? GeofenceLatitude { get; set; }

    [Column("geofence_longitude")]
    public decimal? GeofenceLongitude { get; set; }

    [Column("geofence_radius")]
    public int GeofenceRadius { get; set; } = 100;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties (not stored in DB, excluded from Supabase operations)
    [JsonIgnore]
    public Team? Team { get; set; }

    [JsonIgnore]
    public User? CreatedByUser { get; set; }

    [JsonIgnore]
    public User? AssignedToUser { get; set; }

    [JsonIgnore]
    public Customer? Customer { get; set; }

    // Helper properties for UI compatibility with existing Case model
    [JsonIgnore]
    public CaseStatus CaseStatus => Status switch
    {
        "Ny" => CaseStatus.Pending,
        "I gang" => CaseStatus.InProgress,
        "Afventer" => CaseStatus.Pending,
        "Færdig" => CaseStatus.Completed,
        "Annulleret" => CaseStatus.Cancelled,
        _ => CaseStatus.Pending
    };
}
