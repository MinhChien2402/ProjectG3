namespace ProjectG3.Models
{
    public class Flight
    {
        public int flight_id { get; set; }
        public int airplane_id { get; set; }
        public int location_id { get; set; }
        public string start_location { get; set; }
        public string end_location { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public DateTime real_time_flight { get; set; }
        public string flight_status { get; set; }
    }
}
