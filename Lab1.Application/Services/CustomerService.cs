using Lab1.Application.DTOs.Customer;
using Lab1.Application.Interfaces;

namespace Lab1.Application.Services
{
    public class CustomerService
    {
        private ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<ShowCustomerDTO>> GetCustomersAsync(
            int? customerId = null,
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            string? phoneNumber = null,
            string? driverLicense = null,
            bool? isBanned = null,
            DateTime? createdAt = null,
            string sortBy = "customerId",
            string sortDirection = "ASC"
            )
        {
            var result = await _customerRepository.GetCustomersAsync(customerId, firstName, lastName, email,
                phoneNumber, driverLicense, isBanned, createdAt, sortBy, sortDirection);
            return result;
        }

        public async Task<ShowCustomerDTO> GetCustomerByIdAsync(int id)
        {
            var result = await _customerRepository.GetCustomerByIdAsync(id);
            return result;
        }

        public async Task<ShowCustomerDTO> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {
            var result = await _customerRepository.CreateCustomerAsync(createCustomerDTO);
            return result;
        }

        public async Task<ShowCustomerDTO> UpdateCustomerAsync(int id, UpdateCustomerDTO updateCustomerDTO)
        {
            var result = await _customerRepository.UpdateCustomerAsync(id, updateCustomerDTO);
            return result;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}
