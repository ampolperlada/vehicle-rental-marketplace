    using VehicleRentalMarketplace.Api.Models;

namespace VehicleRentalMarketplace.Api.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<Purchase>> GetAllPurchasesAsync();

        Task<Purchase?> GetPurchaseByIdAsync(int id);

        Task<Purchase> CreatePurchaseAsync( int userId, int assetId, decimal amountPaid, string paymentReference);
        
        // the amountPaid is Asset.SalePrice 
    }
}