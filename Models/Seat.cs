namespace ProjectG3.Models
{
    public class Seat
    {
        public int seat_id { get; set; }
        public int airplane_id { get; set; }
        public int seat_total { get; set; }
        public string seat_type { get; set; }
        public string seat_status { get; set; }
        public decimal price { get; set; }
    }
}
