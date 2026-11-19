using JaTak.App.Models;
using JaTak.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace JaTak.App.Controllers
{
    public class OffersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserApiService _userApiService;
        private readonly ILogger<OffersController> _logger;

        public OffersController(
            IHttpClientFactory httpClientFactory,
            UserApiService userApiService,
            ILogger<OffersController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _userApiService = userApiService;
            _logger = logger;
        }

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
    }
}