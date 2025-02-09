using Lab1.Application.DTOs.Rental;

namespace Lab1.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task<List<ShowRentalListDTO>> GetRentalsAsync(
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
            );

        Task<ShowRentalDTO> GetRentalByIdAsync(long id);
        Task<ShowRentalDTO> CreateRentalAsync(CreateRentalDTO createRentalDTO);
        Task<ShowRentalDTO> UpdateRentalAsync(long id, UpdateRentalDTO updateRentalDTO);
        Task DeleteRentalAsync(long id);
    }
}
