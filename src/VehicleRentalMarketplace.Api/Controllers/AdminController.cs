using Microsoft.AspNetCore.Mvc;
using VehicleRentalMarketplace.Api.Models;
using VehicleRentalMarketplace.Api.Services.Interfaces;

    namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _assetService.GetAllAssetsAsync();
            return Ok(assets);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult>GetById(Guid id)
        {
            var assets = await _assetService.GetAssetByIdAsync(id);
            if (assets == null) return NotFound();

            return Ok(assets);
        }

        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] Asset asset){
            var createdAsset = await _assetService.CreateAssetAsync(asset);
            return CreatedAtAction(nameof(GetById), new { id = createdAsset.AssetID }, createdAsset);
        }

    }
}