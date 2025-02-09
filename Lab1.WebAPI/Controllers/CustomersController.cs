using Lab1.Application.DTOs.Customer;
using Lab1.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomersController(ILogger<CustomersController> logger, CustomerService customerService) : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger = logger;
        private CustomerService _customerService = customerService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowCustomerDTO>>> GetCustomers(
            int? customerId = null,
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            string? phoneNumber = null,
            string? driverLicense = null,
            bool? isBanned = null,
            string sortBy = "customerId",
            string sortDirection = "ASC")
        {
            try
            {
                var result = await _customerService.GetCustomersAsync(customerId, firstName, lastName, email,
                    phoneNumber, driverLicense, isBanned, null, sortBy, sortDirection);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id:long}")]
        public async Task<ActionResult<ShowCustomerDTO>> GetCustomer(long id)
        {
            try
            {
                var result = await _customerService.GetCustomerByIdAsync((int)id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShowCustomerDTO>> InsertCustomer([FromBody] CreateCustomerDTO customer)
        {
            try
            {
                var result = await _customerService.CreateCustomerAsync(customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ShowCustomerDTO>> UpdateCustomer(long id, [FromBody] UpdateCustomerDTO customer)
        {
            try
            {
                var result = await _customerService.UpdateCustomerAsync((int)id, customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync((int)id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
