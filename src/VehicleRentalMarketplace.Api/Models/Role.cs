using System.ComponentModel.DataAnnotations;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Role
    {
        [Key]
        public Guid RoleID {get; set;} = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string RoleName {get; set;} = string.Empty;
    }
}