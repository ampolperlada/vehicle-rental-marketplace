using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Models
{
    public class Purchase
    {
        [Key]
        public int PurchasedID { get; set; }

        [ForeignKey("Asset")]
        public int BookingID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [StringLength(500)]
        public string? RejectedReason { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public int? UserID { get; set; }

        // Navigation properties
        public virtual Asset? Asset { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}