using Lab1.Application.DTOs.Rental;
using Lab1.Application.Interfaces;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly string _connectionString;

        public RentalRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ShowRentalDTO> CreateRentalAsync(CreateRentalDTO createRentalDTO)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"
            INSERT INTO Rentals (customerId, carId, startDate, endDate, totalPrice, createdAt)
            VALUES (@customerId, @carId, @startDate, @endDate, @totalPrice, @createdAt)
            RETURNING rentalId, customerId, carId, startDate, endDate, totalPrice, createdAt;", connection);

            command.Parameters.AddWithValue("customerId", createRentalDTO.CustomerId);
            command.Parameters.AddWithValue("carId", createRentalDTO.CarId);
            command.Parameters.AddWithValue("startDate", createRentalDTO.StartDate);
            command.Parameters.AddWithValue("endDate", createRentalDTO.EndDate);
            command.Parameters.AddWithValue("totalPrice", createRentalDTO.TotalPrice);
            command.Parameters.AddWithValue("createdAt", createRentalDTO.CreateAt);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ShowRentalDTO
                {
                    RentalId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    CarId = reader.GetInt64(2),
                    StartDate = reader.GetDateTime(3),
                    EndDate = reader.GetDateTime(4),
                    TotalPrice = (float)reader.GetDouble(5),
                    CreateAt = reader.GetDateTime(6)
                };
            }

            return null;
        }

        public async Task DeleteRentalAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("DELETE FROM Rentals WHERE rentalId = @rentalId", connection);
            command.Parameters.AddWithValue("rentalId", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<ShowRentalDTO> GetRentalByIdAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT * FROM Rentals WHERE rentalId = @rentalId", connection);
            command.Parameters.AddWithValue("rentalId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ShowRentalDTO
                {
                    RentalId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    CarId = reader.GetInt64(2),
                    StartDate = reader.GetDateTime(3),
                    EndDate = reader.GetDateTime(4),
                    TotalPrice = (float)reader.GetDouble(5),
                    CreateAt = reader.GetDateTime(6)
                };
            }

            return null;
        }

        public async Task<List<ShowRentalListDTO>> GetRentalsAsync(long? rentalId = null, string? email = null, string? make = null, string? model = null, DateTime? startDate = null, DateTime? endDate = null, float? totalPrice = null, DateTime? createdAt = null, string sortBy = "rentalId", string sortDirection = "ASC")
        {
            var rentals = new List<ShowRentalListDTO>();
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT R.rentalId, C.customerId, C2.carId, C.email, C2.make, C2.model, " +
               "R.startDate, R.endDate, R.totalPrice, R.createdAt " +
               "FROM Rentals AS R " +
               "LEFT JOIN Customers AS C ON C.customerId = R.customerId " +
               "LEFT JOIN Cars AS C2 ON C2.carId = R.carId WHERE 1=1";

            if (email != null)
            {
                query += " AND email = @email";
                parameters.Add(new NpgsqlParameter("email", email));
            }
            if (make != null)
            {
                query += " AND make = @make";
                parameters.Add(new NpgsqlParameter("make", make));
            }
            if (model != null)
            {
                query += " AND model = @model";
                parameters.Add(new NpgsqlParameter("model", model));
            }
            if (startDate != null)
            {
                query += " AND startDate = @startDate";
                parameters.Add(new NpgsqlParameter("startDate", startDate));
            }
            if (email != null)
            {
                query += " AND endDate = @email";
                parameters.Add(new NpgsqlParameter("email", email));
            }
            if (endDate != null)
            {
                query += " AND email = @endDate";
                parameters.Add(new NpgsqlParameter("endDate", endDate));
            }
            if (totalPrice != null)
            {
                query += " AND totalPrice = @totalPrice";
                parameters.Add(new NpgsqlParameter("totalPrice", totalPrice));
            }

            query += $" ORDER BY {sortBy} {sortDirection}";

            var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddRange(parameters.ToArray());
            

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                rentals.Add(new ShowRentalListDTO
                {
                    RentalId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    CarId = reader.GetInt64(2),
                    Email = reader.GetString(3),
                    Make = reader.GetString(4),
                    Model = reader.GetString(5),
                    StartDate = reader.GetDateTime(6),
                    EndDate = reader.GetDateTime(7),
                    TotalPrice = (float)reader.GetDouble(8),
                    CreateAt = reader.GetDateTime(9)
                });
            }

            return rentals;
        }

        public async Task<ShowRentalDTO> UpdateRentalAsync(long id, UpdateRentalDTO updateRentalDTO)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand(@"
            UPDATE Rentals SET customerId = @customerId, carId = @carId, 
                startDate = @startDate, endDate = @endDate, 
                totalPrice = @totalPrice, createdAt = @createdAt
            WHERE rentalId = @rentalId
            RETURNING rentalId, customerId, carId, startDate, endDate, totalPrice, createdAt;", connection);

            command.Parameters.AddWithValue("rentalId", id);
            command.Parameters.AddWithValue("customerId", updateRentalDTO.CustomerId);
            command.Parameters.AddWithValue("carId", updateRentalDTO.CarId);
            command.Parameters.AddWithValue("startDate", updateRentalDTO.StartDate);
            command.Parameters.AddWithValue("endDate", updateRentalDTO.EndDate);
            command.Parameters.AddWithValue("totalPrice", updateRentalDTO.TotalPrice);
            command.Parameters.AddWithValue("createdAt", updateRentalDTO.CreateAt);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ShowRentalDTO
                {
                    RentalId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    CarId = reader.GetInt64(2),
                    StartDate = reader.GetDateTime(3),
                    EndDate = reader.GetDateTime(4),
                    TotalPrice = (float)reader.GetDouble(5),
                    CreateAt = reader.GetDateTime(6)
                };
            }

            return null;
        }
    }
}
