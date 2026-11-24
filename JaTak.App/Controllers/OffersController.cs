using JaTak.App.Models;
using JaTak.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 24-11-2026

namespace JaTak.App.Controllers
{
    /// <summary>
    /// Handles requests related to offers in the MVC application.
    /// Retrieves offers and forwards reservation requests to the Web API.
    /// </summary>
    public class OffersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserApiService _userApiService;
        private readonly ILogger<OffersController> _logger;

        /// <summary>
        /// Initializes a new instance of the OffersController class.
        /// </summary>
        public OffersController(
            IHttpClientFactory httpClientFactory,
            UserApiService userApiService,
            ILogger<OffersController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _userApiService = userApiService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves active offers from the Web API.
        /// Synchronizes the authenticated user with the API.
        /// </summary>
        /// <returns>The offers overview.</returns>
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    var user = await _userApiService.GetCurrentUserAsync();

                    _logger.LogInformation(
                        "User synchronization successful. UserId: {UserId}",
                        user?.UserId);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex,
                        "User synchronization failed.");
                }
                catch (TaskCanceledException ex)
                {
                    _logger.LogError(ex,
                        "User synchronization request was cancelled.");
                }
            }

            var client = _httpClientFactory.CreateClient("JaTakApi");

            var offers = await client.GetFromJsonAsync<List<OfferViewModel>>(
                "api/Offers");

            return View(offers ?? new List<OfferViewModel>());
        }

        /// <summary>
        /// Creates a reservation for the authenticated user.
        /// </summary>
        /// <param name="offerId">The ID of the offer to reserve.</param>
        /// <param name="quantity">The number of items to reserve.</param>
        /// <returns>A success or error message.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation(
            int offerId,
            int quantity)
        {
            if (quantity <= 0)
            {
                TempData["ErrorMessage"] = "Antal skal være mindst 1.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var client = _httpClientFactory.CreateClient("JaTakApi");

                var reservation = new
                {
                    OfferId = offerId,
                    Quantity = quantity
                };

                var response = await client.PostAsJsonAsync(
                    "api/Reservations",
                    reservation);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Din reservation er gennemført!";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["ErrorMessage"] =
                        "Din session er udløbet. Log ind igen.";
                }
                else
                {
                    var error = await response.Content
                        .ReadFromJsonAsync<ErrorResponse>();

                    _logger.LogWarning(
                        "Reservation failed. Status: {StatusCode}",
                        response.StatusCode);

                    TempData["ErrorMessage"] =
                        error?.Message ??
                        "Reservationen kunne ikke gennemføres.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Reservation request failed.");

                TempData["ErrorMessage"] =
                    "Der opstod en fejl ved reservationen.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Represents an error response returned by the Web API.
        /// </summary>
        private class ErrorResponse
        {
            public string? Message { get; set; }
        }
    }
}