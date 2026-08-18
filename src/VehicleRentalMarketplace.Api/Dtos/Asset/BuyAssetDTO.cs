namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class BuyAssetDTO
    {
        public int Quantity { get; set; } = 1;
        public string? DeliveryOption { get; set; }
    }
}