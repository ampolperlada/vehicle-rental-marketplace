using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalMarketplace.Api.Dtos.Review;
using VehicleRentalMarketplace.Api.Services.Review;

namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateReview(
            [FromBody] ReviewRequest request)
        {
            var userIdClaim = User.FindFirst("UserID");

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            try
            {
                var review = await _reviewService.CreateReviewAsync(
                    userId,
                    request.AssetID,
                    request.BookingID,
                    request.PurchaseID,
                    request.Rating,
                    request.Comment);

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("asset/{assetId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsByAsset(int assetId)
        {
            var reviews = await _reviewService
                .GetReviewsByAssetAsync(assetId);

            return Ok(reviews);
        }
    }
}