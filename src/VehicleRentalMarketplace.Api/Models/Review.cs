using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [Required]
        public int AssetID { get; set; }

        [ForeignKey(nameof(AssetID))]
        public Asset Asset { get; set; } = null!;

        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; } = null!;

        //// review can happen after this 

        public int? BookingID { get; set; }

        [ForeignKey(nameof(BookingID))]
        public Booking? Booking { get; set; }
        
        // review can happen after this 
        public int? PurchaseID { get; set; }

        [ForeignKey(nameof(PurchaseID))]
        public Purchase? Purchase { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}