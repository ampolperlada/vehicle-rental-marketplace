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
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.CategoryId,
                    c.Name,
                    c.Description
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/categories/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == id && c.IsActive)
                .Select(c => new
                {
                    c.CategoryId,
                    c.Name,
                    c.Description
                })
                .FirstOrDefaultAsync();

            if (category == null)
                return NotFound(new { message = "Category not found" });

            return Ok(category);
        }

        // POST: api/categories (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Category name is required" });

            var existing = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == request.Name);

            if (existing != null)
                return BadRequest(new { message = "Category already exists" });

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, new
            {
                category.CategoryId,
                category.Name,
                category.Description
            });
        }

        // PUT: api/categories/{id} (Admin only)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.IsActive);

            if (category == null)
                return NotFound(new { message = "Category not found" });

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var existing = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == request.Name && c.CategoryId != id);

                if (existing != null)
                    return BadRequest(new { message = "Category name already exists" });

                category.Name = request.Name;
            }

            if (request.Description != null)
                category.Description = request.Description;

            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                category.CategoryId,
                category.Name,
                category.Description
            });
        }

        // DELETE: api/categories/{id} (Admin only)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.IsActive);

            if (category == null)
                return NotFound(new { message = "Category not found" });

            // Check if category is being used
            var isUsed = await _context.Assets.AnyAsync(a => a.CategoryId == id && a.IsActive);
            if (isUsed)
                return BadRequest(new { message = "Cannot delete category that is in use by assets" });

            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Category deleted successfully" });
        }

        // PUT: api/categories/{id}/restore (Admin only)
        [HttpPut("{id:int}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && !c.IsActive);

            if (category == null)
                return NotFound(new { message = "Category not found or already active" });

            category.IsActive = true;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category restored successfully",
                category.CategoryId,
                category.Name,
                category.Description
            });
        }
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}