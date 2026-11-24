using JaTak.DataAccess.Models;

namespace JaTak.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines database operations related to reservations.
    /// </summary>
    public interface IReservationRepository
    {
        /// <summary>
        /// Creates a reservation and returns its generated ID.
        /// Returns null if the reservation cannot be created.
        /// </summary>
        Task<int?> CreateReservationAsync(Reservation reservation);
    }
}