using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalMarketplace.Api.Dtos.Purchase;
using VehicleRentalMarketplace.Api.Services.Interfaces;

namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreatePurchase(
            [FromBody] PurchaseRequest request)
        {
            var userIdClaim = User.FindFirst("UserID");

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            try
            {
                var purchase = await _purchaseService.CreatePurchaseAsync(
                    userId,
                    request.AssetId,
                    request.AmountPaid,
                    request.PaymentReference);

                return Ok(purchase);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();

            return Ok(purchases);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);

            if (purchase == null)
                return NotFound(new
                {
                    message = "Purchase not found."
                });

            return Ok(purchase);
        }
    }
}