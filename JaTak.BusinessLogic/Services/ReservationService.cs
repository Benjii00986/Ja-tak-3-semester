using System.Collections.Generic;
using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories.Interfaces;

namespace JaTak.BusinessLogic.Services
{
    /// <summary>
    /// Handles the business logic related to reservations.
    /// Validates reservation requests and coordinates user, offer and reservation data.
    /// </summary>
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IJaTakUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the ReservationService class.
        /// </summary>
        public ReservationService(
            IReservationRepository reservationRepository,
            IOfferRepository offerRepository,
            IJaTakUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _offerRepository = offerRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Creates a reservation for an authenticated user.
        /// </summary>
        public async Task<Reservation> CreateReservationAsync(
            string keycloakId,
            int offerId,
            int quantity)
        {
            if (string.IsNullOrWhiteSpace(keycloakId))
            {
                throw new ArgumentException(
                    "Du skal være logget ind for at reservere.", nameof(keycloakId));
            }

            if (offerId <= 0)
            {
                throw new ArgumentException(
                    "Ugyldigt tilbuds-ID.", nameof(offerId));
            }

            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "Antal skal være mindst 1.", nameof(quantity));
            }

            JaTakUser? user =
                await _userRepository.GetByKeycloakIdAsync(keycloakId);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    "Brugerprofilen blev ikke fundet.");
            }

            Offer? offer =
                await _offerRepository.GetOfferByIdAsync(offerId);

            if (offer == null)
            {
                throw new KeyNotFoundException(
                    "Tilbuddet blev ikke fundet.");
            }

            if (!offer.IsActive)
            {
                throw new InvalidOperationException(
                    "Tilbuddet er ikke længere aktivt.");
            }

            if (quantity > offer.QuantityLeft)
            {
                throw new InvalidOperationException(
                    $"Der er kun {offer.QuantityLeft} stk. tilbage.");
            }

            Reservation reservation = new Reservation
            {
                UserId = user.UserId,
                OfferId = offerId,
                Quantity = quantity
            };

            int? reservationId =
                await _reservationRepository.CreateReservationAsync(reservation);

            if (reservationId == null)
            {
                throw new InvalidOperationException(
                    "Der er ikke længere nok varer tilbage.");
            }

            reservation.ReservationId = reservationId.Value;
            reservation.ReservedAt = DateTime.UtcNow;

            return reservation;
        }

        /// <summary>
        /// Retrieves all reservations belonging to an authenticated user.
        /// </summary>
        /// <param name="keycloakId">The user's Keycloak ID.</param>
        /// <returns>A list of the user's reservations.</returns>
        public async Task<List<ReservationOverview>> GetReservationsByUserAsync(string keycloakId)
        {
            if (string.IsNullOrWhiteSpace(keycloakId))
            {
                throw new ArgumentException(
                    "Du skal være logget ind for at se dine reservationer.", nameof(keycloakId));
            }

            JaTakUser? user =
                await _userRepository.GetByKeycloakIdAsync(keycloakId);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    "Brugerprofilen blev ikke fundet.");
            }

            return await _reservationRepository.GetReservationsByUserIdAsync(user.UserId);
        }
        public async Task<ReservationDetails> GetReservationDetailsAsync(string keycloakId, int reservationId)
        {
            if (string.IsNullOrWhiteSpace(keycloakId))
            {
                throw new ArgumentException(
                    "Du skal være logget ind for at se reservationen.", nameof(keycloakId));
            }

            JaTakUser? user =
                await _userRepository.GetByKeycloakIdAsync(keycloakId);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    "Brugerprofilen blev ikke fundet.");
            }

            ReservationDetails? details =
                await _reservationRepository.GetReservationDetailsAsync(reservationId, user.UserId);

            if (details == null)
            {
                throw new KeyNotFoundException(
                    "Reservationen blev ikke fundet.");
            }

            return details;
        }
    }
}