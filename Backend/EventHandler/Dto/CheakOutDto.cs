namespace EventHandler.Dto
{
    public class CheckoutDto
    {
        public int TiketId { get; set; }
        public int EventId { get; set; }
        public string userID { get; set; }
        public string EventName { get; set; }
        public string description { get; set; }
        public long TicketPrice { get; set; } 
        public int Quantity { get; set; } 
    }

}
