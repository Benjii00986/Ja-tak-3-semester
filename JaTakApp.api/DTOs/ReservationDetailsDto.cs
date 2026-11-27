// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 27-11-2026

namespace JaTak.WebApi.DTOs
{
    /// <summary>
    /// Contains the reservation details sent to the client for the reservation details popup.
    /// </summary>
    public class ReservationDetailsDto
    {
        public int ReservationId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string ShopName { get; set; } = string.Empty;

        public string? ShopAddress { get; set; }

        public int Quantity { get; set; }

        public DateTime? PickupStartDateTime { get; set; }

        public DateTime? PickupEndDateTime { get; set; }

        public string? PickupCode { get; set; }

        public bool IsCompleted { get; set; }
    }
}