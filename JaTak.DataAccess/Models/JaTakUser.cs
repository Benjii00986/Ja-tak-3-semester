// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.DataAccess.Models;

/// <summary>
/// Represents a user in the JaTak application, including their Keycloak identity,
/// contact information and account creation date.
/// </summary>
public class JaTakUser
{
    public int UserId { get; set; }

    public string KeycloakId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
