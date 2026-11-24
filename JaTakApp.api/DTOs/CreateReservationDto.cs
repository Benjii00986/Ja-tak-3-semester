using System.ComponentModel.DataAnnotations;

namespace JaTak.WebApi.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        public int OfferId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}