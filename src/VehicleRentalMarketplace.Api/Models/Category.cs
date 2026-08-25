using System.ComponentModel.DataAnnotations;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Category : BaseModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}