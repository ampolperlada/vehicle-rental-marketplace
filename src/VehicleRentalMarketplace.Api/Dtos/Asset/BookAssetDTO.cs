namespace VehicleRentalMarketplace.Api.Dtos.Asset
{
    public class BookAssetDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Quantity { get; set; } = 1;
    }
}