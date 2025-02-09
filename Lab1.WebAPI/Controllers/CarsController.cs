using Lab1.Application.DTOs.Car;
using Lab1.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lab1.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarsController(ILogger<CarsController> logger, CarService carService) : ControllerBase
    {
        private readonly ILogger<CarsController> _logger = logger;
        private CarService _carService = carService;


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowCarDTO>>> GetCars(
            long? id = null,
            string? make = null,
            string? model = null,
            int? year = null,
            string? colorHex = null,
            float? pricePerDay = null,
            string? numberPlate = null,
            string? status = null,
            string sortBy = "carId",
            string sortDirection = "ASC")
        {
            try
            {
                var result = await _carService.GetCarsAsync(id, make, model, year, colorHex, pricePerDay, numberPlate, status, null,
                    null, sortBy, sortDirection);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ShowCarDTO>> GetCar(long id)
        {
            try
            {
                var result = await _carService.GetCarByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShowCarDTO>> InsertCar([FromBody] CreateCarDTO car)
        {
            try
            {
                var result = await _carService.CreateCarAsync(car);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ShowCarDTO>> UpdateCar(long id, [FromBody] UpdateCarDTO car)
        {
            try
            {
                var result = await _carService.UpdateCarAsync(id, car);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCar(long id)
        {
            try
            {
                await _carService.DeleteCarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
