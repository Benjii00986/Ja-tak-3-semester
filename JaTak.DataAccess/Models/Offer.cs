// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.DataAccess.Models
{
    /// <summary>
    /// Represents a Ja Tak offer containing product information,
    /// pricing, availability and pickup details.
    /// </summary>
    public class Offer
    {
        public int OfferId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string ShopName { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string PickupPoint { get; set; }

        public DateTime? PickupStartDateTime { get; set; }

        public DateTime? PickupEndDateTime { get; set; }

        public decimal OriginalPrice { get; set; }

        public int Quantity { get; set; }

        public int QuantityLeft { get; set; }
    }
}
