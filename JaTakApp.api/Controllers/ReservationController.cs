using JaTak.BusinessLogic.Services;
using JaTak.WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 24-11-2026

namespace JaTak.WebApi.Controllers
{
    /// <summary>
    /// Handles HTTP requests related to reservations.
    /// Provides endpoints for creating reservations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        /// <summary>
        /// Initializes a new instance of the ReservationsController class.
        /// </summary>
        /// <param name="reservationService">
        /// Service responsible for handling reservation operations.
        /// </param>
        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Creates a reservation for the authenticated user.
        /// </summary>
        /// <param name="reservationDto">
        /// Contains the offer ID and requested quantity.
        /// </param>
        /// <returns>
        /// HTTP 201 Created if the reservation is successful;
        /// otherwise, an appropriate error response.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateReservation(
            CreateReservationDto reservationDto)
        {
            string? keycloakId =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(keycloakId))
            {
                return Unauthorized();
            }

            try
            {
                var reservation =
                    await _reservationService.CreateReservationAsync(
                        keycloakId,
                        reservationDto.OfferId,
                        reservationDto.Quantity);

                return StatusCode(StatusCodes.Status201Created, reservation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}