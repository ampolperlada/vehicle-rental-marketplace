
namespace VehicleRentalMarketplace.Api.Dtos.Purchase
{
    public class PurchaseRequest
    {
        public int AssetId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
    }
}