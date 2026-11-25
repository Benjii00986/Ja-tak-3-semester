namespace JaTak.DataAccess.Models
{
    /// <summary>
    /// Contains the basic information displayed in a reservation overview.
    /// </summary>
    public class ReservationOverview
    {
        public int ReservationId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public DateTime? PickupEndDateTime { get; set; }

        public bool IsCompleted { get; set; }
    }
}