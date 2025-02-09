using Lab1.Application.DTOs.Rental;
using Lab1.Application.Interfaces;

namespace Lab1.Application.Services
{
    public class RentalService
    {
        private IRentalRepository _customerRepository;

        public RentalService(IRentalRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<ShowRentalListDTO>> GetRentalsAsync(
            long? rentalId = null,
            string? email = null,
            string? make = null,
            string? model = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            float? totalPrice = null,
            DateTime? createdAt = null,
            string sortBy = "rentalId",
            string sortDirection = "ASC"
            )
        {
            var result = await _customerRepository.GetRentalsAsync(rentalId, email, make, model, startDate, endDate, totalPrice, createdAt, sortBy, sortDirection);
            return result;
        }

        public async Task<ShowRentalDTO> GetRentalByIdAsync(long id)
        {
            var result = await _customerRepository.GetRentalByIdAsync(id);
            return result;
        }

        public async Task<ShowRentalDTO> CreateRentalAsync(CreateRentalDTO createRentalDTO)
        {
            var result = await _customerRepository.CreateRentalAsync(createRentalDTO);
            return result;
        }

        public async Task<ShowRentalDTO> UpdateRentalAsync(long id, UpdateRentalDTO updateRentalDTO)
        {
            var result = await _customerRepository.UpdateRentalAsync(id, updateRentalDTO);
            return result;
        }

        public async Task DeleteRentalAsync(long id)
        {
            await _customerRepository.DeleteRentalAsync(id);
        }
    }
}
