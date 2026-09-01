using VehicleRentalMarketplace.Api.Dtos.Booking;

namespace VehicleRentalMarketplace.Api.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingListResponse>> GetAllBookingsAsync();
        Task<IEnumerable<BookingListResponse>> GetMyBookingsAsync(int userId);
        Task<IEnumerable<BookingListResponse>> GetBookingsByAssetAsync(int assetId, int userId);
        Task<BookingResponse> GetBookingByIdAsync(int id, int userId);
        Task<BookingResponse> CreateBookingAsync(int userId, BookingRequest request);
        Task<BookingResponse> CancelBookingAsync(int id, int userId, string? reason);
        Task<IEnumerable<BookingListResponse>> GetMyAssetBookingsAsync(int userId);
    }
}