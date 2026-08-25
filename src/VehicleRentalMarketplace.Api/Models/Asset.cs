using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Asset : BaseModel
    {
        [Key]
        public int AssetID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User Owner { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;  

        [Required]
        [MaxLength(150)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DailyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;  
        public bool IsAvailable { get; set; } = true;


        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;


        [Required]
        public int ListingTypeId { get; set; }

        [ForeignKey(nameof(ListingTypeId))]
        public ListingType ListingType { get; set; } = null!;

    }
}