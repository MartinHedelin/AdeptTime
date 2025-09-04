using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace AdeptTime.Shared.Models;

[Table("users")]
public class User : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("user_type_id")]
    public int UserTypeId { get; set; } = 0; // 0 = worker, 1 = admin

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("user_state")]
    public string UserState { get; set; } = "active";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Helper properties (computed, not stored in DB)
    [JsonIgnore]
    public bool IsAdmin => UserTypeId == 1;

    [JsonIgnore]
    public bool IsWorker => UserTypeId == 0;
}

// Simple DTO for database operations to avoid serialization issues
public class UserDto
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int UserTypeId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
