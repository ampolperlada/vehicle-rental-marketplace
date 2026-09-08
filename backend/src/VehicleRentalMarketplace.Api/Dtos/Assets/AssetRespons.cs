namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class AssetResponse
    {
        public int AssetID { get; set; }
        public int UserID { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ListingTypeId { get; set; }
        public string ListingTypeName { get; set; } = string.Empty;
        public decimal? DailyRate { get; set; }
        public decimal? SalePrice { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}