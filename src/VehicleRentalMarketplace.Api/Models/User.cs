//pim
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class User
    {
        [Key]
        public Guid UserID {get; set;}  = Guid.NewGuid();

        [Required]
        public Guid RoleID {get; set;}

        [ForeignKey(nameof(RoleID))]
        public Role Role {get; set;} = null!;

        [Required]
        [MaxLength(50)]
        public string Username {get; set;} = string.Empty;

        [Required]
        public string Password {get; set;} = string.Empty;

        public string? Token {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;} = string.Empty;

        public string Firstname {get; set;} = string.Empty;
        public string Lasname {get; set;} = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address {get; set;} = string.Empty;

        public string City {get; set;} = string.Empty;

        public string State {get; set;} = string.Empty;

        public bool isActive {get; set;} = true;
        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
        public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;

        
    }
}