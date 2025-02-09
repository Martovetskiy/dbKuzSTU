using Lab1.Application.DTOs.Customer;

namespace Lab1.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<ShowCustomerDTO>> GetCustomersAsync(
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
            );

        Task<ShowCustomerDTO> GetCustomerByIdAsync(int id);
        Task<ShowCustomerDTO> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO);
        Task<ShowCustomerDTO> UpdateCustomerAsync(int id, UpdateCustomerDTO updateCustomerDTO);
        Task DeleteCustomerAsync(int id);
    }
}
