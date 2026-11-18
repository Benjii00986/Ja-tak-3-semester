namespace JaTak.WebApi.DTOs
{
    /// <summary>
    /// Represents the offer data transferred from the API to the client.
    /// Contains product information, pricing and availability details.
    /// </summary>
    public class OfferDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the offer.
        /// </summary>
        public int OfferId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the shop offering the product.
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// Gets or sets the discounted price of the product.
        /// </summary>>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the product.
        /// Defaults to an empty string.
        /// </summary>
        public string PickupPoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the original price before the discount.
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Gets or sets the total quantity available in the offer.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the remaining quantity available.
        /// </summary>
        public int QuantityLeft { get; set; }
    }
}