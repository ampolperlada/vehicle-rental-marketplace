using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Purchase
    {
        [Key]
        public Guid PurchaseID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AssetID { get; set; }

        [ForeignKey(nameof(AssetID))]
        public Asset Asset { get; set; } = null!;

        [Required]
        public Guid BuyerID { get; set; }

        [ForeignKey(nameof(BuyerID))]
        public User Buyer { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        public string Status { get; set; } = "PENDING"; 
        public string? RejectedReason { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}