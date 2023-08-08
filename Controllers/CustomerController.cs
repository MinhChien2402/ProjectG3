using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Customer>> GetCustomer()
        {
            var customers = new List<Customer>();


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Customer;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer
                            {
                                customer_id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                first_name = reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.GetString(reader.GetOrdinal("last_name")),
                                date_of_birth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                                email = reader.GetString(reader.GetOrdinal("email")),
                                address = reader.GetString(reader.GetOrdinal("address")),
                                phone_number = reader.GetString(reader.GetOrdinal("phone_number")),
                                username = reader.GetString(reader.GetOrdinal("username")),
                                password = reader.GetString(reader.GetOrdinal("password")),
                                passort = reader.GetInt32(reader.GetOrdinal("passort")),
                            };

                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;


        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertCustomer(Customer customer)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                //string tableName = "Customer";
                //string enableIdentityInsertQuery = $"SET IDENTITY_INSERT {tableName} ON;";

                var commandText = "INSERT INTO Customer (first_name, last_name, date_of_birth, email, address, phone_number, username, password, passort) " +
                    "VALUES (@first_name, @last_name, @date_of_birth, @email, @address, @phone_number, @username, @password, @passort);";
                
                using (var command = new SqlCommand(commandText, connection))
                {
                    //command.Parameters.AddWithValue("@customer_id", customer.customer_id);
                    command.Parameters.AddWithValue("@first_name", customer.first_name);
                    command.Parameters.AddWithValue("@last_name", customer.last_name);
                    command.Parameters.AddWithValue("@date_of_birth", customer.date_of_birth);
                    command.Parameters.AddWithValue("@email", customer.email);
                    command.Parameters.AddWithValue("@address", customer.address);
                    command.Parameters.AddWithValue("@phone_number", customer.phone_number);
                    command.Parameters.AddWithValue("@username", customer.username);
                    command.Parameters.AddWithValue("@password", customer.password);
                    command.Parameters.AddWithValue("@passort", customer.passort);
                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustsomer(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Customer SET first_name = @first_name, last_name = @last_name, date_of_birth = @date_of_birth, email = @email, address = @address, phone_number = @phone_number" +
                    " WHERE customer_id = @customer_id";


                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.first_name);
                    command.Parameters.AddWithValue("@seat_total", customer.last_name);
                    command.Parameters.AddWithValue("@date_of_birth", customer.date_of_birth);
                    command.Parameters.AddWithValue("@email", customer.email);
                    command.Parameters.AddWithValue("@address", customer.address);
                    command.Parameters.AddWithValue("@phone_number", customer.phone_number);
                    command.Parameters.AddWithValue("@customer_id", customer.customer_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }
    }
}
