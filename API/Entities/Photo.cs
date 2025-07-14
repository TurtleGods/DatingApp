using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    // Navigation properties
    [JsonIgnore]
    public Member Member { get; set; } = null!; // Ensure non-nullable reference type
    public string MemberId { get; set; } = null!;
}
