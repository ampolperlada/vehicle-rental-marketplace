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
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.IsActive && a.IsAvailable)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assets.Select(a => new AssetListResponse
            {
                AssetID = a.AssetID,
                Title = a.Title,
                CategoryName = a.Category?.Name ?? string.Empty,
                ListingTypeName = a.ListingType?.Name ?? string.Empty,
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
                .Include(a => a.Category)
                .Include(a => a.ListingType)
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
                CategoryId = asset.CategoryId,
                CategoryName = asset.Category?.Name ?? string.Empty,
                ListingTypeId = asset.ListingTypeId,
                ListingTypeName = asset.ListingType?.Name ?? string.Empty,
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
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.UserID == userId && a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assets.Select(a => new AssetListResponse
            {
                AssetID = a.AssetID,
                Title = a.Title,
                CategoryName = a.Category?.Name ?? string.Empty,
                ListingTypeName = a.ListingType?.Name ?? string.Empty,
                DailyRate = a.DailyRate,
                SalePrice = a.SalePrice,
                Location = a.Location,
                IsAvailable = a.IsAvailable,
                OwnerName = $"{a.Owner?.Firstname} {a.Owner?.Lastname}".Trim(),
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<IEnumerable<AssetListResponse>> GetAssetsByUserAsync(int userId)
        {
            var assets = await _context.Assets
                .Include(a => a.Owner)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.UserID == userId && a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assets.Select(a => new AssetListResponse
            {
                AssetID = a.AssetID,
                Title = a.Title,
                CategoryName = a.Category?.Name ?? string.Empty,
                ListingTypeName = a.ListingType?.Name ?? string.Empty,
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

            // Verify Category exists
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                throw new Exception("Invalid Category. Please select a valid category.");

            // Verify ListingType exists
            var listingType = await _context.ListingTypes.FindAsync(request.ListingTypeId);
            if (listingType == null)
                throw new Exception("Invalid ListingType. Please select a valid listing type.");

            var asset = new Asset
            {
                UserID = userId,
                Title = request.Title,
                Description = request.Description,
                CategoryId = request.CategoryId,
                ListingTypeId = request.ListingTypeId,
                DailyRate = request.DailyRate,
                SalePrice = request.SalePrice,
                Location = request.Location,
                IsAvailable = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            var createdAsset = await _context.Assets
                .Include(a => a.Owner)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == asset.AssetID);

            return MapToAssetResponse(createdAsset!);
        }

        public async Task<AssetResponse> UpdateAssetAsync(int id, int userId, AssetRequest request)
        {
            var asset = await _context.Assets
                .Include(a => a.Owner)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id && a.IsActive);

            if (asset == null)
                throw new Exception("Asset not found");

            if (asset.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not the owner of this asset");
            }

            ValidateAssetRequest(request);

            // Verify Category exists
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                throw new Exception("Invalid Category. Please select a valid category.");

            // Verify ListingType exists
            var listingType = await _context.ListingTypes.FindAsync(request.ListingTypeId);
            if (listingType == null)
                throw new Exception("Invalid ListingType. Please select a valid listing type.");

            asset.Title = request.Title;
            asset.Description = request.Description;
            asset.CategoryId = request.CategoryId;
            asset.ListingTypeId = request.ListingTypeId;
            asset.DailyRate = request.DailyRate;
            asset.SalePrice = request.SalePrice;
            asset.Location = request.Location;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedAsset = await _context.Assets
                .Include(a => a.Owner)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id);

            return MapToAssetResponse(updatedAsset!);
        }

        public async Task<bool> DeleteAssetAsync(int id, int userId)
        {
            var asset = await _context.Assets
                .FirstOrDefaultAsync(a => a.AssetID == id && a.IsActive);

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
                .Include(a => a.Owner)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id);

            if (asset == null)
                throw new Exception("Asset not found");

            if (asset.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not authorized to restore this asset");
            }

            asset.IsActive = true;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var restoredAsset = await _context.Assets
                .Include(a => a.Owner)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id);

            return MapToAssetResponse(restoredAsset!);
        }

        private void ValidateAssetRequest(AssetRequest request)
        {
            // Check if CategoryId is valid (will be validated in database)
            if (request.CategoryId <= 0)
                throw new Exception("CategoryId is required.");

            // Check if ListingTypeId is valid (will be validated in database)
            if (request.ListingTypeId <= 0)
                throw new Exception("ListingTypeId is required.");

            // Validate based on ListingTypeId (checking via database will be done later)
            // For now, we check the request values
            var listingType = _context.ListingTypes.Find(request.ListingTypeId);
            if (listingType == null)
                throw new Exception("Invalid ListingType.");

            if ((listingType.Name == "Rent" || listingType.Name == "Both") && (!request.DailyRate.HasValue || request.DailyRate <= 0))
                throw new Exception("DailyRate is required and must be greater than 0 for Rent or Both");

            if ((listingType.Name == "Sale" || listingType.Name == "Both") && (!request.SalePrice.HasValue || request.SalePrice <= 0))
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
                CategoryId = asset.CategoryId,
                CategoryName = asset.Category?.Name ?? string.Empty,
                ListingTypeId = asset.ListingTypeId,
                ListingTypeName = asset.ListingType?.Name ?? string.Empty,
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