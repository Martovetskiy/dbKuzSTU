using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Lab1.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly string _connectionString = "Host=192.168.86.128;Database=cars;Username=postgres;Password=postgres";

        [HttpGet("car-status-count")]
        public IActionResult GetCarStatusCount()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT status, COUNT(*) AS total_count FROM public.cars GROUP BY status;";
                using (var cmd = new NpgsqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    var result = new System.Collections.Generic.List<object>();
                    while (reader.Read())
                    {
                        result.Add(new
                        {
                            Status = reader["status"],
                            TotalCount = reader["total_count"]
                        });
                    }
                    return Ok(result);
                }
            }
        }

        [HttpGet("total-income")]
        public IActionResult GetTotalIncome()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(amount) AS total_income FROM public.payments;";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    var totalIncome = cmd.ExecuteScalar();
                    return Ok(new { TotalIncome = totalIncome });
                }
            }
        }

        [HttpGet("trucks-cars-union")]
        public IActionResult GetTrucksAndCarsUnion()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT truckId AS id, make, model, year, colorHex, numberPlate, 'Truck' AS type
                    FROM public.Trucks
                    UNION
                    SELECT carid AS id, make, model, year, colorHex, numberPlate, 'Car' AS type
                    FROM public.cars;";

                using (var cmd = new NpgsqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    var result = new List<object>();
                    while (reader.Read())
                    {
                        result.Add(new
                        {
                            Id = reader["id"],
                            Make = reader["make"],
                            Model = reader["model"],
                            Year = reader["year"],
                            ColorHex = reader["colorHex"],
                            NumberPlate = reader["numberPlate"],
                            Type = reader["type"]
                        });
                    }
                    return Ok(result);
                }
            }
        }

        [HttpGet("count-cars-by-color")]
        public IActionResult GetCountCarsByColor()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT colorHex, COUNT(*) AS total_count FROM public.cars GROUP BY colorHex;";
                using (var cmd = new NpgsqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    var result = new List<object>();
                    while (reader.Read())
                    {
                        result.Add(new
                        {
                            ColorHex = reader["colorHex"],
                            TotalCount = reader["total_count"]
                        });
                    }
                    return Ok(result);
                }
            }
        }

        [HttpGet("average-car-age")]
        public IActionResult GetAverageCarAge()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT AVG(EXTRACT(YEAR FROM (CURRENT_DATE - createdAt))) AS average_car_age FROM public.cars;";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    var averageAge = cmd.ExecuteScalar();
                    return Ok(new { AverageCarAge = averageAge });
                }
            }
        }
    }
}
