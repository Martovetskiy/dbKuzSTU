using Lab1.Application.DTOs.Payment;
using Lab1.Application.Interfaces;
using Npgsql;

namespace Lab1.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ShowPaymentDTO> CreatePaymentAsync(CreatePaymentDTO createPaymentDTO)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("INSERT INTO Payments (rentalId, amount, step, paymentDate, paymentMethod, createdAt) VALUES (@rentalId, @amount, @step, @paymentDate, @paymentMethod, @createdAt) RETURNING paymentId;", connection))
                {
                    command.Parameters.AddWithValue("rentalId", createPaymentDTO.RentalId);
                    command.Parameters.AddWithValue("amount", createPaymentDTO.Amount);
                    command.Parameters.AddWithValue("step", createPaymentDTO.Step);
                    command.Parameters.AddWithValue("paymentDate", createPaymentDTO.PaymentDate);
                    command.Parameters.AddWithValue("paymentMethod", createPaymentDTO.PaymentMethod);
                    command.Parameters.AddWithValue("createdAt", createPaymentDTO.CreateAt);

                    var paymentId = await command.ExecuteScalarAsync();
                    return new ShowPaymentDTO
                    {
                        PaymentId = Convert.ToInt64(paymentId),
                        RentalId = createPaymentDTO.RentalId,
                        Amount = createPaymentDTO.Amount,
                        Step = createPaymentDTO.Step,
                        PaymentDate = createPaymentDTO.PaymentDate,
                        PaymentMethod = createPaymentDTO.PaymentMethod,
                        CreateAt = createPaymentDTO.CreateAt
                    };
                }
            }
        }

        public async Task DeletePaymentAsync(long id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("DELETE FROM Payments WHERE paymentId = @paymentId;", connection))
                {
                    command.Parameters.AddWithValue("paymentId", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<ShowPaymentDTO> GetPaymentByIdAsync(long id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT * FROM Payments WHERE paymentId = @paymentId;", connection))
                {
                    command.Parameters.AddWithValue("paymentId", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ShowPaymentDTO
                            {
                                PaymentId = reader.GetInt64(0),
                                RentalId = reader.GetInt64(1),
                                Amount = reader.GetFloat(2),
                                Step = reader.GetInt32(3),
                                PaymentDate = reader.GetDateTime(4),
                                PaymentMethod = reader.GetString(5),
                                CreateAt = reader.GetDateTime(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<ShowPaymentListDTO>> GetPaymentsAsync(long? rentalId = null, string? email = null, float? amount = null, int? step = null, DateTime? paymentDate = null, string? paymentMethod = null, DateTime? createdAt = null, string sortBy = "paymentId", string sortDirection = "ASC")
        {
            var payments = new List<ShowPaymentListDTO>();
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            string query = "SELECT p.paymentId, r.rentalId, c.email, p.amount, p.step, p.paymentDate, p.paymentMethod, p.createdAt FROM Payments p" +
                " LEFT JOIN Rentals r ON r.rentalId = p.rentalId" +
                " LEFT JOIN Customers c ON c.customerId = r.customerId" +
                " WHERE TRUE";

            if (rentalId.HasValue) { 
                query += " AND rentalId = @rentalId";
                parameters.Add(new NpgsqlParameter("@rentalId", rentalId));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query += " AND email = @email"; // предполагается, что в Payments есть поле email
                parameters.Add(new NpgsqlParameter("@email", email));
            }

            if (amount.HasValue)
            {
                query += " AND amount = @amount";
                parameters.Add(new NpgsqlParameter("@amount", amount));
            }
                
            if (step.HasValue)
            {
                query += " AND step = @step";
                parameters.Add(new NpgsqlParameter("@step", step));
            }
                
            if (paymentDate.HasValue)
            {
                query += " AND paymentDate = @paymentDate";
                parameters.Add(new NpgsqlParameter("@paymentDate", paymentDate));
            }
                
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                query += " AND paymentMethod = @paymentMethod";
                parameters.Add(new NpgsqlParameter("@paymentMethod", paymentMethod));
            }
                
            if (createdAt.HasValue)
            {
                query += " AND createdAt = @createdAt";
                parameters.Add(new NpgsqlParameter("@createdAt", createdAt.Value.Date));
            }
                

            query += $" ORDER BY {sortBy} {sortDirection}";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            payments.Add(new ShowPaymentListDTO
                            {
                                PaymentId = reader.GetInt64(0),
                                RentalId = reader.GetInt64(1),
                                Email = reader.GetString(2), // предполагается, что в Payments есть поле email
                                Amount = reader.GetFloat(3),
                                Step = reader.GetInt32(4),
                                PaymentDate = reader.GetDateTime(5),
                                PaymentMethod = reader.GetString(6),
                                CreateAt = reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }
            return payments;
        }

        public async Task<ShowPaymentDTO> UpdatePaymentAsync(long id, UpdatePaymentDTO updatePaymentDTO)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("UPDATE Payments SET rentalId = @rentalId, amount = @amount, step = @step, paymentDate = @paymentDate, paymentMethod = @paymentMethod, createdAt = @createdAt WHERE paymentId = @paymentId;", connection))
                {
                    command.Parameters.AddWithValue("paymentId", id);
                    command.Parameters.AddWithValue("rentalId", updatePaymentDTO.RentalId);
                    command.Parameters.AddWithValue("amount", updatePaymentDTO.Amount);
                    command.Parameters.AddWithValue("step", updatePaymentDTO.Step);
                    command.Parameters.AddWithValue("paymentDate", updatePaymentDTO.PaymentDate);
                    command.Parameters.AddWithValue("paymentMethod", updatePaymentDTO.PaymentMethod);
                    command.Parameters.AddWithValue("createdAt", updatePaymentDTO.CreateAt);

                    await command.ExecuteNonQueryAsync();
                    return await GetPaymentByIdAsync(id); // Возвращаем обновленный объект
                }
            }
        }
    }
}
