namespace VehicleRentalMarketplace.Api.Dtos.User
{
    public class UpdateUserDTO
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}