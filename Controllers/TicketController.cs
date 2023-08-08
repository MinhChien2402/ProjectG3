using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            var tickets = new List<Ticket>();


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = @"SELECT * FROM Ticket a " +
                    " LEFT JOIN Flight b ON a.flight_id = b.flight_id" +
                    " LEFT JOIN Customer c ON c.customer_id = a.customer_id" +
                    " LEFT JOIN Seat d ON d.seat_id = a.seat_id";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var ticket = new Ticket
                            {
                                ticket_id = reader.GetInt32(reader.GetOrdinal("ticket_id")),
                                flight_id = reader.GetInt32(reader.GetOrdinal("flight_id")),
                                customer_id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                seat_id = reader.GetInt32(reader.GetOrdinal("seat_id")),
                                create_at = reader.GetDateTime(reader.GetOrdinal("create_at")),
                                update_at = reader.GetDateTime(reader.GetOrdinal("update_at")),
                                departure_time = reader.GetDateTime(reader.GetOrdinal("departure_time")),
                                arrival_time = reader.GetDateTime(reader.GetOrdinal("arrival_time")),
                                real_time_flight = reader.GetDateTime(reader.GetOrdinal("real_time_flight")),
                                ticket_status = reader.GetString(reader.GetOrdinal("ticket_status")),
                                price = reader.GetDecimal(reader.GetOrdinal("price")),
                            };

                            tickets.Add(ticket);
                        }
                    }
                }
            }

            return tickets;


        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertTicket(Ticket ticket)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Ticket (flight_id, customer_id, seat_id, create_at, update_at, departure_time, arrival_time, real_time_flight, ticket_status, price )" +
                    " VALUES (@flight_id, @customer_id, @seat_id, @create_at, @update_at, @departure_time, @arrival_time, @real_time_flight, @ticket_status ,@price);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@flight_id", ticket.flight_id);
                    command.Parameters.AddWithValue("@customer_id", ticket.customer_id);
                    command.Parameters.AddWithValue("@seat_id", ticket.seat_id);
                    command.Parameters.AddWithValue("@create_at", ticket.create_at);
                    command.Parameters.AddWithValue("@update_at", ticket.update_at);
                    command.Parameters.AddWithValue("@departure_time", ticket.departure_time);
                    command.Parameters.AddWithValue("@arrival_time", ticket.arrival_time);
                    command.Parameters.AddWithValue("@real_time_flight", ticket.real_time_flight);
                    command.Parameters.AddWithValue("@ticket_status", ticket.ticket_status);
                    command.Parameters.AddWithValue("@price", ticket.price);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateTicket(Ticket ticket)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Ticket SET flight_id = @flight_id, customer_id = @customer_id, seat_id = @seat_id, @create_at = create_at, @update_at = update_at , @departure_time=departure_time, @arrival_time=arrival_time, @real_time_flight= real_time_flight, @ticket_status=ticket_status, @price=price " +
                    " WHERE ticket_id = @ticket_id";


                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@flight_id", ticket.flight_id);
                    command.Parameters.AddWithValue("@customer_id", ticket.customer_id);
                    command.Parameters.AddWithValue("@seat_id", ticket.seat_id);
                    command.Parameters.AddWithValue("@create_at", ticket.create_at);
                    command.Parameters.AddWithValue("@update_at", ticket.update_at);
                    command.Parameters.AddWithValue("@departure_time", ticket.departure_time);
                    command.Parameters.AddWithValue("@arrival_time", ticket.arrival_time);
                    command.Parameters.AddWithValue("@real_time_flight", ticket.real_time_flight);
                    command.Parameters.AddWithValue("@ticket_status", ticket.ticket_status);
                    command.Parameters.AddWithValue("@price", ticket.price);
                    command.Parameters.AddWithValue("@ticket_id", ticket.ticket_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }
    }
}
