namespace JaTak.WebApi.DTOs
{
    public class OfferDto
    {
        public int OfferId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string ShopName { get; set; }

        public decimal Price { get; set; }

        public string PickupPoint { get; set; } = string.Empty;

        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }

        public int QuantityLeft { get; set; }
    }
}