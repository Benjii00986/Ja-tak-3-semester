namespace JaTak.App.ViewModels
{
    /// <summary>
    /// Contains reservation information displayed in the reservation overview.
    /// </summary>
    public class ReservationOverviewViewModel
    {
        public int ReservationId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}