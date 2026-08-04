using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Asset
    {
        [Key]
        public Guid AssetID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User Owner { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = "Vehicle"; 

        /// <summary>
        /// Defines the mode: "Rental" (for renters) or "Sale" (for buyers)
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string ListingType { get; set; } = "Rental"; 

        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        public string Location { get; set; } = string.Empty;

        public string ApprovalStatus { get; set; } = "Pending"; 

        public Guid? ApprovedBy { get; set; }

        /// <summary>
        /// Controls asset availability.
        /// Note: This is NOT toggled automatically when a booking ends. 
        /// An Owner or Admin must manually trigger the end-of-booking endpoint,
        /// which sets IsAvailable = true and updates UpdatedAt.
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Updated whenever an Owner/Admin changes state or completes a booking.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}