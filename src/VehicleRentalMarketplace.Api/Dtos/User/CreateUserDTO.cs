using System.ComponentModel.DataAnnotations;

namespace VehicleRentalMarketplace.Api.Dtos.User
{
    public class CreateUserDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public int RoleID { get; set; } = 2; // Default to "User" role
    }
}