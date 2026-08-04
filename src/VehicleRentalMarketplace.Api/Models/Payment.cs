using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Explicit Foreign Key to the user making the payment.
        /// </summary>
        [Required]
        public Guid UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!;

        public Guid? BookingID { get; set; }

        [ForeignKey(nameof(BookingID))]
        public Booking? Booking { get; set; }

        public Guid? PurchaseID { get; set; }

        [ForeignKey(nameof(PurchaseID))]
        public Purchase? Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Card"; // e.g., Card, Cash, Bank Transfer

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending"; // e.g., Pending, Success, Failed

        public string TransactionID { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp set ONLY when payment is completed/successful.
        /// Remains null while PaymentStatus is "Pending".
        /// </summary>
        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}