using System.ComponentModel.DataAnnotations;

namespace VehicleRentalMarketplace.Api.Models
{
    public class ListingType : BaseModel
    {
        [Key]
        public int ListingTypeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}