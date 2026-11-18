using JaTakApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace JaTakApp.Controllers
{
    /// <summary>
    /// Handles displaying offers in the MVC application.
    /// </summary>
    public class OffersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OffersController"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create HTTP clients.</param>
        public OffersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Retrieves active offers from the Web API and displays them.
        /// </summary>
        /// <returns>The offers view containing the retrieved offers.</returns>
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("JaTakApi");

            var offers = await client.GetFromJsonAsync<List<OfferViewModel>>(
                "api/Offers");

            return View(offers ?? new List<OfferViewModel>());
        }
    }
}