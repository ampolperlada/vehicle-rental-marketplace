using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Review
    {
        [Key]
        public Guid ReviewID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Explicit Foreign Key referencing the user writing the review.
        /// </summary>
        [Required]
        public Guid UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!;

        /// <summary>
        /// Links to the specific booking being reviewed (if rental).
        /// </summary>
        public Guid? BookingID { get; set; }

        [ForeignKey(nameof(BookingID))]
        public Booking? Booking { get; set; }

        /// <summary>
        /// Links to the specific payment tied to this transaction.
        /// </summary>
        public Guid? PaymentID { get; set; }

        [ForeignKey(nameof(PaymentID))]
        public Payment? Payment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}