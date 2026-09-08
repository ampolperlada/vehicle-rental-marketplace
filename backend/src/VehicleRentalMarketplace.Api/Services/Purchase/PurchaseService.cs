using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Models;
using VehicleRentalMarketplace.Api.Services.Interfaces;

namespace VehicleRentalMarketplace.Api.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ApplicationDbContext _context;

        public PurchaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchasesAsync()
        {
            return await _context.Purchases
                .Include(p => p.Asset)
                .Include(p => p.Buyer)
                .ToListAsync();
        }

        public async Task<Purchase?> GetPurchaseByIdAsync(int id)
        {
            return await _context.Purchases
                .Include(p => p.Asset)
                .Include(p => p.Buyer)
                .FirstOrDefaultAsync(p => p.PurchaseID == id);
        }

        public async Task<Purchase> CreatePurchaseAsync(
            int userId,
            int assetId,
            decimal amountPaid,
            string paymentReference)
        {
            var asset = await _context.Assets
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == assetId);

            if (asset == null)
                throw new Exception("Asset not found.");

            if (asset.ListingType.Name != "Sale")
                throw new Exception("This asset is not available for sale.");

            if (!asset.IsAvailable)
                throw new Exception("This asset is no longer available.");

            if (!asset.SalePrice.HasValue)
                throw new Exception("This asset does not have a sale price.");

            if (amountPaid != asset.SalePrice.Value)
                throw new Exception("Payment amount does not match the sale price.");

            if (string.IsNullOrWhiteSpace(paymentReference))
                throw new Exception("Payment reference is required.");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                asset.IsAvailable = false;

                var purchase = new Purchase
                {
                    AssetID = asset.AssetID,
                    BuyerID = userId,
                    PurchasePrice = asset.SalePrice.Value,
                    Status = "PENDING",
                    Date = DateTime.UtcNow
                };

                _context.Purchases.Add(purchase);

                await _context.SaveChangesAsync();

                var payment = new Payment
                {
                    PurchaseID = purchase.PurchaseID,
                    Amount = amountPaid,
                    PaymentMethod = "Card",
                    PaymentStatus = "Paid",
                    TransactionID = paymentReference,
                    PaidAt = DateTime.UtcNow
                };

                _context.Payments.Add(payment);

                purchase.Status = "COMPLETED";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return purchase;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}