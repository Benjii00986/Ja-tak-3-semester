using JaTak.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.App.Controllers
{
    /// <summary>
    /// Handles the general pages of the MVC application,
    /// such as the home page, the privacy page and the error page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The home page view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the privacy page.
        /// </summary>
        /// <returns>The privacy page view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the error page when an unhandled error occurs.
        /// Caching is disabled so the error page is never stored.
        /// </summary>
        /// <returns>The error page view with the ID of the failed request.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
