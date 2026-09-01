using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Dtos.Booking;
using VehicleRentalMarketplace.Api.Services.Interfaces;

namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ApplicationDbContext _context;

        public BookingsController(IBookingService bookingService, ApplicationDbContext context)
        {
            _bookingService = bookingService;
            _context = context;
        }

        // GET: api/bookings (Admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // GET: api/bookings/my-bookings (Renter only)
        [HttpGet("my-bookings")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> GetMyBookings()
        {
            try
            {
                var userId = GetUserId();
                var bookings = await _bookingService.GetMyBookingsAsync(userId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/bookings/asset/{assetId} (Owner/Admin only)
        [HttpGet("asset/{assetId:int}")]
        public async Task<IActionResult> GetBookingsByAsset(int assetId)
        {
            try
            {
                var userId = GetUserId();

                var asset = await _context.Assets
                    .FirstOrDefaultAsync(a => a.AssetID == assetId);

                if (asset == null)
                    return NotFound(new { message = "Asset not found" });

                var user = await _context.Users.FindAsync(userId);
                if (asset.UserID != userId && user?.Role?.RoleName != "Admin")
                    return Unauthorized(new { message = "You are not the owner of this asset" });

                var bookings = await _bookingService.GetBookingsByAssetAsync(assetId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/bookings/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var userId = GetUserId();
                var booking = await _bookingService.GetBookingByIdAsync(id);

                var asset = await _context.Assets
                    .FirstOrDefaultAsync(a => a.AssetID == booking.AssetID);

                var user = await _context.Users.FindAsync(userId);

                if (booking.UserID != userId &&
                    asset?.UserID != userId &&
                    user?.Role?.RoleName != "Admin")
                {
                    return Unauthorized(new { message = "You are not authorized to view this booking" });
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/bookings (Renter only)
        [HttpPost]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request)
        {
            try
            {
                var userId = GetUserId();
                var booking = await _bookingService.CreateBookingAsync(userId, request);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingID }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/bookings/{id}/cancel
        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingRequest request)
        {
            try
            {
                var userId = GetUserId();
                var booking = await _bookingService.GetBookingByIdAsync(id);

                var asset = await _context.Assets
                    .FirstOrDefaultAsync(a => a.AssetID == booking.AssetID);

                var user = await _context.Users.FindAsync(userId);

                if (booking.UserID != userId &&
                    asset?.UserID != userId &&
                    user?.Role?.RoleName != "Admin")
                {
                    return Unauthorized(new { message = "You are not authorized to cancel this booking" });
                }

                var result = await _bookingService.CancelBookingAsync(id, userId, request.Reason);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("UserID")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in token");

            return int.Parse(userIdClaim);
        }
    }

    public class CancelBookingRequest
    {
        public string? Reason { get; set; }
    }
}