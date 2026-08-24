namespace VehicleRentalMarketplace.Api.Dtos.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int UserID { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
