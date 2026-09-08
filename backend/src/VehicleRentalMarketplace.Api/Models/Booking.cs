using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Booking : BaseModel
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public int AssetID { get; set; }

        [ForeignKey(nameof(AssetID))]
        public Asset Asset { get; set; } = null!;

        [Required]
        public int UserID { get; set; } 

        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!; 

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int NumberofDays { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRentalPrice { get; set; }

        // Status: "FullyPaid", "Cancelled"
        public string Status { get; set; } = "FullyPaid";

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        public string? PaymentReference { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
}