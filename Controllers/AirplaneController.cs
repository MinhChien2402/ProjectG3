using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirplaneController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("all")]
        public async Task<IEnumerable<Airplane>> GetAirplanes()
        {
            var airplanes = new List<Airplane>();


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Airplane;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var airplane = new Airplane
                            {
                                airplane_id = reader.GetInt32(reader.GetOrdinal("airplane_id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                seat_total = reader.GetInt32(reader.GetOrdinal("seat_total")),
                                airplane_brand = reader.GetString(reader.GetOrdinal("airplane_brand")),
                                //logo_url = reader.GetString(reader.GetOrdinal("logo_url")),
                            };

                            airplanes.Add(airplane);
                        }
                    }
                }
            }

            return airplanes;


        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertAirplane(Airplane airplane)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Airplane ( Name, seat_total, airplane_brand) VALUES (@Name, @seat_total, @airplane_brand);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Name", airplane.Name);
                    command.Parameters.AddWithValue("@seat_total", airplane.seat_total);
                    command.Parameters.AddWithValue("@airplane_brand", airplane.airplane_brand);
                    //command.Parameters.AddWithValue("@logo_url", airplane.logo_url);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAirplane(Airplane airplane)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Airplane SET Name = @Name, seat_total = @seat_total, airplane_brand = @airplane_brand WHERE airplane_id = @airplane_id";


                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Name", airplane.Name);
                    command.Parameters.AddWithValue("@seat_total", airplane.seat_total);
                    command.Parameters.AddWithValue("@airplane_brand", airplane.airplane_brand);
                    command.Parameters.AddWithValue("@airplane_id", airplane.airplane_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }

        [HttpPost("insertImage")]
        public async Task<IActionResult> InsertAirplanes([FromForm] Airplane airplane)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Airplane (Name, seat_total,airplane_brand,logo_url ) VALUES (@Name, @seat_total, @airplane_brand, @logo_url); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Name", airplane.Name);
                    command.Parameters.AddWithValue("@seat_total", airplane.seat_total);
                    command.Parameters.AddWithValue("@airplane_brand", airplane.airplane_brand);


                    // var file = student.ImagePath; // Get the uploaded file from the student object
                    var file = HttpContext.Request.Form.Files.FirstOrDefault();

                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        // Generate a unique file name
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                        // Set the file path where the image will be saved on the server
                        var filePath = Path.Combine("uploads", uniqueFileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        command.Parameters.AddWithValue("@logo_url", filePath);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@logo_url", DBNull.Value);
                    }

                    // Execute the SQL command and get the inserted student ID
                    int insertedAirplaneId = Convert.ToInt32(await command.ExecuteScalarAsync());

                    // Set the ID, name, age, and image path in the student object
                    airplane.airplane_id = insertedAirplaneId;

                }
            }

            return Ok(airplane);
        }
    }
}
