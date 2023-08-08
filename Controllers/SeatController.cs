using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Seat>> GetSeats()
        {
            var seats = new List<Seat>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Seat;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var seat = new Seat
                            {
                                seat_id = reader.GetInt32(reader.GetOrdinal("seat_id")),
                                airplane_id = reader.GetInt32(reader.GetOrdinal("airplane_id")),
                                seat_total = reader.GetInt32(reader.GetOrdinal("seat_total")),
                                seat_type = reader.GetString(reader.GetOrdinal("seat_type")),
                                seat_status = reader.GetString(reader.GetOrdinal("seat_status")),
                                price = reader.GetDecimal(reader.GetOrdinal("price")),
                                
                            };

                            seats.Add(seat);
                        }
                    }

                }
            }
            return seats;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertSeat(Seat seat)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Seat (airplane_id, seat_total,seat_type, seat_status, price) " +
                    "VALUES (@airplane_id, @seat_number, @seat_total, @seat_type, @seat_status, @price);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@airplane_id", seat.airplane_id);
                    command.Parameters.AddWithValue("@seat_total", seat.seat_total);
                    command.Parameters.AddWithValue("@seat_type", seat.seat_type);
                    command.Parameters.AddWithValue("@seat_status", seat.seat_status);
                    command.Parameters.AddWithValue("@price", seat.price);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateSeat(Seat seat)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = "UPDATE Seat SET airplane_id = @airplane_id, seat_total = @seat_total, seat_type = @seat_type, seat_status = @seat_status, price = @price " +
                    "WHERE seat_id = @seat_id";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@airplane_id", seat.airplane_id);
                    command.Parameters.AddWithValue("@seat_total", seat.seat_total);
                    command.Parameters.AddWithValue("@seat_type", seat.seat_type);
                    command.Parameters.AddWithValue("@seat_status", seat.seat_status);
                    command.Parameters.AddWithValue("@price", seat.price);
                    command.Parameters.AddWithValue("@seat_id", seat.seat_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }
    }
}
