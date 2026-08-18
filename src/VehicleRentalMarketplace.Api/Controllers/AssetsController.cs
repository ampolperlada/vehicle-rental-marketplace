using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Data;
using VehicleRentalMarketplace.Api.Dtos.Asset;
using VehicleRentalMarketplace.Models;

namespace VehicleRentalMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // GET: api/assets - View all assets (Role-based filtering)
        // ============================================================
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AssetDTO>>> GetAssets()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var assetsQuery = _context.Assets
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.IsActive);

            // Role-based filtering
            if (userRole == "User" || userRole == "Buyer" || userRole == "Renter")
            {
                // Users can only see approved assets
                assetsQuery = assetsQuery.Where(a => a.ApprovalStatus == "Approved");
            }
            else if (userRole == "Seller")
            {
                // Sellers see their own assets + approved assets from others
                assetsQuery = assetsQuery.Where(a =>
                    a.ApprovalStatus == "Approved" ||
                    a.RenterID == userId
                );
            }
            // Admin and Moderator see everything (no filtering)

            var assets = await assetsQuery
                .Select(a => new AssetDTO
                {
                    AssetID = a.AssetID,
                    Title = a.Title,
                    Description = a.Description,
                    Location = a.Location,
                    DailyRate = a.DailyRate,
                    SalePrice = a.SalePrice,
                    ListingType = a.ListingType != null ? a.ListingType.TypeName : "Unknown",
                    CategoryName = a.Category != null ? a.Category.CategoryName : "Unknown",
                    ApprovalStatus = a.ApprovalStatus,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    IsActive = a.IsActive,
                    OwnerName = a.User != null ? $"{a.User.Firstname} {a.User.Lastname}" : "Unknown",
                    OwnerID = a.RenterID,
                    IsOwner = a.RenterID == userId
                })
                .ToListAsync();

            return Ok(assets);
        }

        // ============================================================
        // GET: api/assets/my - Get current user's assets (Sellers/Admin)
        // ============================================================
        [HttpGet("my")]
        [Authorize(Roles = "Seller,Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<AssetDTO>>> GetMyAssets()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userName = User.Identity?.Name ?? "Unknown";

            var assetsQuery = _context.Assets
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.IsActive && a.RenterID == userId);

            // For Admin/Moderator, show all assets (not just their own)
            if (userRole == "Admin" || userRole == "Moderator")
            {
                assetsQuery = _context.Assets
                    .Include(a => a.Category)
                    .Include(a => a.ListingType)
                    .Where(a => a.IsActive);
            }

            var assets = await assetsQuery
                .Select(a => new AssetDTO
                {
                    AssetID = a.AssetID,
                    Title = a.Title,
                    Description = a.Description,
                    Location = a.Location,
                    DailyRate = a.DailyRate,
                    SalePrice = a.SalePrice,
                    ListingType = a.ListingType != null ? a.ListingType.TypeName : "Unknown",
                    CategoryName = a.Category != null ? a.Category.CategoryName : "Unknown",
                    ApprovalStatus = a.ApprovalStatus,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    IsActive = a.IsActive,
                    OwnerName = userName,
                    OwnerID = a.RenterID,
                    IsOwner = a.RenterID == userId
                })
                .ToListAsync();

            return Ok(assets);
        }

        // ============================================================
        // GET: api/assets/{id} - Get single asset
        // ============================================================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AssetDTO>> GetAsset(int id)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var assetQuery = _context.Assets
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.AssetID == id && a.IsActive);

            // Role-based filtering
            if (userRole == "User" || userRole == "Buyer" || userRole == "Renter")
            {
                assetQuery = assetQuery.Where(a => a.ApprovalStatus == "Approved");
            }
            else if (userRole == "Seller")
            {
                assetQuery = assetQuery.Where(a =>
                    a.ApprovalStatus == "Approved" ||
                    a.RenterID == userId
                );
            }
            // Admin and Moderator see everything

            var asset = await assetQuery
                .Select(a => new AssetDTO
                {
                    AssetID = a.AssetID,
                    Title = a.Title,
                    Description = a.Description,
                    Location = a.Location,
                    DailyRate = a.DailyRate,
                    SalePrice = a.SalePrice,
                    ListingType = a.ListingType != null ? a.ListingType.TypeName : "Unknown",
                    CategoryName = a.Category != null ? a.Category.CategoryName : "Unknown",
                    ApprovalStatus = a.ApprovalStatus,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    IsActive = a.IsActive,
                    OwnerName = a.User != null ? $"{a.User.Firstname} {a.User.Lastname}" : "Unknown",
                    OwnerID = a.RenterID,
                    IsOwner = a.RenterID == userId
                })
                .FirstOrDefaultAsync();

            if (asset == null)
            {
                return NotFound(new { message = "Asset not found or not accessible" });
            }

            return Ok(asset);
        }

        // ============================================================
        // GET: api/assets/pending - Get pending assets (Admin/Moderator only)
        // ============================================================
        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<AssetDTO>>> GetPendingAssets()
        {
            var assets = await _context.Assets
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.ApprovalStatus == "Pending" && a.IsActive)
                .Select(a => new AssetDTO
                {
                    AssetID = a.AssetID,
                    Title = a.Title,
                    Description = a.Description,
                    Location = a.Location,
                    DailyRate = a.DailyRate,
                    SalePrice = a.SalePrice,
                    ListingType = a.ListingType != null ? a.ListingType.TypeName : "Unknown",
                    CategoryName = a.Category != null ? a.Category.CategoryName : "Unknown",
                    ApprovalStatus = a.ApprovalStatus,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    IsActive = a.IsActive,
                    OwnerName = a.User != null ? $"{a.User.Firstname} {a.User.Lastname}" : "Unknown",
                    OwnerID = a.RenterID
                })
                .ToListAsync();

            return Ok(assets);
        }

        // ============================================================
        // POST: api/assets - Create asset (Seller/Admin only)
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<ActionResult<AssetDTO>> PostAsset(CreateAssetDTO createAssetDTO)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Validate Category exists
            var category = await _context.Categories.FindAsync(createAssetDTO.CategoryID);
            if (category == null)
            {
                return BadRequest(new { message = "Invalid category" });
            }

            // Validate ListingType exists
            var listingType = await _context.ListingTypes.FindAsync(createAssetDTO.ListingTypeID);
            if (listingType == null)
            {
                return BadRequest(new { message = "Invalid listing type" });
            }

            // Validate required fields based on listing type
            if (listingType.TypeName == "Rent" && !createAssetDTO.DailyRate.HasValue)
            {
                return BadRequest(new { message = "Daily rate is required for rent assets" });
            }

            if (listingType.TypeName == "Sale" && !createAssetDTO.SalePrice.HasValue)
            {
                return BadRequest(new { message = "Sale price is required for sale assets" });
            }

            var asset = new Asset
            {
                RenterID = userId,
                Title = createAssetDTO.Title,
                Description = createAssetDTO.Description,
                CategoryID = createAssetDTO.CategoryID,
                ListingTypeID = createAssetDTO.ListingTypeID,
                DailyRate = createAssetDTO.DailyRate,
                SalePrice = createAssetDTO.SalePrice,
                Location = createAssetDTO.Location,
                ApprovalStatus = "Pending",
                Status = "Available",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            var assetDTO = await _context.Assets
                .Include(a => a.Category)
                .Include(a => a.ListingType)
                .Where(a => a.AssetID == asset.AssetID)
                .Select(a => new AssetDTO
                {
                    AssetID = a.AssetID,
                    Title = a.Title,
                    Description = a.Description,
                    Location = a.Location,
                    DailyRate = a.DailyRate,
                    SalePrice = a.SalePrice,
                    ListingType = a.ListingType != null ? a.ListingType.TypeName : "Unknown",
                    CategoryName = a.Category != null ? a.Category.CategoryName : "Unknown",
                    ApprovalStatus = a.ApprovalStatus,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    IsActive = a.IsActive,
                    OwnerID = a.RenterID,
                    IsOwner = true
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetAsset), new { id = asset.AssetID }, assetDTO);
        }

        // ============================================================
        // PUT: api/assets/{id} - Update asset (Owner/Admin/Moderator)
        // ============================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller,Admin,Moderator")]
        public async Task<IActionResult> PutAsset(int id, UpdateAssetDTO updateAssetDTO)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound(new { message = "Asset not found" });
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // Check permissions: Seller can only edit their own assets
            if (userRole == "Seller" && asset.RenterID != userId)
            {
                return Forbid("You can only edit your own assets");
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateAssetDTO.Title))
                asset.Title = updateAssetDTO.Title;

            if (!string.IsNullOrEmpty(updateAssetDTO.Description))
                asset.Description = updateAssetDTO.Description;

            if (updateAssetDTO.CategoryID.HasValue)
            {
                var category = await _context.Categories.FindAsync(updateAssetDTO.CategoryID.Value);
                if (category == null)
                {
                    return BadRequest(new { message = "Invalid category" });
                }
                asset.CategoryID = updateAssetDTO.CategoryID.Value;
            }

            if (updateAssetDTO.ListingTypeID.HasValue)
            {
                var listingType = await _context.ListingTypes.FindAsync(updateAssetDTO.ListingTypeID.Value);
                if (listingType == null)
                {
                    return BadRequest(new { message = "Invalid listing type" });
                }
                asset.ListingTypeID = updateAssetDTO.ListingTypeID.Value;
            }

            if (updateAssetDTO.DailyRate.HasValue)
                asset.DailyRate = updateAssetDTO.DailyRate.Value;

            if (updateAssetDTO.SalePrice.HasValue)
                asset.SalePrice = updateAssetDTO.SalePrice.Value;

            if (!string.IsNullOrEmpty(updateAssetDTO.Location))
                asset.Location = updateAssetDTO.Location;

            if (!string.IsNullOrEmpty(updateAssetDTO.Status))
                asset.Status = updateAssetDTO.Status;

            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ============================================================
        // PUT: api/assets/approve/{id} - Approve/Reject asset (Admin/Moderator)
        // ============================================================
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> ApproveAsset(int id, [FromBody] bool approved)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound(new { message = "Asset not found" });
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            asset.ApprovalStatus = approved ? "Approved" : "Rejected";
            asset.ApproveBy = userId;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = approved ? "Asset approved" : "Asset rejected",
                asset.AssetID,
                asset.ApprovalStatus
            });
        }

        // ============================================================
        // DELETE: api/assets/{id} - Delete asset (Admin only)
        // ============================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound(new { message = "Asset not found" });
            }

            // Soft delete
            asset.IsActive = false;
            asset.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Asset deleted successfully" });
        }

        // ============================================================
        // POST: api/assets/{id}/book - Book/Rent an asset (User/Buyer)
        // ============================================================
        [HttpPost("{id}/book")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookAsset(int id, [FromBody] BookAssetDTO bookDTO)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            var asset = await _context.Assets
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id && a.IsActive && a.ApprovalStatus == "Approved");

            if (asset == null)
            {
                return NotFound(new { message = "Asset not found or not available" });
            }

            // Check if asset is available for booking
            if (asset.Status != "Available")
            {
                return BadRequest(new { message = "Asset is not available for booking" });
            }

            // Create a purchase/booking record
            var purchase = new Purchase
            {
                BookingID = asset.AssetID,
                PurchasePrice = asset.DailyRate ?? asset.SalePrice ?? 0,
                Date = DateTime.UtcNow,
                UserID = userId
            };

            _context.Purchases.Add(purchase);

            // Update asset status
            asset.Status = "Booked";
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Asset booked successfully",
                purchaseId = purchase.PurchasedID,
                assetId = asset.AssetID
            });
        }

        // ============================================================
        // POST: api/assets/{id}/buy - Buy an asset (User/Buyer)
        // ============================================================
        [HttpPost("{id}/buy")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BuyAsset(int id, [FromBody] BuyAssetDTO buyDTO)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            var asset = await _context.Assets
                .Include(a => a.ListingType)
                .FirstOrDefaultAsync(a => a.AssetID == id && a.IsActive && a.ApprovalStatus == "Approved");

            if (asset == null)
            {
                return NotFound(new { message = "Asset not found or not available" });
            }

            // Check if asset is for sale
            if (asset.ListingType.TypeName != "Sale" && asset.ListingType.TypeName != "Both")
            {
                return BadRequest(new { message = "This asset is not for sale" });
            }

            if (asset.Status != "Available")
            {
                return BadRequest(new { message = "Asset is not available for purchase" });
            }

            // Create purchase record
            var purchase = new Purchase
            {
                BookingID = asset.AssetID,
                PurchasePrice = asset.SalePrice ?? 0,
                Date = DateTime.UtcNow,
                UserID = userId
            };

            _context.Purchases.Add(purchase);

            // Update asset status
            asset.Status = "Sold";
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Asset purchased successfully",
                purchaseId = purchase.PurchasedID,
                assetId = asset.AssetID,
                price = purchase.PurchasePrice
            });
        }
    }
}