using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JaTak.App.Controllers
{
    [Authorize]
    public class MyReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}