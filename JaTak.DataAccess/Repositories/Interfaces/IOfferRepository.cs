using JaTak.DataAccess.Models;

namespace JaTak.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Defines database operations related to offers.
    /// </summary>
    public interface IOfferRepository
    {
        Task<List<Offer>> GetActiveOffersAsync();

        Task<Offer?> GetOfferByIdAsync(int offerId);
    }
}