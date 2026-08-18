using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Dtos.Auth;
using VehicleRentalMarketplace.Data;
using VehicleRentalMarketplace.Models;
using VehicleRentalMarketplace.Services;

namespace VehicleRentalMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(AuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = await _authService.Authenticate(loginDto.Username, loginDto.Password);

                if (user == null)
                    return Unauthorized(new { message = "Invalid username or password" });

                var response = new AuthResponseDTO
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email ?? "",
                    Role = user.Role?.RoleName ?? "User",
                    Token = user.Token ?? "",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    IsActive = user.IsActive
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                // ... validation code ...

                var user = await _authService.Register(registerDto);

                var response = new AuthResponseDTO
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email ?? "",
                    Role = user.Role?.RoleName ?? "User",
                    Token = user.Token ?? "",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    IsActive = user.IsActive
                };

                // Change this line
                return Ok(response);  // Instead of CreatedAtAction
                                      // OR add the GetUserById method above
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Add this at the bottom of your AuthController class
        [HttpGet("users/{id}")]
        private async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
                return NotFound();

            return user;
        }
    }
}