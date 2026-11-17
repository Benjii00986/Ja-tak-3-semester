using JaTakApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace JaTakApp.Controllers
{
    public class OffersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OffersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("JaTakApi");

            var offers = await client.GetFromJsonAsync<List<OfferViewModel>>(
                "api/Offers");

            return View(offers);
        }
    }
}