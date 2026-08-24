//using Microsoft.EntityFrameworkCore;
//using VehicleRentalMarketplace.Api.Data;
//using VehicleRentalMarketplace.Api.Models;
//using VehicleRentalMarketplace.Api.Services.Interfaces; 

//namespace VehicleRentalMarketplace.Api.Services
//{
//    public class AssetService : IAssetService
//    {
//        private readonly ApplicationDbContext _context;

//        public AssetService(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
//        {
//            return await _context.Assets
//                .Include(a => a.UserID)
//                .AsNoTracking()
//                .ToListAsync();
//        }
//        public async Task<Asset?> GetAssetByIdAsync(Guid assetId)
//        {
//            return await _context.Assets
//            .Include(a => a.UserID)
//            .FirstOrDefaultAsync(a => a.AssetID == assetId);
//        }
//        public async Task<Asset> CreateAssetAsync(Asset asset)
//        {
//            asset.AssetID = Guid.NewGuid();
//            asset.CreatedAt = DateTime.UtcNow;

//            _context.Assets.Add(asset);
//            await _context.SaveChangesAsync();

//            return asset;
//        }

//        public async Task<bool> UpdateAssetAsync(Asset asset)
//        {
//            var existingAsset = await _context.Assets.FindAsync(asset.AssetID);
//            if (existingAsset == null) return false;

//            _context.Entry(existingAsset).CurrentValues.SetValues(asset);
//            await _context.SaveChangesAsync();

//            return true;
//        }
//        public async Task<bool> DeleteAssetAsync(Guid assetId)
//        {
//            var asset = await _context.Assets.FindAsync(assetId);
//            if(asset == null) return false;

//            _context.Assets.Remove(asset);
//            await _context.SaveChangesAsync();

//            return true;
//        }
//    }
//}