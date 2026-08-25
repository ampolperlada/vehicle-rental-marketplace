namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class AssetResponse
    {
        public int AssetID { get; set; }
        public int UserID { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ListingType { get; set; } = string.Empty;
        public decimal? DailyRate { get; set; }
        public decimal? SalePrice { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}