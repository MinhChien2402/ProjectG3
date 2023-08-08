namespace ProjectG3.Models
{
    public class Ticket
    {
        public int ticket_id {  get; set; }
        public int flight_id { get; set; }
        public int customer_id { get; set; }
        public int seat_id { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set;}
        public DateTime departure_time { get; set; }
        public DateTime arrival_time { get; set; }
        public DateTime real_time_flight { get; set; }
        public string ticket_status { get; set; }
        public decimal price { get; set; }
    }
}
