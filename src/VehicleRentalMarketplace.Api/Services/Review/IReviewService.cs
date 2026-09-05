using VehicleRentalMarketplace.Api.Models;

namespace VehicleRentalMarketplace.Api.Services.Review
{
    public interface IReviewService
    {
        Task<IEnumerable<Models.Review>> GetReviewsByAssetAsync(int assetId);

        Task<Models.Review> CreateReviewAsync(
            int userId,
            int assetId,
            int? bookingId,
            int? purchaseId,
            int rating,
            string comment);
    }
}