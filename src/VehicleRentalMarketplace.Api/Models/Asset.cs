using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Asset
    {
        [Key]
        public Guid AssetID { get; set; } = Guid.NewGuid();

        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User Owner { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "Vehicle"; 
        public string ListingType { get; set; } = "Rent"; 

        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        public string Location { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = "Pending"; 
        public Guid? ApprovedBy { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}