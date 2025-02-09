using Lab1.Application.DTOs.Payment;
using Lab1.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lab1.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentsController(ILogger<PaymentsController> logger, PaymentService paymentSerivce) : ControllerBase
    {
        private PaymentService _paymentService = paymentSerivce;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowPaymentListDTO>>>
            GetPaymentsForTable(
            long? rentalId = null,
            string? email = null,
            float? amount = null,
            int? step = null,
            DateTime? paymentDate = null,
            string? paymentMethod = null,
            DateTime? createdAt = null,
            string sortBy = "paymentId",
            string sortDirection = "ASC"
            )
        {
            try
            {
                var result = await _paymentService.GetPaymentsAsync(rentalId, email, amount, step, paymentDate, paymentMethod, createdAt, sortBy, sortDirection);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id:long}")]
        public async Task<ActionResult<ShowPaymentDTO>> GetPayment(long id)
        {
            try
            {
                var result = await _paymentService.GetPaymentByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        public async Task<ActionResult<ShowPaymentDTO>> InsertPayment([FromBody] CreatePaymentDTO payment)
        {
            try
            {
                var result = await _paymentService.CreatePaymentAsync(payment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ShowPaymentDTO>> UpdatePayment(long id, [FromBody] UpdatePaymentDTO payment)
        {
            try
            {
                var result = await _paymentService.UpdatePaymentAsync(id, payment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletePayment(long id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
