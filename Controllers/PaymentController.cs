using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Payment>> GetPayments()
        {
            var payments = new List<Payment>();


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = @"SELECT * FROM Payment a " +
                    " LEFT JOIN Booking b ON a.booking_id = b.booking_id" +
                    " LEFT JOIN Customer c ON c.customer_id = a.customer_id" +
                    " LEFT JOIN Ticket d ON d.ticket_id = a.ticket_id";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var payment = new Payment
                            {
                                payment_id = reader.GetInt32(reader.GetOrdinal("payment_id")),
                                booking_id = reader.GetInt32(reader.GetOrdinal("booking_id")),
                                customer_id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                ticket_id = reader.GetInt32(reader.GetOrdinal("ticket_id")),
                                payment_time = reader.GetDateTime(reader.GetOrdinal("payment_time")),
                                payment_method = reader.GetString(reader.GetOrdinal("payment_method")),
                                create_at = reader.GetDateTime(reader.GetOrdinal("create_at")),
                                
                            };

                            payments.Add(payment);
                        }
                    }
                }
            }

            return payments;


        }


        [HttpPost("insert")]
        public async Task<IActionResult> InsertPayment(Payment payment)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Payment (booking_id, customer_id, ticket_id, payment_time, payment_method, create_at) " +
                "VALUES (@booking_id, @customer_id, @ticket_id, @payment_time, @payment_method, @create_at);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@booking_id", payment.booking_id);
                    command.Parameters.AddWithValue("@customer_id", payment.customer_id);
                    command.Parameters.AddWithValue("@ticket_id", payment.ticket_id);
                    command.Parameters.AddWithValue("@payment_time", payment.payment_time);
                    command.Parameters.AddWithValue("@payment_method", payment.payment_method);
                    command.Parameters.AddWithValue("@create_at", payment.create_at);
                    

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAirplane(Payment payment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Payment SET booking_id = @booking_id, customer_id = @customer_id, ticket_id = @ticket_id, payment_time = @payment_time, payment_method=@payment_method, create_at = @create_at  " +
                    "WHERE payment_id = @payment_id";


                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@booking_id", payment.booking_id);
                    command.Parameters.AddWithValue("@customer_id", payment.customer_id);
                    command.Parameters.AddWithValue("@ticket_id", payment.ticket_id);
                    command.Parameters.AddWithValue("@payment_time", payment.payment_time);
                    command.Parameters.AddWithValue("@payment_method", payment.payment_method);
                    command.Parameters.AddWithValue("@create_at", payment.create_at);
                    command.Parameters.AddWithValue("@payment_id", payment.payment_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }

    }
}
