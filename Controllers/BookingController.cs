using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Booking>> GetBookings()
        {
            var bookings = new List<Booking>();


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = @"SELECT * FROM Booking a " +
                    " LEFT JOIN Ticket b ON a.ticket_id = b.ticket_id" +
                    " LEFT JOIN Customer c ON c.customer_id = a.customer_id";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var booking = new Booking
                            {
                                booking_id = reader.GetInt32(reader.GetOrdinal("booking_id")),
                                ticket_id = reader.GetInt32(reader.GetOrdinal("ticket_id")),
                                customer_id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                time_oder = reader.GetDateTime(reader.GetOrdinal("time_oder")),
                                booking_status = reader.GetString(reader.GetOrdinal("booking_status")),
                            };

                            bookings.Add(booking);
                        }
                    }
                }
            }

            return bookings;


        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertBooking(Booking booking)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Booking (ticket_id, customer_id, time_oder, booking_status) " +
                    "VALUES (@ticket_id, @customer_id, @time_oder, @booking_status);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@ticket_id", booking.ticket_id);
                    command.Parameters.AddWithValue("@customer_id", booking.customer_id);
                    command.Parameters.AddWithValue("@time_oder", booking.time_oder);
                    command.Parameters.AddWithValue("@booking_status", booking.booking_status);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBooking(Booking booking)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Booking SET ticket_id = @ticket_id, customer_id = @customer_id, time_oder = @time_oder, booking_status = @booking_status " +
                    " WHERE booking_id = @booking_id";


                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@ticket_id", booking.ticket_id);
                    command.Parameters.AddWithValue("@customer_id", booking.customer_id);
                    command.Parameters.AddWithValue("@time_oder", booking.time_oder);
                    command.Parameters.AddWithValue("@booking_status", booking.booking_status);
                    command.Parameters.AddWithValue("@booking_id", booking.booking_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }
    }
}
