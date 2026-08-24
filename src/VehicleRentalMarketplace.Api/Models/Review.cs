using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Models
{
    public class Review
    {
        [Key]
        public int ReviewerID { get; set; }

        [ForeignKey("Payment")]
        public int PaymentID { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Payment? Payment { get; set; }
        public virtual User? User { get; set; }
    }
}