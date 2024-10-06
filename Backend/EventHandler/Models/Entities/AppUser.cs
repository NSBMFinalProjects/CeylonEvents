using Microsoft.AspNetCore.Identity;

namespace EventHandler.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string NIC {  get; set; }

        public int reqId {  get; set; }

        public List<Event> Events { get; set; }

        public List<Ticket> Tickets { get; set; }

        public List<Purchase> Purchases { get; set; }

        public Requests Requests {  get; set; }

        public AppUser()
        {
            Purchases = new List<Purchase>(); 
        }
    }
}
