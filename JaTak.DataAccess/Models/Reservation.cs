using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 24-11-2026

namespace JaTak.DataAccess.Models
{
    public class Reservation
    {
        public int reservationId { get; set; }
        public int userId { get; set; }
        public int offerId { get; set; }
        public int quantity { get; set; }
        public DateTime reservedAt { get; set; }
    }

}
