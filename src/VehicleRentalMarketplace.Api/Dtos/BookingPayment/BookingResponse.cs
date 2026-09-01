namespace VehicleRentalMarketplace.Api.Dtos.Booking
{
    public class BookingResponse
    {
        public int BookingID { get; set; }
        public int AssetID { get; set; }
        public string AssetTitle { get; set; } = string.Empty;
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberofDays { get; set; }
        public decimal DailyRate { get; set; }
        public decimal TotalRentalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public string? PaymentReference { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}