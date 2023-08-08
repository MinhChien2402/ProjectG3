using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectG3.Models;
using ProjectG3.ViewModel;
using System.Data.SqlClient;

namespace ProjectG3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        //string _connectionString = "Server=tcp:arss.database.windows.net,1433;Database=ARSS;User Id=admindb;Password=01Aibiet@;";
        private readonly string _connectionString = "Server=LAPTOP-QF0MBSMD;Database=ARS;Trusted_Connection=True;";

        [HttpGet("getall")]
        public async Task<IEnumerable<FlightVM>> GetFlights()
        {
            var ltflights = new List<FlightVM>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "SELECT * FROM Flight f JOIN Location I ON f.location_id = I.location_id JOIN Airplane a ON a.airplane_id = f.airplane_id;";

                using (var command = new SqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            //// Khai báo biến để lưu giá trị đọc từ cơ sở dữ liệu
                            //int airplane_id;
                            //int location_id;

                            //// Kiểm tra xem cột "airplane_id" có giá trị null hay không
                            //if (!reader.IsDBNull(reader.GetOrdinal("airplane_id")))
                            //{
                            //    airplane_id = reader.GetInt32(reader.GetOrdinal("airplane_id"));
                            //}
                            //else
                            //{
                            //    // Xử lý khi giá trị null (trong trường hợp này, bạn có thể gán một giá trị mặc định hoặc làm gì đó phù hợp với logic của ứng dụng)
                            //    // Ví dụ:
                            //    airplane_id = 0; // Gán giá trị mặc định là 0
                            //}

                            //// Kiểm tra xem cột "location_id" có giá trị null hay không
                            //if (!reader.IsDBNull(reader.GetOrdinal("location_id")))
                            //{
                            //    location_id = reader.GetInt32(reader.GetOrdinal("location_id"));
                            //}
                            //else
                            //{
                            //    // Xử lý khi giá trị null (tương tự như trên, bạn có thể gán giá trị mặc định hoặc xử lý theo logic ứng dụng)
                            //    // Ví dụ:
                            //    location_id = 0; 
                            //}
                            var flight = new FlightVM
                            {

                                //flight_id = reader.GetInt32(reader.GetOrdinal("flight_id")),
                                //airplane_id = reader.GetInt32(reader.GetOrdinal("airplane_id")),
                                //location_id = reader.GetInt32(reader.GetOrdinal("location_id")),
                                //start_location = reader.GetString(reader.GetOrdinal("start_location")),
                                //end_location = reader.GetString(reader.GetOrdinal("end_location")),
                                //time_start = reader.GetDateTime(reader.GetOrdinal("time_start")),
                                //time_end = reader.GetDateTime(reader.GetOrdinal("time_end")),
                                //time_start_ = reader.GetDateTime(reader.GetOrdinal("time_start")).ToString("HH:mm:ss"),
                                //time_end_ = reader.GetDateTime(reader.GetOrdinal("time_end")).ToString("HH:mm:ss"),
                                //real_time_flight_ = reader.GetDateTime(reader.GetOrdinal("real_time_flight")).ToString("HH:mm:ss"),
                                //flight_status = reader.GetString(reader.GetOrdinal("flight_status")),
                                flight_id = reader.GetInt32(reader.GetOrdinal("flight_id")),
                                airplane_id = reader.GetInt32(reader.GetOrdinal("airplane_id")),
                                location_id = reader.GetInt32(reader.GetOrdinal("location_id")),
                                start_location = reader.GetString(reader.GetOrdinal("start_location")),
                                end_location = reader.GetString(reader.GetOrdinal("end_location")),
                                time_start = reader.GetDateTime(reader.GetOrdinal("time_start")),
                                time_end = reader.GetDateTime(reader.GetOrdinal("time_end")),
                                flight_status = reader.GetString(reader.GetOrdinal("flight_status")),
                            };
                            // Calculate the real_time_flight duration
                            TimeSpan flightDuration = flight.time_end - flight.time_start;
                            flight.real_time_flight = flightDuration;


                            // Convert time_start, time_end, and real_time_flight to formatted strings
                            flight.time_start_ = flight.time_start.ToString("HH:mm:ss");
                            flight.time_end_ = flight.time_end.ToString("HH:mm:ss");
                            flight.real_time_flight_ = flight.real_time_flight.ToString(@"hh\:mm\:ss");

                            ltflights.Add(flight);
                        }
                    }

                }
            }
            return ltflights;
        }


        [HttpPost("insert")]
        public async Task<IActionResult> InsertFlight(Flight flight)
        {


            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "INSERT INTO Flight (airplane_id, location_id, start_location, end_location, time_start,time_end,real_time_flight, flight_status) " +
                    "VALUES (@airplane_id,@location_id, @start_location, @end_location, @time_start, @time_end, @real_time_flight, @flight_status);";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@airplane_id", flight.airplane_id);
                    command.Parameters.AddWithValue("@location_id", flight.location_id);
                    command.Parameters.AddWithValue("@start_location", flight.start_location);
                    command.Parameters.AddWithValue("@end_location", flight.end_location);
                    command.Parameters.AddWithValue("@time_start", flight.time_start);
                    command.Parameters.AddWithValue("@time_end", flight.time_end);
                    command.Parameters.AddWithValue("@real_time_flight", flight.real_time_flight);
                    command.Parameters.AddWithValue("@flight_status", flight.flight_status);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateFlight(Flight flight)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var commandText = "UPDATE Flight SET airplane_id = @airplane_id, location_id = @location_id, start_location = @start_location, end_location = @end_location ," +
                    "time_start = @time_start, time_end = @time_end, real_time_flight = @real_time_flight, flight_status = @flight_status " +
                    "WHERE flight_id = @flight_id";



                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@airplane_id", flight.airplane_id);
                    command.Parameters.AddWithValue("@location_id", flight.location_id);
                    command.Parameters.AddWithValue("@start_location", flight.start_location);
                    command.Parameters.AddWithValue("@end_location", flight.end_location);
                    command.Parameters.AddWithValue("@time_start", flight.time_start);
                    command.Parameters.AddWithValue("@time_end", flight.time_end);
                    command.Parameters.AddWithValue("@real_time_flight", flight.real_time_flight);
                    command.Parameters.AddWithValue("@flight_status", flight.flight_status);
                    command.Parameters.AddWithValue("@flight_id", flight.flight_id);


                    await command.ExecuteNonQueryAsync();
                }
            }
            return Ok();
        }
    }
}
