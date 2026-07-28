using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Api.Models
{
 public class Booking
    {
        [Key]
        public Guid BookingID { get; set;} = Guid.NewGuid();

        //fk 1: asset

        [Required]
        public Guid AssetID {get; set;}

        [ForeignKey(nameof(AssetID))]
        public Asset Asset {get; set;} = null!;

        //fk 2 renter / user 
        [Required]
        public Guid RenderID {get; set;}

        [ForeignKey(nameof(RenterID))]
        public User Renter {get; set;} = null!;

        [Required]
        public DateTime StartDate {get; set;}

        [Required]
        public DateTime EndDate {get; set;}

        public int NumberofDays {get; set;}

        [Column(TypeName ="decimal(18,2)")]
        public decimal DailyRate {get; set;}

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRentalPrice {get; set;}

        //Pending, Approve, Paid, Active, Completed, Completed, Rejected

        public string Status {get; set;} = "Pending";

        public string? RejectionReason {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    }   
}