using System.ComponentModel.DataAnnotations;

namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class AssetRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; } 
        public int ListingTypeId { get; set; }  
        public decimal? DailyRate { get; set; }
        public decimal? SalePrice { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;
    }
}