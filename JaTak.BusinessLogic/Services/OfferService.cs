using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories;

namespace JaTak.BusinessLogic.Services
{
    public class OfferService
    {
        private readonly OfferRepository _offerRepository;

        public OfferService(OfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<List<Offer>> GetActiveOffersAsync()
        {
            return await _offerRepository.GetActiveOffersAsync();
        }
    }
}