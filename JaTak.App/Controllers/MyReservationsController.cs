using JaTak.App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace JaTak.App.Controllers
{
    [Authorize]
    [Route("MineReservationer")]
    public class MyReservationsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MyReservationsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = _httpClientFactory.CreateClient("JaTakApi");

            List<ReservationOverviewViewModel>? reservations =
                await client.GetFromJsonAsync<List<ReservationOverviewViewModel>>(
                    "api/Reservations/my");

            return View(reservations ?? new List<ReservationOverviewViewModel>());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            HttpClient client = _httpClientFactory.CreateClient("JaTakApi");

            HttpResponseMessage response = await client.GetAsync($"api/Reservations/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            ReservationDetailsViewModel? details =
                await response.Content.ReadFromJsonAsync<ReservationDetailsViewModel>();

            return PartialView("_ReservationDetails", details);
        }
    }
}