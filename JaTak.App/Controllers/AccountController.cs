using JaTak.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace JaTak.App.Controllers
{
    /// <summary>
    /// Handles the pages for creating a user account.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Shows the empty create account form.
        /// </summary>
        /// <returns>The register view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        /// <summary>
        /// Shows the message that the account was created.
        /// </summary>
        /// <returns>The confirmation view, or the form if no account was just created.</returns>
        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            if (TempData["RegisteredEmail"] is not string email)
            {
                return RedirectToAction(nameof(Register));
            }

            ViewData["Email"] = email;
            return View();
        }
    }
}