namespace VehicleRentalMarketplace.Api.Dtos.Review
{
    public class ReviewRequest
    {
        public int AssetID { get; set; }

        public int? BookingID { get; set; }

        public int? PurchaseID { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}