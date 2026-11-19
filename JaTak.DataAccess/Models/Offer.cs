namespace JaTak.DataAccess.Models
{
    /// <summary>
    /// Represents a Ja Tak offer containing product information,
    /// pricing, availability and pickup details.
    /// </summary>
    public class Offer
    {
        /// <summary>
        /// Gets or sets the unique identifier of the offer.
        /// </summary>
        public int OfferId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product being offered.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets a description of the product.
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
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets whether the offer is currently active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the location where the product can be picked up.
        /// </summary>
        public string PickupPoint { get; set; }

        /// <summary>
        /// Gets or sets the starting date and time for product pickup.
        /// </summary>
        public DateTime? PickupStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the ending date and time for product pickup.
        /// </summary>
        public DateTime? PickupEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the original price of the product before the discount.
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Gets or sets the total quantity of the product available.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product left in stock.
        /// </summary>
        public int QuantityLeft { get; set; }
    }
}
