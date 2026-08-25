using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Dtos.Asset;
using VehicleRentalMarketplace.Api.Models;
using VehicleRentalMarketplace.Api.Services.Interfaces;

namespace VehicleRentalMarketplace.Api.Services
{
    public class AssetService : IAssetService
    {
        private readonly ApplicationDbContext _context;

        public AssetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetListResponse>> GetAllAssetsAsync()
        {
            var assets = await _context.Assets
                .Include(a => a.Owner)
                .Where(a => a.IsActive && a.IsAvailable)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assets.Select(a => new AssetListResponse
            {
                AssetID = a.AssetID,
                Title = a.Title,
                Category = a.Category,
                ListingType = a.ListingType,
                DailyRate = a.DailyRate,
                SalePrice = a.SalePrice,
                Location = a.Location,
                IsAvailable = a.IsAvailable,
                OwnerName = $"{a.Owner?.Firstname} {a.Owner?.Lastname}".Trim(),
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<AssetResponse> GetAssetByIdAsync(int id)
        {
            var asset = await _context.Assets
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(a => a.AssetID == id && a.IsActive);

            if (asset == null)
                throw new Exception("Asset not found");

            return new AssetResponse
            {
                AssetID = asset.AssetID,
                UserID = asset.UserID,
                OwnerName = $"{asset.Owner?.Firstname} {asset.Owner?.Lastname}".Trim(),
                Title = asset.Title,
                Description = asset.Description,
                Category = asset.Category,
                ListingType = asset.ListingType,
                DailyRate = asset.DailyRate,
                SalePrice = asset.SalePrice,
                Location = asset.Location,
                IsAvailable = asset.IsAvailable,
                CreatedAt = asset.CreatedAt,
                UpdatedAt = asset.UpdatedAt
            };
        }

        public async Task<IEnumerable<AssetListResponse>> GetMyAssetsAsync(int userId)
        {
            var assets = await _context.Assets
                .Include(a => a.Owner)
                .Where(a => a.UserID == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assets.Select(a => new AssetListResponse
            {
                AssetID = a.AssetID,
                Title = a.Title,
                Category = a.Category,
                ListingType = a.ListingType,
                DailyRate = a.DailyRate,
                SalePrice = a.SalePrice,
                Location = a.Location,
                IsAvailable = a.IsAvailable,
                IsActive = a.IsActive,
                OwnerName = $"{a.Owner?.Firstname} {a.Owner?.Lastname}".Trim(),
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<IEnumerable<AssetListResponse>> GetAssetsByUserAsync(int userId)
        {
            var assets = await _context.Assets
                .Include(a => a.Owner)
                .Where(a => a.UserID == userId && a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assets.Select(a => new AssetListResponse
            {
                AssetID = a.AssetID,
                Title = a.Title,
                Category = a.Category,
                ListingType = a.ListingType,
                DailyRate = a.DailyRate,
                SalePrice = a.SalePrice,
                Location = a.Location,
                IsAvailable = a.IsAvailable,
                OwnerName = $"{a.Owner?.Firstname} {a.Owner?.Lastname}".Trim(),
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<AssetResponse> CreateAssetAsync(int userId, AssetRequest request)
        {
            ValidateAssetRequest(request);

            var asset = new Asset
            {
                UserID = userId,
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                ListingType = request.ListingType,
                DailyRate = request.DailyRate,
                SalePrice = request.SalePrice,
                Location = request.Location,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            var createdAsset = await _context.Assets
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(a => a.AssetID == asset.AssetID);

            return MapToAssetResponse(createdAsset!);
        }

        public async Task<AssetResponse> UpdateAssetAsync(int id, int userId, AssetRequest request)
        {
            var asset = await _context.Assets
                .FirstOrDefaultAsync(a => a.AssetID == id);

            if (asset == null)
                throw new Exception("Asset not found");

            if (asset.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not the owner of this asset");
            }

            ValidateAssetRequest(request);

            asset.Title = request.Title;
            asset.Description = request.Description;
            asset.Category = request.Category;
            asset.ListingType = request.ListingType;
            asset.DailyRate = request.DailyRate;
            asset.SalePrice = request.SalePrice;
            asset.Location = request.Location;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedAsset = await _context.Assets
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(a => a.AssetID == id);

            return MapToAssetResponse(updatedAsset!);
        }

        public async Task<bool> DeleteAssetAsync(int id, int userId)
        {
            var asset = await _context.Assets
                .FirstOrDefaultAsync(a => a.AssetID == id);

            if (asset == null)
                throw new Exception("Asset not found");

            if (asset.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not the owner of this asset");
            }

            asset.IsActive = false;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AssetResponse> RestoreAssetAsync(int id, int userId)
        {
            var asset = await _context.Assets
                .FirstOrDefaultAsync(a => a.AssetID == id);

            if (asset == null)
                throw new Exception("Asset not found");

            // Check ownership or admin
            if (asset.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not authorized to restore this asset");
            }

            // Restore
            asset.IsActive = true;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var restoredAsset = await _context.Assets
                .Include(a => a.Owner)
                .FirstOrDefaultAsync(a => a.AssetID == id);

            return MapToAssetResponse(restoredAsset!);
        }

        private void ValidateAssetRequest(AssetRequest request)
        {
            if (!new[] { "Rent", "Sale", "Both" }.Contains(request.ListingType))
                throw new Exception("ListingType must be 'Rent', 'Sale', or 'Both'");

            if ((request.ListingType == "Rent" || request.ListingType == "Both") && (!request.DailyRate.HasValue || request.DailyRate <= 0))
                throw new Exception("DailyRate is required and must be greater than 0 for Rent or Both");

            if ((request.ListingType == "Sale" || request.ListingType == "Both") && (!request.SalePrice.HasValue || request.SalePrice <= 0))
                throw new Exception("SalePrice is required and must be greater than 0 for Sale or Both");
        }

        private AssetResponse MapToAssetResponse(Asset asset)
        {
            return new AssetResponse
            {
                AssetID = asset.AssetID,
                UserID = asset.UserID,
                OwnerName = $"{asset.Owner?.Firstname} {asset.Owner?.Lastname}".Trim(),
                Title = asset.Title,
                Description = asset.Description,
                Category = asset.Category,
                ListingType = asset.ListingType,
                DailyRate = asset.DailyRate,
                SalePrice = asset.SalePrice,
                Location = asset.Location,
                IsAvailable = asset.IsAvailable,
                CreatedAt = asset.CreatedAt,
                UpdatedAt = asset.UpdatedAt
            };
        }
    }
}