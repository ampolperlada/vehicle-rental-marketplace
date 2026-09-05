using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Models;

namespace VehicleRentalMarketplace.Api.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Review>> GetReviewsByAssetAsync(int assetId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.AssetID == assetId)
                .ToListAsync();
        }

        public async Task<Models.Review> CreateReviewAsync(
            int userId,
            int assetId,
            int? bookingId,
            int? purchaseId,
            int rating,
            string comment)
        {
            if (rating < 1 || rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            if (string.IsNullOrWhiteSpace(comment))
                throw new Exception("Comment is required.");

            var asset = await _context.Assets
                .FirstOrDefaultAsync(a => a.AssetID == assetId);

            if (asset == null)
                throw new Exception("Asset not found.");

            if (bookingId.HasValue)
            {
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b =>
                        b.BookingID == bookingId.Value &&
                        b.UserID == userId &&
                        b.AssetID == assetId &&
                        b.Status == "FullyPaid");

                if (booking == null)
                    throw new Exception("You cannot review this booking.");
            }
            else if (purchaseId.HasValue)
            {
                var purchase = await _context.Purchases
                    .FirstOrDefaultAsync(p =>
                        p.PurchaseID == purchaseId.Value &&
                        p.BuyerID == userId &&
                        p.AssetID == assetId &&
                        p.Status == "COMPLETED");

                if (purchase == null)
                    throw new Exception("You cannot review this purchase.");
            }
            else
            {
                throw new Exception("A booking or purchase is required.");
            }

            var review = new Models.Review
            {
                AssetID = assetId,
                UserID = userId,
                BookingID = bookingId,
                PurchaseID = purchaseId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review;
        }
    }
}