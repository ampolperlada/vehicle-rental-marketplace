namespace VehicleRentalMarketplace.Api.Dtos.User
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }  // If you have this
        public string? RoleName { get; set; }  // Instead of full Role object
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}