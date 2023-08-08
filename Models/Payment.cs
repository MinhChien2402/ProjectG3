namespace ProjectG3.Models
{
    public class Payment
    {
        public int payment_id {  get; set; }
        public int booking_id { get; set; }
        public int customer_id { get; set; }
        public int ticket_id { get; set; }
        public DateTime payment_time { get; set; }
        public string payment_method { get; set; }
        public DateTime create_at { get; set; }
    }
}
