using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Booking
    {
        [Key]
        public Guid BookingID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid RenterID { get; set; }

        /// <summary>
        /// Explicit Foreign Key referencing UserID in the User/Auth table.
        /// </summary>
        [ForeignKey(nameof(RenterID))]
        public User Renter { get; set; } = null!;

        [Required]
        public Guid AssetID { get; set; }

        [ForeignKey(nameof(AssetID))]
        public Asset Asset { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int NumberofDays { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRentalPrice { get; set; }

        /// <summary>
        /// Lifecycle statuses:
        /// - "Pending": Initial creation.
        /// - "Paid": Payment complete.
        /// - "Canceled": System/Admin canceled (e.g., StartDate reached without payment).
        /// - "Completed": Owner/Admin manually completes booking post-EndDate.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }   
}