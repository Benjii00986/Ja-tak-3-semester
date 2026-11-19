using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace JaTak.App.Controllers;

public class AccountController : Controller
{
    // Redirects the user to Keycloak for login.
    [HttpGet]
    public IActionResult Login()
    {
        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "/"
            },
            "OpenIdConnect");
    }

    // Logs the user out of JaTak and Keycloak.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        return SignOut(
            new AuthenticationProperties
            {
                RedirectUri = "/"
            },
            "Cookies",
            "OpenIdConnect");
    }
}