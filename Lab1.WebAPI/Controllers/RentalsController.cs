using Lab1.Application.DTOs.Rental;
using Lab1.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lab1.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RentalsController(ILogger<RentalsController> logger, RentalService rentalService) : ControllerBase
    {
        private readonly ILogger<RentalsController> _logger = logger;
        private RentalService _rentalService = rentalService;

        static async Task DelayExample()
        {
            await Task.Delay(10000);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowRentalListDTO>>> GetRentalsForTable(
            long? rentalId = null,
            string? email = null,
            string? make = null,
            string? model = null,
            float? totalPrice = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string sortBy = "rentalId",
            string sortDirection = "ASC")
        {
            try
            {
                var result = await _rentalService.GetRentalsAsync(rentalId, email, make, model, startDate, endDate, totalPrice, null, sortBy, sortDirection);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ShowRentalDTO>> GetRental(long id)
        {
            try
            {
                var result = await _rentalService.GetRentalByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShowRentalDTO>> InsertRental([FromBody] CreateRentalDTO rental)
        {
            try
            {
                var result = await _rentalService.CreateRentalAsync(rental);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ShowRentalDTO>> UpdateRental(long id, [FromBody] UpdateRentalDTO rental)
        {
            try
            {
                var result = await _rentalService.UpdateRentalAsync(id, rental);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteRental(long id)
        {
            try
            {
                await _rentalService.DeleteRentalAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
