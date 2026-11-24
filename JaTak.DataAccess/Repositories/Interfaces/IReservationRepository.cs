using JaTak.DataAccess.Models;

namespace JaTak.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines database operations related to reservations.
    /// </summary>
    public interface IReservationRepository
    {
        Task<int?> CreateReservationAsync(Reservation reservation);
    }
}