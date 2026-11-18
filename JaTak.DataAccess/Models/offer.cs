using System;
using System.Collections.Generic;
using System.Text;

namespace JaTak.DataAccess.Models
{
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
        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public int QuantityLeft { get; set; }



    }
}
