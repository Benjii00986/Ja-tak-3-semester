using JaTak.BusinessLogic.Services;
using JaTak.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JaTak.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly OfferService _offerService;

        public OffersController(OfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OfferDto>>> GetActiveOffers()
        {
            var offers = await _offerService.GetActiveOffersAsync();

            var offerDtos = offers.Select(offer => new OfferDto
            {
                OfferId = offer.OfferId,
                ProductName = offer.ProductName,
                Description = offer.Description,
                ImageUrl = offer.ImageUrl,
                ShopName = offer.ShopName,
                Price = offer.Price,
                PickupPoint = offer.PickupPoint,
                OriginalPrice = offer.OriginalPrice,
                Quantity = offer.Quantity,
                QuantityLeft = offer.QuantityLeft,
            }).ToList();

            return Ok(offerDtos);
        }
    }
}