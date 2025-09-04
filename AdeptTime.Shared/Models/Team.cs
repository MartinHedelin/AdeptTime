using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace AdeptTime.Shared.Models;

[Table("teams")]
public class Team : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("color")]
    public string Color { get; set; } = "#3498db";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
