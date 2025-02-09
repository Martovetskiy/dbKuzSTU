using Lab1.Application.DTOs.Car;

namespace Lab1.Application.Interfaces
{
    public interface ICarRepository
    {
        Task<List<ShowCarDTO>> GetCarsAsync(
            long? carId = null,
            string? make = null,
            string? model = null,
            int? year = null,
            string? colorHex = null,
            float? pricePerDay = null,
            string? numberPlate = null,
            string? status = null,
            string? type = null,
            DateTime? createdAt = null,
            string sortBy = "carId",
            string sortDirection = "ASC"
            );

        Task<ShowCarDTO> GetCarByIdAsync(long id);
        Task<ShowCarDTO> CreateCarAsync(CreateCarDTO createCarDTO);
        Task<ShowCarDTO> UpdateCarAsync(long id, UpdateCarDTO updateCarDTO);
        Task DeleteCarAsync(long id);
    }
}
