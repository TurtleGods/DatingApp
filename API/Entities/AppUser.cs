using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public required byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
}