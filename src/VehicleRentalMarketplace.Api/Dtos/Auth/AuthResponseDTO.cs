namespace VehicleRentalMarketplace.Api.Dtos.Auth
{
    public class AuthResponseDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}