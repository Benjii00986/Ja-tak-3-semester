using JaTak.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JaTak.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly JaTakUserService _userService;

        public UsersController(JaTakUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves the authenticated user's JaTak profile.
        /// Creates a profile if the user does not already exist.
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Retrieves the user's information from the Keycloak token.
            string? keycloakId = User.FindFirst("sub")?.Value;
            string? email = User.FindFirst("email")?.Value;
            string? phoneNumber = User.FindFirst("phoneNumber")?.Value;

            // Ensures the required claims are available.
            if (string.IsNullOrWhiteSpace(keycloakId) ||
                string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized(
                    "Brugeroplysninger mangler i adgangstokenet.");
            }

            // Retrieves the existing user or creates a new JaTak profile.
            var user = await _userService.GetOrCreateUserAsync(
                keycloakId,
                email,
                phoneNumber);

            return Ok(new
            {
                user.UserId,
                user.KeycloakId,
                user.Email,
                user.PhoneNumber,
                user.CreatedAt,
            });
        }
    }
}