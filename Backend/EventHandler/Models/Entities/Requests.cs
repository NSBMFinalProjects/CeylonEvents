namespace EventHandler.Models.Entities
{
    public class Requests
    {
        public int ReqId { get; set; }

        public string reqDetails { get; set; }

        public string userId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
