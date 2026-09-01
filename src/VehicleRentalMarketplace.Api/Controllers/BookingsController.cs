using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("my-asset-bookings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMyAssetBookings()
        {
            try
            {
                var userId = GetUserId();
                var bookings = await _bookingService.GetMyAssetBookingsAsync(userId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("asset/{assetId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookingsByAsset(int assetId)
        {
            try
            {
                var userId = GetUserId();
                var bookings = await _bookingService.GetBookingsByAssetAsync(assetId, userId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-bookings")]
        [Authorize(Roles = "Customer")]
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

        [HttpPost]
        [Authorize(Roles = "Customer")]
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var userId = GetUserId();
                var booking = await _bookingService.GetBookingByIdAsync(id, userId);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingRequest request)
        {
            try
            {
                var userId = GetUserId();
                var booking = await _bookingService.CancelBookingAsync(id, userId, request.Reason);
                return Ok(booking);
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