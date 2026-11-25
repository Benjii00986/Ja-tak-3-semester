using JaTak.App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace JaTak.App.Controllers
{
    [Authorize]
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
    }
}