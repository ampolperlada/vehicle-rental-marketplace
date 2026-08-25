using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        public int? BookingID { get; set; }

        [ForeignKey(nameof(BookingID))]
        public Booking? Booking { get; set; }

        public int? PurchaseID { get; set; }

        [ForeignKey(nameof(PurchaseID))]
        public Purchase? Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Card";
        public string PaymentStatus { get; set; } = "Pending";
        public string TransactionID { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}