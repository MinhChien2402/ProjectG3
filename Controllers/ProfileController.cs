using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Profile>> GetProfiles()
        {
            var profiles = new List<Profile>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Profile;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var profile = new Profile
                            {
                                ProfileId = reader.GetInt32(reader.GetOrdinal("ProfileId")),
                                customer_id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                first_name = reader.GetString(reader.GetOrdinal("first_name")),
                                last_name = reader.GetString(reader.GetOrdinal("last_name")),
                                address = reader.GetString(reader.GetOrdinal("address")),
                                phone_number = reader.GetString(reader.GetOrdinal("phone_number")),
                                email = reader.GetString(reader.GetOrdinal("email")),
                                sex = reader.GetString(reader.GetOrdinal("sex")),
                                date_of_birth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                                CreditCard = reader.GetString(reader.GetOrdinal("CreditCard")),
                                SkyMiles = reader.GetString(reader.GetOrdinal("SkyMiles")),

                            };

                            profiles.Add(profile);
                        }
                    }
                }
            }

            return profiles;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertProfile(Profile profile)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Profile (customer_id, first_name, last_name, address, phone_number, email, sex, date_of_birth, CreditCard, SkyMiles) " +
                    "VALUES (@customer_id, @first_name, @last_name, @address, @phone_number, @email, @sex, @date_of_birth, @CreditCard, @SkyMiles);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@customer_id", profile.customer_id);
                    command.Parameters.AddWithValue("@first_name", profile.first_name);
                    command.Parameters.AddWithValue("@last_name", profile.last_name);
                    command.Parameters.AddWithValue("@address", profile.address);
                    command.Parameters.AddWithValue("@phone_number", profile.phone_number);
                    command.Parameters.AddWithValue("@email", profile.email);
                    command.Parameters.AddWithValue("@sex", profile.sex);
                    command.Parameters.AddWithValue("@date_of_birth", profile.date_of_birth);
                    command.Parameters.AddWithValue("@CreditCard", profile.CreditCard);
                    command.Parameters.AddWithValue("@SkyMiles", profile.SkyMiles);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(Profile profile)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Profile SET customer_id = @customer_id, first_name = @first_name, last_name = @last_name, address = @address, phone_number = @phone_number, email = @email, sex = @sex, date_of_birth= @date_of_birth, CreditCard = @CreditCard, SkyMiles = @SkyMiles " +
                    "WHERE ProfileId = @ProfileId";



                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@customer_id", profile.customer_id);
                    command.Parameters.AddWithValue("@first_name", profile.first_name);
                    command.Parameters.AddWithValue("@last_name", profile.last_name);
                    command.Parameters.AddWithValue("@address", profile.address);
                    command.Parameters.AddWithValue("@phone_number", profile.phone_number);
                    command.Parameters.AddWithValue("@email", profile.email);
                    command.Parameters.AddWithValue("@sex", profile.sex);
                    command.Parameters.AddWithValue("@date_of_birth", profile.date_of_birth);
                    command.Parameters.AddWithValue("@CreditCard", profile.CreditCard);
                    command.Parameters.AddWithValue("@SkyMiles", profile.SkyMiles);
                    command.Parameters.AddWithValue("@ProfileId", profile.ProfileId);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }
    }
}
