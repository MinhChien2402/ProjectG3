namespace ProjectG3.Models
{
    public class Booking
    {
        public int booking_id { get; set; }
        public int ticket_id { get; set; }
        public int customer_id { get; set; }
        public DateTime time_oder { get; set; }
        public string booking_status { get; set; }
    }
}
