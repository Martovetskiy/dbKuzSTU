using Lab1.Application.DTOs.Car;
using Lab1.Application.Interfaces;

namespace Lab1.Application.Services
{
    public class CarService
    {
        private ICarRepository _customerRepository;

        public CarService(ICarRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<ShowCarDTO>> GetCarsAsync(
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
            )
        {
            var result = await _customerRepository.GetCarsAsync(carId, make, model, year, colorHex, pricePerDay, numberPlate,
                status, type, createdAt, sortBy, sortDirection);
            return result;
        }

        public async Task<ShowCarDTO> GetCarByIdAsync(long id)
        {
            var result = await _customerRepository.GetCarByIdAsync(id);
            return result;
        }

        public async Task<ShowCarDTO> CreateCarAsync(CreateCarDTO createCarDTO)
        {
            var result = await _customerRepository.CreateCarAsync(createCarDTO);
            return result;
        }

        public async Task<ShowCarDTO> UpdateCarAsync(long id, UpdateCarDTO updateCarDTO)
        {
            var result = await _customerRepository.UpdateCarAsync(id, updateCarDTO);
            return result;
        }

        public async Task DeleteCarAsync(long id)
        {
            await _customerRepository.DeleteCarAsync(id);
        }
    }
}
