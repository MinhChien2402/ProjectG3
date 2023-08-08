using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";
        [HttpGet("getall")]
        public async Task<IEnumerable<Location>> GetLocations()
        {
            var locations = new List<Location>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Location;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var location = new Location
                            {
                                location_id = reader.GetInt32(reader.GetOrdinal("location_id")),
                                code = reader.GetString(reader.GetOrdinal("code")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                parentId = reader.GetInt32(reader.GetOrdinal("parentId")),
                            };

                            locations.Add(location);
                        }
                    }

                }
            }
            return locations;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertLocation(Location location)
        {


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Location (code, name, parentId) VALUES (@code, @name, @parentId);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@code", location.code);
                    command.Parameters.AddWithValue("@name", location.name);
                    command.Parameters.AddWithValue("@parentId", location.parentId);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateLocation(Location location)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Location SET code = @code, name = @name, parentId = @parentId WHERE location_id = @location_id";



                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@code", location.code);
                    command.Parameters.AddWithValue("@name", location.name);
                    command.Parameters.AddWithValue("@parentId", location.parentId);
                    command.Parameters.AddWithValue("@location_id", location.location_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }
    }
}
