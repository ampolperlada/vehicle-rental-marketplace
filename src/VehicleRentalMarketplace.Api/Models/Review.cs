using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Review
    {
        [Key]
        public Guid ReviewID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AssetID { get; set; }

        [ForeignKey(nameof(AssetID))]
        public Asset Asset { get; set; } = null!;

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

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}