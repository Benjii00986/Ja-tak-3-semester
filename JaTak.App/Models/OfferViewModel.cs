namespace JaTak.App.Models
{
    /// <summary>
    /// Represents the offer information displayed in the application's user interface.
    /// </summary>
    public class OfferViewModel
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
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the pickup location for the product.
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
        /// Gets or sets the original price before the discount.
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Gets or sets the total quantity available.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the remaining quantity available.
        /// </summary>
        public int QuantityLeft { get; set; }

        /// <summary>
        /// Gets the CSS class used to indicate the remaining stock level.
        /// </summary>
        public string StockColorClass
        {
            get
            {
                if (QuantityLeft <= 0)
                {
                    return "stock-red";
                }

                double ratio = Quantity > 0
                    ? (double)QuantityLeft / Quantity
                    : 0;

                if (ratio < 0.25)
                {
                    return "stock-red";
                }

                if (ratio < 0.50)
                {
                    return "stock-orange";
                }

                return "stock-green";
            }
        }

        /// <summary>
        /// Gets the text displayed in the remaining stock indicator.
        /// </summary>
        public string StockText
        {
            get
            {
                if (QuantityLeft <= 0)
                {
                    return "UDSOLGT";
                }

                if (StockColorClass == "stock-red")
                {
                    return $"KUN {QuantityLeft} TILBAGE!";
                }

                return $"{QuantityLeft} TILBAGE";
            }
        }
    }
}
