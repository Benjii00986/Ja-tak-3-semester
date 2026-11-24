using JaTak.DataAccess.Models;

namespace JaTak.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines database operations related to JaTak users.
    /// </summary>
    public interface IJaTakUserRepository
    {
        /// <summary>
        /// Retrieves a user by their Keycloak ID.
        /// Returns null if the user does not exist.
        /// </summary>
        Task<JaTakUser?> GetByKeycloakIdAsync(string keycloakId);
    }
}