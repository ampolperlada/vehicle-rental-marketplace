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
                IsActive = a.IsActive,
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
                .Where(a => a.UserID == userId)
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
                IsActive = a.IsActive,
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
                IsActive = a.IsActive,
                OwnerName = $"{a.Owner?.Firstname} {a.Owner?.Lastname}".Trim(),
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<AssetResponse> CreateAssetAsync(int userId, AssetRequest request)
        {
            ValidateAssetRequest(request);

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                throw new Exception("Invalid Category. Please select a valid category.");

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
            // Get asset without IsActive filter
            var asset = await _context.Assets
                .Include(a => a.Owner)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id);

            // Check if asset exists
            if (asset == null)
                throw new Exception("Asset not found");

            // Check if asset is deleted
            if (!asset.IsActive)
                throw new Exception("Asset is deleted and cannot be updated");

            // Check if user is the owner or admin
            if (asset.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not authorized to update this asset");
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

            // Update fields
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
            // Get asset without IsActive filter para malaman kung deleted na
            var asset = await _context.Assets
                .FirstOrDefaultAsync(a => a.AssetID == id);

            // Check if asset exists
            if (asset == null)
                throw new Exception("Asset not found");

            // Check if already deleted
            if (!asset.IsActive)
                throw new Exception("Asset is already deleted");

            // Check if user is the owner or admin
            if (asset.UserID != userId)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user?.Role?.RoleName != "Admin")
                    throw new Exception("You are not authorized to delete this asset");
            }

            // Soft delete
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

            // Check if already active
            if (asset.IsActive)
                throw new Exception("Asset is already active");

            // Check if user is the owner or admin
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
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id);

            return MapToAssetResponse(restoredAsset!);
        }
        private void ValidateAssetRequest(AssetRequest request)
        {
            if (request.CategoryId <= 0)
                throw new Exception("CategoryId is required.");

            if (request.ListingTypeId <= 0)
                throw new Exception("ListingTypeId is required.");

            var listingType = _context.ListingTypes.Find(request.ListingTypeId);
            if (listingType == null)
                throw new Exception("Invalid ListingType.");

            if (listingType.Name == "Rent")
            {
                if (!request.DailyRate.HasValue || request.DailyRate <= 0)
                    throw new Exception("DailyRate is required and must be greater than 0 for Rent");
                if (request.SalePrice.HasValue)
                    throw new Exception("SalePrice should be null for Rent-only listings");
            }
            else if (listingType.Name == "Sale")
            {
                if (!request.SalePrice.HasValue || request.SalePrice <= 0)
                    throw new Exception("SalePrice is required and must be greater than 0 for Sale");
                if (request.DailyRate.HasValue)
                    throw new Exception("DailyRate should be null for Sale-only listings");
            }
            else
            {
                throw new Exception("ListingType must be 'Rent' or 'Sale'");
            }
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