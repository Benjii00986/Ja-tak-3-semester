using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JaTak.DataAccess.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int OfferId { get; set; }
        public int Quantity { get; set; }
        public DateTime ReservedAt { get; set; }
    }

}
