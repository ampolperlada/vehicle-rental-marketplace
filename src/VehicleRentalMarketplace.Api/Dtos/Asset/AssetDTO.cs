namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class AssetDTO
    {
        public int AssetID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public decimal? DailyRate { get; set; }
        public decimal? SalePrice { get; set; }
        public string ListingType { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // Owner information
        public string OwnerName { get; set; } = string.Empty;
        public int OwnerID { get; set; }

        // ✅ NEW: Shows if the current user owns this asset
        public bool IsOwner { get; set; }
    }
}