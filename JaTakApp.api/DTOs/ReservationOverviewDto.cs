namespace JaTak.WebApi.DTOs
{
    /// <summary>
    /// Contains reservation information for the reservation overview.
    /// </summary>
    public class ReservationOverviewDto
    {
        public int ReservationId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}