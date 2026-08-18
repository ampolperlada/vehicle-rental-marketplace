using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VehicleRentalMarketplace.Models
{
    public class Asset
    {
        [Key]
        public int AssetID { get; set; }

        [ForeignKey("User")]
        public int RenterID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [ForeignKey("Category")]
        public int? CategoryID { get; set; }  // Renamed for clarity

        [ForeignKey("ListingType")]
        public int? ListingTypeID { get; set; }  // Foreign key to ListingType

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DailyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        [StringLength(255)]
        public string? Location { get; set; }

        [StringLength(20)]
        public string ApprovalStatus { get; set; } = "Pending"; // Pending, Approved, Rejected

        public int? ApproveBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(20)]
        public string Status { get; set; } = "Available"; // Available, Rented, Sold

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ListingType? ListingType { get; set; }  // New navigation
        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}