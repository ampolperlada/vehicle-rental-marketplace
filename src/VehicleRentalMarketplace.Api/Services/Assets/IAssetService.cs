using VehicleRentalMarketplace.Api.Dtos.Asset;

namespace VehicleRentalMarketplace.Api.Services.Interfaces
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetListResponse>> GetAllAssetsAsync();
        Task<AssetResponse> GetAssetByIdAsync(int id);
        Task<IEnumerable<AssetListResponse>> GetMyAssetsAsync(int userId);
        Task<IEnumerable<AssetListResponse>> GetAssetsByUserAsync(int userId);
        Task<AssetResponse> CreateAssetAsync(int userId, AssetRequest request);
        Task<AssetResponse> UpdateAssetAsync(int id, int userId, AssetRequest request);
        Task<bool> DeleteAssetAsync(int id, int userId);
        Task<AssetResponse> RestoreAssetAsync(int id, int userId);
    }
}