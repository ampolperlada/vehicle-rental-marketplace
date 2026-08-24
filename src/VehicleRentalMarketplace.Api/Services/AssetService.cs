using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Models;

namespace VehicleRentalMarketplace.Api.Services
{
    public class AssetService
    {
        private readonly ApplicationDbContext db;

        public AssetService(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        {
            return await db.Assets.ToListAsync();
        }

        public async Task<Asset?> GetAssetByIdAsync(int id)
        {
            return await db.Assets.FirstOrDefaultAsync(a => a.AssetID == id);
        }

        public async Task<Asset> CreateAssetAsync(Asset asset)
        {
            db.Assets.Add(asset);
            await db.SaveChangesAsync();
            return asset;
        }

        public async Task<bool> UpdateAssetAsync(int id, Asset asset)
        {
            if (id != asset.AssetID)
            {
                return false;
            }

            db.Entry(asset).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetExists(id))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAssetAsync(int id)
        {
            var asset = await db.Assets.FindAsync(id);
            if (asset == null)
            {
                return false;
            }

            db.Assets.Remove(asset);
            await db.SaveChangesAsync();
            return true;
        }

        private bool AssetExists(int id)
        {
            return db.Assets.Any(e => e.AssetID == id);
        }
    }
}