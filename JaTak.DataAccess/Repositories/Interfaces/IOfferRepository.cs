using JaTak.DataAccess.Models;

namespace JaTak.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines database operations related to offers.
    /// </summary>
    public interface IOfferRepository
    {
        /// <summary>
        /// Retrieves an offer by its ID.
        /// Returns null if the offer does not exist.
        /// </summary>
        Task<Offer?> GetOfferByIdAsync(int offerId);
    }
}