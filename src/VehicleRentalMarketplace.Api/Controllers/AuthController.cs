using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Dtos.Auth;
using VehicleRentalMarketplace.Api.Helpers;
using VehicleRentalMarketplace.Api.Models;
using VehicleRentalMarketplace.Api.Services.Auth;

namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email already registered" });

            var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUsername != null)
                return BadRequest(new { message = "Username already taken" });

            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer");
            if (customerRole == null)
                return BadRequest(new { message = "Default role not found" });

            var user = new User
            {
                UserID = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Password = PasswordHelper.HashPassword(request.Password),
                Firstname = request.Firstname,
                Lasname = request.Lastname,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                State = request.State,
                RoleID = customerRole.RoleID,
                isActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userWithRole = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == user.UserID);

            var token = _jwtService.GenerateToken(userWithRole!);

            return Ok(new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = userWithRole!.Role.RoleName,
                UserID = user.UserID,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            if (!user.isActive)
                return Unauthorized(new { message = "Account is deactivated" });

            if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
                return Unauthorized(new { message = "Invalid email or password" });

            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role.RoleName,
                UserID = user.UserID,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult TestAuth()
        {
            return Ok(new
            {
                message = "You are authenticated!",
                user = User.Identity?.Name,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }
    }
}