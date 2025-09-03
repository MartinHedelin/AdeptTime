using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace AdeptTime.Shared.Models;

[Table("user_types")]
public class UserType : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("description")]
    public string Description { get; set; } = string.Empty;
}
