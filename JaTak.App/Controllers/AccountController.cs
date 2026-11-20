using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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

    /// <summary>
    /// Redirects the user directly to the Keycloak registration page.
    /// </summary>
    /// <returns>A redirect to Keycloak's create account page.</returns>
    [HttpGet]
    public IActionResult Register()
    {
        return Challenge(
            new OpenIdConnectChallengeProperties
            {
                RedirectUri = "/",
                Prompt = "create"
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