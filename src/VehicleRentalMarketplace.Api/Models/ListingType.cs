using System.ComponentModel.DataAnnotations;

namespace VehicleRentalMarketplace.Models
{
    public class ListingType
    {
        [Key]
        public int ListingTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; } = string.Empty; // "Rent", "Sale", "Both"

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}