using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Models;

namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ListingTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ListingTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/listingtypes
        [HttpGet]
        public async Task<IActionResult> GetListingTypes()
        {
            var listingTypes = await _context.ListingTypes
                .Where(l => l.IsActive)
                .OrderBy(l => l.Name)
                .Select(l => new
                {
                    l.ListingTypeId,
                    l.Name,
                    l.Description
                })
                .ToListAsync();

            return Ok(listingTypes);
        }

        // GET: api/listingtypes/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetListingType(int id)
        {
            var listingType = await _context.ListingTypes
                .Where(l => l.ListingTypeId == id && l.IsActive)
                .Select(l => new
                {
                    l.ListingTypeId,
                    l.Name,
                    l.Description
                })
                .FirstOrDefaultAsync();

            if (listingType == null)
                return NotFound(new { message = "Listing type not found" });

            return Ok(listingType);
        }

        // POST: api/listingtypes (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateListingType([FromBody] CreateListingTypeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Listing type name is required" });

            var existing = await _context.ListingTypes
                .FirstOrDefaultAsync(l => l.Name == request.Name);

            if (existing != null)
                return BadRequest(new { message = "Listing type already exists" });

            var listingType = new ListingType
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ListingTypes.Add(listingType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetListingType), new { id = listingType.ListingTypeId }, new
            {
                listingType.ListingTypeId,
                listingType.Name,
                listingType.Description
            });
        }

        // PUT: api/listingtypes/{id} (Admin only)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateListingType(int id, [FromBody] UpdateListingTypeRequest request)
        {
            var listingType = await _context.ListingTypes
                .FirstOrDefaultAsync(l => l.ListingTypeId == id && l.IsActive);

            if (listingType == null)
                return NotFound(new { message = "Listing type not found" });

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var existing = await _context.ListingTypes
                    .FirstOrDefaultAsync(l => l.Name == request.Name && l.ListingTypeId != id);

                if (existing != null)
                    return BadRequest(new { message = "Listing type name already exists" });

                listingType.Name = request.Name;
            }

            if (request.Description != null)
                listingType.Description = request.Description;

            listingType.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                listingType.ListingTypeId,
                listingType.Name,
                listingType.Description
            });
        }

        // DELETE: api/listingtypes/{id} (Admin only)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteListingType(int id)
        {
            var listingType = await _context.ListingTypes
                .FirstOrDefaultAsync(l => l.ListingTypeId == id && l.IsActive);

            if (listingType == null)
                return NotFound(new { message = "Listing type not found" });

            // Check if listing type is being used
            var isUsed = await _context.Assets.AnyAsync(a => a.ListingTypeId == id && a.IsActive);
            if (isUsed)
                return BadRequest(new { message = "Cannot delete listing type that is in use by assets" });

            listingType.IsActive = false;
            listingType.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Listing type deleted successfully" });
        }

        // PUT: api/listingtypes/{id}/restore (Admin only)
        [HttpPut("{id:int}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreListingType(int id)
        {
            var listingType = await _context.ListingTypes
                .FirstOrDefaultAsync(l => l.ListingTypeId == id && !l.IsActive);

            if (listingType == null)
                return NotFound(new { message = "Listing type not found or already active" });

            listingType.IsActive = true;
            listingType.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Listing type restored successfully",
                listingType.ListingTypeId,
                listingType.Name,
                listingType.Description
            });
        }
    }

    public class CreateListingTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateListingTypeRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}