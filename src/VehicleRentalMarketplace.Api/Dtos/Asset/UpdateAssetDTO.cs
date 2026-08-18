namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class UpdateAssetDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryID { get; set; }
        public int? ListingTypeID { get; set; }
        public decimal? DailyRate { get; set; }
        public decimal? SalePrice { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; } // Available, Rented, Sold
    }
}