using VehicleRentalMarketplace.Api.Dtos.Auth;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);  
    Task<AuthResponse> LoginAsync(LoginRequest request);
}