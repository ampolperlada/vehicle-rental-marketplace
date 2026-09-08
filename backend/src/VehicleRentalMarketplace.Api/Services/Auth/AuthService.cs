using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Dtos.Auth;
using VehicleRentalMarketplace.Api.Helpers;
using VehicleRentalMarketplace.Api.Models;

namespace VehicleRentalMarketplace.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            // Check if email exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("Email already registered");

            // Check if username exists
            var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUsername != null)
                throw new Exception("Username already taken");

            // Get default role (Customer)
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer");
            if (defaultRole == null)
                throw new Exception("Default role not found");

            // Create user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = PasswordHelper.HashPassword(request.Password),
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                State = request.State,
                RoleID = defaultRole.RoleID,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Get user with role
            var userWithRole = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == user.UserID);

            // Return RegisterResponse WITHOUT token
            return new RegisterResponse
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Role = userWithRole!.Role.RoleName,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // Find user
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                throw new Exception("Invalid username or password");

            if (!user.IsActive)
                throw new Exception("Account is deactivated");

            if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
                throw new Exception("Invalid username or password");

            // Generate token (ONLY in login)
            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role.RoleName,
                UserID = user.UserID,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}