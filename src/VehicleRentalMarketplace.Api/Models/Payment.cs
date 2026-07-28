using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; } = Guid.NewGuid();

        public Guid? BookingID { get; set; }

        [ForeignKey(nameof(BookingID))]
        public Booking? Booking { get; set; }

        public Guid? PurchaseID { get; set; }

        [ForeignKey(nameof(PurchaseID))]
        public Purchase? Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Card"; // cash, bank transfer or something
        public string PaymentStatus { get; set; } = "Peding"; // pending, success, failed
        public string TransactionID { get; set; } = string.Empty;

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}