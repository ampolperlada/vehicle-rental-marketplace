using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VehicleRentalMarketplace.Api.Dtos.Asset;
using VehicleRentalMarketplace.Api.Services.Interfaces;

namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        // GET: api/assets
        [HttpGet]
        public async Task<IActionResult> GetAssets()
        {
            var assets = await _assetService.GetAllAssetsAsync();
            return Ok(assets);
        }

        // GET: api/assets/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsset(int id)
        {
            try
            {
                var asset = await _assetService.GetAssetByIdAsync(id);
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/assets/my-assets
        [HttpGet("my-assets")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMyAssets()
        {
            try
            {
                var userId = GetUserId();
                var assets = await _assetService.GetMyAssetsAsync(userId);
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/assets/user/{userId}
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetAssetsByUser(int userId)
        {
            var assets = await _assetService.GetAssetsByUserAsync(userId);
            return Ok(assets);
        }

        // POST: api/assets
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsset([FromBody] AssetRequest request)
        {
            try
            {
                var userId = GetUserId();
                var asset = await _assetService.CreateAssetAsync(userId, request);
                return CreatedAtAction(nameof(GetAsset), new { id = asset.AssetID }, asset);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/assets/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] AssetRequest request)
        {
            try
            {
                var userId = GetUserId();
                var asset = await _assetService.UpdateAssetAsync(id, userId, request);
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/assets/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            try
            {
                var userId = GetUserId();
                await _assetService.DeleteAssetAsync(id, userId);
                return Ok(new { message = "Asset deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // RESTORE: api/assets/{id}/restore
        [HttpPut("{id:int}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreAsset(int id)
        {
            try
            {
                var userId = GetUserId();
                var asset = await _assetService.RestoreAssetAsync(id, userId);
                return Ok(new { message = "Asset restored successfully", asset });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("UserID")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in token");

            return int.Parse(userIdClaim);
        }
    }
}