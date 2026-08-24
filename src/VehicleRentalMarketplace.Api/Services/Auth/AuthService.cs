using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Data;
using VehicleRentalMarketplace.Models;
using VehicleRentalMarketplace.Api.Dtos.Auth;

namespace VehicleRentalMarketplace.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public AuthService(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            // Generate token but DON'T save to database
            // Just return the user with the token
            user.Token = _tokenService.GenerateToken(user);

            // Remove this line - don't save token to DB
            // await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Register(RegisterDTO registerDto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username);

            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            int roleId = registerDto.RoleID ?? 2;

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
                throw new InvalidOperationException("Invalid role");

            var user = new User
            {
                Username = registerDto.Username,
                // This line calls HashPassword - make sure it exists!
                PasswordHash = HashPassword(registerDto.Password),
                Firstname = registerDto.Firstname,
                Lastname = registerDto.Lastname,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                City = registerDto.City,
                State = registerDto.State,
                RoleID = roleId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Token = _tokenService.GenerateToken(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> Logout(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.Token = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByToken(string token)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Token == token);
        }

        private string HashPassword(string password)
        {
            // For development - simple Base64 encoding
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        /// Verifies a password against a stored hash
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            // For development - compare Base64 encoded values
            return HashPassword(password) == hash;
        }
    }
}
