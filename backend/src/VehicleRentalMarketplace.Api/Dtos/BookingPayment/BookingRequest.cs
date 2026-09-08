namespace VehicleRentalMarketplace.Api.Dtos.Booking
{
    public class BookingRequest
    {
        public int AssetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
    }
}