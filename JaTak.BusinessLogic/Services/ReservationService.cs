using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories;

namespace JaTak.BusinessLogic.Services
{

    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;

        public ReservationService(
            ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
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


            var (status, reservationId) =
    await _reservationRepository.CreateReservationAsync(
        keycloakId, offerId, quantity);

            switch (status)
            {
                case "UserNotFound":
                    throw new InvalidOperationException(
                        "Brugerprofilen blev ikke fundet.");

                case "OfferNotFound":
                    throw new InvalidOperationException(
                        "Tilbuddet findes ikke eller er ikke længere aktivt.");

                case "InsufficientQuantity":
                    throw new InvalidOperationException(
                        "Der er ikke nok varer tilbage.");
            }

            Reservation reservation = new Reservation
            {
                ReservationId = reservationId!.Value,
                OfferId = offerId,
                Quantity = quantity,
                ReservedAt = DateTime.Now
            };

            return reservation; 
        }
    }
}