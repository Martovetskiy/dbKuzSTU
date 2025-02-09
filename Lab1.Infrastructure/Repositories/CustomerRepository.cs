using Lab1.Application.DTOs.Customer;
using Lab1.Application.Interfaces;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ShowCustomerDTO> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                INSERT INTO Customers (firstName, lastName, email, phoneNumber, driverLicense, isBanned, createAt)
                VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @DriverLicense, @IsBanned, @CreateAt)
                RETURNING customerId;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("FirstName", createCustomerDTO.FirstName);
                    command.Parameters.AddWithValue("LastName", createCustomerDTO.LastName);
                    command.Parameters.AddWithValue("Email", createCustomerDTO.Email);
                    command.Parameters.AddWithValue("PhoneNumber", createCustomerDTO.PhoneNumber);
                    command.Parameters.AddWithValue("DriverLicense", createCustomerDTO.DriverLicense);
                    command.Parameters.AddWithValue("IsBanned", createCustomerDTO.IsBanned);
                    command.Parameters.AddWithValue("CreateAt", createCustomerDTO.CreateAt);

                    var customerId = await command.ExecuteScalarAsync();

                    return new ShowCustomerDTO
                    {
                        CustomerId = Convert.ToInt64(customerId),
                        FirstName = createCustomerDTO.FirstName,
                        LastName = createCustomerDTO.LastName,
                        Email = createCustomerDTO.Email,
                        PhoneNumber = createCustomerDTO.PhoneNumber,
                        DriverLicense = createCustomerDTO.DriverLicense,
                        IsBanned = createCustomerDTO.IsBanned,
                        CreateAt = createCustomerDTO.CreateAt
                    };
                }
            }
        }

        public async Task<ShowCustomerDTO> GetCustomerByIdAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Customers WHERE customerId = @CustomerId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("CustomerId", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ShowCustomerDTO
                            {
                                CustomerId = reader.GetInt64(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                DriverLicense = reader.GetString(5),
                                IsBanned = reader.GetBoolean(6),
                                CreateAt = reader.GetDateTime(7)
                            };
                        }
                    }
                }
            }

            return null; // Если клиент не найден
        }

        public async Task<List<ShowCustomerDTO>> GetCustomersAsync(int? customerId = null, string? firstName = null, string? lastName = null, string? email = null, string? phoneNumber = null, string? driverLicense = null, bool? isBanned = null, DateTime? createdAt = null, string sortBy = "customerId", string sortDirection = "ASC")
        {
            var customers = new List<ShowCustomerDTO>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Customers WHERE TRUE"; // Начинаем с TRUE, чтобы динамически добавлять условия
                var parameters = new List<NpgsqlParameter>();

                if (customerId.HasValue)
                {
                    query += " AND customerId = @CustomerId";
                    parameters.Add(new NpgsqlParameter("CustomerId", customerId.Value));
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    query += " AND firstName ILIKE @FirstName";
                    parameters.Add(new NpgsqlParameter("FirstName", $"%{firstName}%"));
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    query += " AND lastName ILIKE @LastName";
                    parameters.Add(new NpgsqlParameter("LastName", $"%{lastName}%"));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    query += " AND email ILIKE @Email";
                    parameters.Add(new NpgsqlParameter("Email", $"%{email}%"));
                }

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    query += " AND phoneNumber ILIKE @PhoneNumber";
                    parameters.Add(new NpgsqlParameter("PhoneNumber", $"%{phoneNumber}%"));
                }

                if (!string.IsNullOrEmpty(driverLicense))
                {
                    query += " AND driverLicense = @DriverLicense";
                    parameters.Add(new NpgsqlParameter("DriverLicense", driverLicense));
                }

                if (isBanned.HasValue)
                {
                    query += " AND isBanned = @IsBanned";
                    parameters.Add(new NpgsqlParameter("IsBanned", isBanned.Value));
                }

                if (createdAt.HasValue)
                {
                    query += " AND createAt >= @CreateAt";
                    parameters.Add(new NpgsqlParameter("CreateAt", createdAt.Value));
                }

                query += $" ORDER BY {sortBy} {sortDirection}";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            customers.Add(new ShowCustomerDTO
                            {
                                CustomerId = reader.GetInt64(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                DriverLicense = reader.GetString(5),
                                IsBanned = reader.GetBoolean(6),
                                CreateAt = reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }

            return customers;
        }

        public async Task<ShowCustomerDTO> UpdateCustomerAsync(int id, UpdateCustomerDTO updateCustomerDTO)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                UPDATE Customers
                SET firstName = @FirstName,
                    lastName = @LastName,
                    email = @Email,
                    phoneNumber = @PhoneNumber,
                    driverLicense = @DriverLicense,
                    isBanned = @IsBanned,
                    createAt = @CreateAt
                WHERE customerId = @CustomerId;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("CustomerId", id);
                    command.Parameters.AddWithValue("FirstName", updateCustomerDTO.FirstName);
                    command.Parameters.AddWithValue("LastName", updateCustomerDTO.LastName);
                    command.Parameters.AddWithValue("Email", updateCustomerDTO.Email);
                    command.Parameters.AddWithValue("PhoneNumber", updateCustomerDTO.PhoneNumber);
                    command.Parameters.AddWithValue("DriverLicense", updateCustomerDTO.DriverLicense);
                    command.Parameters.AddWithValue("IsBanned", updateCustomerDTO.IsBanned);
                    command.Parameters.AddWithValue("CreateAt", updateCustomerDTO.CreateAt);

                    await command.ExecuteNonQueryAsync();

                    return new ShowCustomerDTO
                    {
                        CustomerId = id,
                        FirstName = updateCustomerDTO.FirstName,
                        LastName = updateCustomerDTO.LastName,
                        Email = updateCustomerDTO.Email,
                        PhoneNumber = updateCustomerDTO.PhoneNumber,
                        DriverLicense = updateCustomerDTO.DriverLicense,
                        IsBanned = updateCustomerDTO.IsBanned,
                        CreateAt = updateCustomerDTO.CreateAt,
                    };
                }
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM Customers WHERE customerId = @CustomerId;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("CustomerId", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
