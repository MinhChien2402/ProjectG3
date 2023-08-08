
namespace ProjectG3.ViewModel
{
    public class FlightVM
    {
        public int flight_id { get; set; }
        public int? airplane_id { get; set; }
        public int? location_id { get; set; }
        public string start_location { get; set; }
        public string end_location { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public TimeSpan real_time_flight { get; set; }
        public string flight_status { get; set; }
        public string time_start_ { get; set; }
        public string time_end_ { get; set; }
        public string real_time_flight_ { get; set; }

    }
}
