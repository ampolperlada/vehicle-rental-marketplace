using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleRentalMarketplace.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty; // Store hashed password

        [StringLength(3000)]
        public string? Token { get; set; }

        [Required]
        [StringLength(100)]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Lastname { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; } // Add email field

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Role? Role { get; set; }
        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}