using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [ForeignKey("Asset")]
        public int? BookingID { get; set; }

        [ForeignKey("Purchase")]
        public int? PurchasedID { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // Credit Card, PayPal, etc.

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed

        [StringLength(100)]
        public string? TransactionNumber { get; set; }

        public DateTime? PaidAt { get; set; }

        // Navigation properties
        public virtual Asset? Asset { get; set; }
        public virtual Purchase? Purchase { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}