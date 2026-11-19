namespace JaTak.DataAccess.Models;

public class JaTakUser
{
    public int UserId { get; set; }

    public string KeycloakId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
