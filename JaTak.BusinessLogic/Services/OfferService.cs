using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories.Interfaces;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.BusinessLogic.Services
{
    /// <summary>
    /// Provides business logic for retrieving offers.
    /// Acts as an intermediary between the application and the OfferRepository.
    /// </summary>
    public class OfferService
    {
        private readonly IOfferRepository _offerRepository;

        /// <summary>
        /// Initializes a new instance of the OfferService class.
        /// </summary>
        /// <param name="offerRepository">
        /// The repository responsible for accessing offer data.
        /// </param>
        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        /// <summary>
        /// Retrieves all currently active offers.
        /// </summary>
        /// <returns>
        /// A list containing all active offers.
        /// </returns>
        public async Task<List<Offer>> GetActiveOffersAsync()
        {
            return await _offerRepository.GetActiveOffersAsync();
        }

        /// <summary>
        /// Retrieves an offer using its unique ID.
        /// </summary>
        /// <param name="offerId">The unique ID of the offer.</param>
        /// <returns>
        /// The offer if found; otherwise, null.
        /// </returns>
        public async Task<Offer?> GetOfferByIdAsync(int offerId)
        {
            return await
            _offerRepository.GetOfferByIdAsync(offerId);
        }
    }
}