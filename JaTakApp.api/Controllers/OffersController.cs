using JaTak.BusinessLogic.Services;
using JaTak.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JaTak.WebApi.Controllers
{

    /// <summary>
    /// Handles HTTP requests related to offers.
    /// Provides endpoints for retrieving active offers and individual offer details.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly OfferService _offerService;

        /// <summary>
        /// Initializes a new instance of the OffersController class.
        /// </summary>
        /// <param name="offerService">
        /// Service responsible for retrieving offer data.
        /// </param>
        public OffersController(OfferService offerService)
        {
            _offerService = offerService;
        }

        /// <summary>
        /// Retrieves all active offers.
        /// Converts the offers into DTOs before returning them.
        /// </summary>
        /// <returns>
        /// HTTP 200 OK with a list of active offers.
        /// </returns>
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
                PickupStartDateTime = offer.PickupStartDateTime,
                PickupEndDateTime = offer.PickupEndDateTime,
                OriginalPrice = offer.OriginalPrice,
                Quantity = offer.Quantity,
                QuantityLeft = offer.QuantityLeft,
            }).ToList();

            return Ok(offerDtos);
        }

        /// <summary>
        /// Retrieves the details of a specific offer using its ID.
        /// </summary>
        /// <param name="id">The unique ID of the requested offer.</param>
        /// <returns>
        /// HTTP 200 OK with the offer details if found;
        /// otherwise, HTTP 404 Not Found.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<OfferDto>> GetOfferById(int id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);

            if (offer == null)
            {
                return NotFound();
            }

            var offerDto = new OfferDto
            {
                OfferId = offer.OfferId,
                ProductName = offer.ProductName,
                Description = offer.Description,
                ImageUrl = offer.ImageUrl,
                ShopName = offer.ShopName,
                Price = offer.Price,
                PickupPoint = offer.PickupPoint,
                PickupStartDateTime = offer.PickupStartDateTime,
                PickupEndDateTime = offer.PickupEndDateTime,
                OriginalPrice = offer.OriginalPrice,
                Quantity = offer.Quantity,
                QuantityLeft = offer.QuantityLeft
            };

            return Ok(offerDto);
        }

    }
}