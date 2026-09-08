using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseID { get; set; }

        [Required]
        public int AssetID { get; set; }  

        [ForeignKey(nameof(AssetID))]
        public Asset Asset { get; set; } = null!;

        [Required]
        public int BuyerID { get; set; } 

        [ForeignKey(nameof(BuyerID))]
        public User Buyer { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        public string Status { get; set; } = "PENDING";
        public string? RejectedReason { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}