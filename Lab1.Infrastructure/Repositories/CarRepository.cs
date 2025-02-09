using Lab1.Application.DTOs.Car;
using Lab1.Application.Interfaces;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly string _connectionString;

        public CarRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ShowCarDTO> CreateCarAsync(CreateCarDTO createCarDTO)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"INSERT INTO cars (make, model, year, colorHex, pricePerDay, numberPlate, status, type, createdAt) 
                       VALUES (@make, @model, @year, @colorHex, @pricePerDay, @numberPlate, @status, @type, @createdAt) 
                       RETURNING carId;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("make", createCarDTO.Make);
            command.Parameters.AddWithValue("model", createCarDTO.Model);
            command.Parameters.AddWithValue("year", createCarDTO.Year);
            command.Parameters.AddWithValue("colorHex", createCarDTO.ColorHex);
            command.Parameters.AddWithValue("pricePerDay", createCarDTO.PricePerDay);
            command.Parameters.AddWithValue("numberPlate", createCarDTO.NumberPlate);
            command.Parameters.AddWithValue("status", createCarDTO.Status);
            command.Parameters.AddWithValue("type", "car");
            command.Parameters.AddWithValue("createdAt", createCarDTO.CreateAt);

            var carId = await command.ExecuteScalarAsync();

            return new ShowCarDTO
            {
                CarId = (long)carId,
                Make = createCarDTO.Make,
                Model = createCarDTO.Model,
                Year = createCarDTO.Year,
                ColorHex = createCarDTO.ColorHex,
                PricePerDay = createCarDTO.PricePerDay,
                NumberPlate = createCarDTO.NumberPlate,
                Status = createCarDTO.Status,
                //Type = createCarDTO.Type,
                CreateAt = createCarDTO.CreateAt
            };
        }

        public async Task DeleteCarAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "DELETE FROM cars WHERE carId = @carId;";
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("carId", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<ShowCarDTO> GetCarByIdAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM cars WHERE carId = @carId;";
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("carId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ShowCarDTO
                {
                    CarId = reader.GetInt64(0),
                    Make = reader.GetString(1),
                    Model = reader.GetString(2),
                    Year = reader.GetInt32(3),
                    ColorHex = reader.GetString(4),
                    PricePerDay = reader.GetFloat(5),
                    NumberPlate = reader.GetString(6),
                    Status = reader.GetString(7),
                    //Type = reader.GetString(8),
                    CreateAt = reader.GetDateTime(9)
                };
            }

            return null; // или возможно выбросить исключение, если не найдено
        }

        public async Task<List<ShowCarDTO>> GetCarsAsync(long? carId = null, string? make = null, string? model = null, int? year = null, string? colorHex = null, float? pricePerDay = null, string? numberPlate = null, string? status = null, string? type = null, DateTime? createdAt = null, string sortBy = "carId", string sortDirection = "ASC")
        {
            var cars = new List<ShowCarDTO>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM cars WHERE TRUE";
            if (carId.HasValue) query += " AND carId = @carId";
            if (!string.IsNullOrEmpty(make)) query += " AND make = @make";
            if (!string.IsNullOrEmpty(model)) query += " AND model = @model";
            if (year.HasValue) query += " AND year = @year";
            if (!string.IsNullOrEmpty(colorHex)) query += " AND colorHex = @colorHex";
            if (pricePerDay.HasValue) query += " AND pricePerDay = @pricePerDay";
            if (!string.IsNullOrEmpty(numberPlate)) query += " AND numberPlate = @numberPlate";
            if (!string.IsNullOrEmpty(status)) query += " AND status = @status";
            //if (!string.IsNullOrEmpty(type)) query += " AND type = @type";
            if (createdAt.HasValue) query += " AND createdAt = @createdAt";
            query += $" ORDER BY {sortBy} {sortDirection};";

            using var command = new NpgsqlCommand(query, connection);
            if (carId.HasValue) command.Parameters.AddWithValue("carId", carId.Value);
            if (!string.IsNullOrEmpty(make)) command.Parameters.AddWithValue("make", make);
            if (!string.IsNullOrEmpty(model)) command.Parameters.AddWithValue("model", model);
            if (year.HasValue) command.Parameters.AddWithValue("year", year.Value);
            if (!string.IsNullOrEmpty(colorHex)) command.Parameters.AddWithValue("colorHex", colorHex);
            if (pricePerDay.HasValue) command.Parameters.AddWithValue("pricePerDay", pricePerDay.Value);
            if (!string.IsNullOrEmpty(numberPlate)) command.Parameters.AddWithValue("numberPlate", numberPlate);
            if (!string.IsNullOrEmpty(status)) command.Parameters.AddWithValue("status", status);
            if (!string.IsNullOrEmpty(type)) command.Parameters.AddWithValue("type", type);
            if (createdAt.HasValue) command.Parameters.AddWithValue("createdAt", createdAt.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cars.Add(new ShowCarDTO
                {
                    CarId = reader.GetInt64(0),
                    Make = reader.GetString(1),
                    Model = reader.GetString(2),
                    Year = reader.GetInt32(3),
                    ColorHex = reader.GetString(4),
                    PricePerDay = reader.GetFloat(5),
                    NumberPlate = reader.GetString(6),
                    Status = reader.GetString(7),
                    //Type = reader.GetString(8),
                    CreateAt = reader.GetDateTime(9)
                });
            }

            return cars;
        }

        public async Task<ShowCarDTO> UpdateCarAsync(long id, UpdateCarDTO updateCarDTO)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"UPDATE cars 
                      SET make = @make, model = @model, year = @year, colorHex = @colorHex, pricePerDay = @pricePerDay, 
                          numberPlate = @numberPlate, status = @status, createdAt = @createdAt 
                      WHERE carId = @carId;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("carId", id);
            command.Parameters.AddWithValue("make", updateCarDTO.Make);
            command.Parameters.AddWithValue("model", updateCarDTO.Model);
            command.Parameters.AddWithValue("year", updateCarDTO.Year);
            command.Parameters.AddWithValue("colorHex", updateCarDTO.ColorHex);
            command.Parameters.AddWithValue("pricePerDay", updateCarDTO.PricePerDay);
            command.Parameters.AddWithValue("numberPlate", updateCarDTO.NumberPlate);
            command.Parameters.AddWithValue("status", updateCarDTO.Status);
            //command.Parameters.AddWithValue("type", updateCarDTO.Type);
            command.Parameters.AddWithValue("createdAt", updateCarDTO.CreateAt);

            await command.ExecuteNonQueryAsync();

            return await GetCarByIdAsync(id);
        }
    }
}
