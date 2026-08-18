using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Models;
using VehicleRentalMarketplace.Data;
using VehicleRentalMarketplace.Api.Dtos.User;

namespace VehicleRentalMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDTO
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    RoleName = u.Role != null ? u.Role.RoleName : null,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDTO
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    PhoneNumber = u.PhoneNumber,
                    RoleName = u.Role != null ? u.Role.RoleName : null,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDTO)
        {
            // Create a new User entity from the DTO
            var user = new User
            {
                Username = createUserDTO.Username,
                PasswordHash = createUserDTO.Password, // In real app, hash this!
                Firstname = createUserDTO.Firstname,
                Lastname = createUserDTO.Lastname,
                PhoneNumber = createUserDTO.PhoneNumber,
                Address = createUserDTO.Address,
                City = createUserDTO.City,
                State = createUserDTO.State,
                RoleID = createUserDTO.RoleID,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return a DTO, not the full entity
            var userDTO = new UserDTO
            {
                UserID = user.UserID,
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return CreatedAtAction("GetUser", new { id = user.UserID }, userDTO);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDTO updateUserDTO)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update only the fields that can be changed
            user.Firstname = updateUserDTO.Firstname ?? user.Firstname;
            user.Lastname = updateUserDTO.Lastname ?? user.Lastname;
            user.PhoneNumber = updateUserDTO.PhoneNumber ?? user.PhoneNumber;
            user.Address = updateUserDTO.Address ?? user.Address;
            user.City = updateUserDTO.City ?? user.City;
            user.State = updateUserDTO.State ?? user.State;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Soft delete
            user.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}