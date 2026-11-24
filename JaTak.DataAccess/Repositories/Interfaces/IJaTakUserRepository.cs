using JaTak.DataAccess.Models;

namespace JaTak.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines database operations related to JaTak users.
    /// </summary>
    public interface IJaTakUserRepository
    {
        Task<JaTakUser?> GetByKeycloakIdAsync(string keycloakId);

        Task<JaTakUser?> GetByEmailAsync(string email);

        Task<int> CreateAsync(JaTakUser user);

        Task UpdateAsync(JaTakUser user);
    }
}