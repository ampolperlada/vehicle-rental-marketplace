using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalMarketplace.Api.Data;
using VehicleRentalMarketplace.Api.Models;

namespace VehicleRentalMarketplace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public PaymentsController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await db.Payments.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await db.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Payments.Add(payment);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentID }, payment);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            if (id != payment.PaymentID)
            {
                return BadRequest("ID mismatch between URL and request body.");
            }

            db.Entry(payment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            db.Payments.Remove(payment);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return db.Payments.Any(e => e.PaymentID == id);
        }
    }
}