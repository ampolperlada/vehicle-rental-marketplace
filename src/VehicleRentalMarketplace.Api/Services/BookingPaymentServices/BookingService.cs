using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Dtos.Booking;
using VehicleRentalMarketplace.Api.Models;
using VehicleRentalMarketplace.Api.Services.Interfaces;

namespace VehicleRentalMarketplace.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingListResponse>> GetAllBookingsAsync()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Asset)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(MapToBookingListResponse);
        }

        public async Task<IEnumerable<BookingListResponse>> GetMyBookingsAsync(int userId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Asset)
                .Include(b => b.User)
                .Where(b => b.UserID == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(MapToBookingListResponse);
        }

        public async Task<IEnumerable<BookingListResponse>> GetBookingsByAssetAsync(int assetId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Asset)
                .Include(b => b.User)
                .Where(b => b.AssetID == assetId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(MapToBookingListResponse);
        }

        public async Task<BookingResponse> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Asset)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null)
                throw new Exception("Booking not found");

            return MapToBookingResponse(booking);
        }

        public async Task<BookingResponse> CreateBookingAsync(int userId, BookingRequest request)
        {
            // Get asset
            var asset = await _context.Assets
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == request.AssetId && a.IsActive);

            if (asset == null)
                throw new Exception("Asset not found");

            // Check if asset is available for rent
            if (asset.ListingType?.Name != "Rent" && asset.ListingType?.Name != "Both")
                throw new Exception("This asset is not available for rent");

            if (!asset.IsAvailable)
                throw new Exception("Asset is currently not available");

            // Calculate days
            var days = (int)(request.EndDate - request.StartDate).TotalDays;
            if (days <= 0)
                throw new Exception("End date must be after start date");

            // Check if user already has a booking for these dates (SELF)
            var ownBooking = await _context.Bookings
                .AnyAsync(b => b.AssetID == request.AssetId &&
                               b.UserID == userId &&
                               b.Status == "FullyPaid" &&
                               ((request.StartDate >= b.StartDate && request.StartDate < b.EndDate) ||
                                (request.EndDate > b.StartDate && request.EndDate <= b.EndDate) ||
                                (request.StartDate <= b.StartDate && request.EndDate >= b.EndDate)));

            if (ownBooking)
                throw new Exception("You have already booked this asset for the selected dates");

            // Check for date conflicts with OTHER users
            var conflict = await _context.Bookings
                .AnyAsync(b => b.AssetID == request.AssetId &&
                               b.Status == "FullyPaid" &&
                               b.UserID != userId &&  // <-- I-exclude ang sarili
                               ((request.StartDate >= b.StartDate && request.StartDate < b.EndDate) ||
                                (request.EndDate > b.StartDate && request.EndDate <= b.EndDate) ||
                                (request.StartDate <= b.StartDate && request.EndDate >= b.EndDate)));

            if (conflict)
                throw new Exception("Asset is already booked for the selected dates by another user");

            // Compute total price
            var totalPrice = days * (asset.DailyRate ?? 0);

            // Validate payment
            if (request.AmountPaid != totalPrice)
                throw new Exception($"Payment amount must be exactly {totalPrice}. You entered {request.AmountPaid}");

            // Validate payment reference
            if (string.IsNullOrWhiteSpace(request.PaymentReference))
                throw new Exception("Payment reference is required");

            var booking = new Booking
            {
                AssetID = request.AssetId,
                UserID = userId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                NumberofDays = days,
                DailyRate = asset.DailyRate ?? 0,
                TotalRentalPrice = totalPrice,
                Status = "FullyPaid",
                AmountPaid = request.AmountPaid,
                PaymentReference = request.PaymentReference,
                PaymentDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            // Set asset as not available
            asset.IsAvailable = false;
            asset.UpdatedAt = DateTime.UtcNow;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var createdBooking = await _context.Bookings
                .Include(b => b.Asset)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingID == booking.BookingID);

            return MapToBookingResponse(createdBooking!);
        }
        public async Task<BookingResponse> CancelBookingAsync(int id, int userId, string? reason)
        {
            var booking = await _context.Bookings
                .Include(b => b.Asset)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null)
                throw new Exception("Booking not found");

            // Check if user is authorized
            if (booking.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not authorized to cancel this booking");
            }

            if (booking.Status == "Cancelled")
                throw new Exception("Booking is already cancelled");

            if (booking.StartDate < DateTime.UtcNow)
                throw new Exception("Cannot cancel a booking that has already started");

            booking.Status = "Cancelled";
            booking.CancellationReason = reason ?? "User cancelled";
            booking.CancelledAt = DateTime.UtcNow;
            booking.UpdatedAt = DateTime.UtcNow;

            // Set asset back to available
            var asset = await _context.Assets.FindAsync(booking.AssetID);
            if (asset != null)
            {
                asset.IsAvailable = true;
                asset.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            var updatedBooking = await _context.Bookings
                .Include(b => b.Asset)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            return MapToBookingResponse(updatedBooking!);
        }

        private BookingListResponse MapToBookingListResponse(Booking booking)
        {
            return new BookingListResponse
            {
                BookingID = booking.BookingID,
                AssetTitle = booking.Asset?.Title ?? string.Empty,
                UserName = $"{booking.User?.Firstname} {booking.User?.Lastname}".Trim(),
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                NumberofDays = booking.NumberofDays,
                TotalRentalPrice = booking.TotalRentalPrice,
                Status = booking.Status,
                AmountPaid = booking.AmountPaid,
                PaymentReference = booking.PaymentReference,
                CreatedAt = booking.CreatedAt
            };
        }

        private BookingResponse MapToBookingResponse(Booking booking)
        {
            return new BookingResponse
            {
                BookingID = booking.BookingID,
                AssetID = booking.AssetID,
                AssetTitle = booking.Asset?.Title ?? string.Empty,
                UserID = booking.UserID,
                UserName = $"{booking.User?.Firstname} {booking.User?.Lastname}".Trim(),
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                NumberofDays = booking.NumberofDays,
                DailyRate = booking.DailyRate,
                TotalRentalPrice = booking.TotalRentalPrice,
                Status = booking.Status,
                AmountPaid = booking.AmountPaid,
                PaymentReference = booking.PaymentReference,
                PaymentDate = booking.PaymentDate,
                CancellationReason = booking.CancellationReason,
                CancelledAt = booking.CancelledAt,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}