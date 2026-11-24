using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories;

namespace JaTak.BusinessLogic.Services
{

    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly OfferRepository _offerRepository;
        private readonly JaTakUserRepository _userRepository;

        public ReservationService(
            ReservationRepository reservationRepository,
            OfferRepository offerRepository,
            JaTakUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _offerRepository = offerRepository;
            _userRepository = userRepository;
        }
           
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
                offerId = offerId,
                quantity = quantity
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

        }
    }
}