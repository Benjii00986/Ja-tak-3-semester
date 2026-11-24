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
        /// <param name="reservationRepository">
        /// Repository responsible for saving reservation data.
        /// </param>
        /// <param name="offerRepository">
        /// Repository responsible for retrieving offer data.
        /// </param>
        /// <param name="userRepository">
        /// Repository responsible for retrieving user data.
        /// </param>
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
        /// Validates the user, offer and requested quantity before saving
        /// the reservation to the database.
        /// </summary>
        /// <param name="keycloakId">
        /// The Keycloak ID of the authenticated user.
        /// </param>
        /// <param name="offerId">
        /// The unique ID of the offer being reserved.
        /// </param>
        /// <param name="quantity">
        /// The number of products the user wants to reserve.
        /// </param>
        /// <returns>
        /// The created reservation, including its generated ID and reservation date.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the Keycloak ID is empty or the requested quantity is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the user or offer cannot be found, the offer is inactive,
        /// or there is insufficient available quantity.
        /// </exception>
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

            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "Antal skal være mindst 1.", nameof(quantity));
            }

            JaTakUser? user =
                await _userRepository.GetByKeycloakIdAsync(keycloakId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Brugerprofilen blev ikke fundet.");
            }

            Offer? offer = await _offerRepository.GetOfferByIdAsync(offerId);

            if (offer == null || !offer.IsActive)
            {
                throw new InvalidOperationException(
                    "Tilbuddet findes ikke eller er ikke længere aktivt.");
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
    }
}