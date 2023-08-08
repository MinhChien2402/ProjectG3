namespace ProjectG3.Models
{
    public class Location
    {
        public int location_id {  get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int? parentId { get; set; }
    }
}
