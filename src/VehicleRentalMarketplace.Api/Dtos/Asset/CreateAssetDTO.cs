using System.ComponentModel.DataAnnotations;

namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class CreateAssetDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        public int ListingTypeID { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DailyRate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalePrice { get; set; }

        [StringLength(255)]
        public string? Location { get; set; }
    }
}