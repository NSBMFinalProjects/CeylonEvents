using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventHandler.Models.Entities
{
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; } 

        public int? EventId { get; set; }

        public int TicketId { get; set; } 

        public int Quantity { get; set; } 

        public DateTime PurchaseDate { get; set; } 

       public Event Event { get; set; }
        public Ticket Ticket { get; set; }
        public AppUser User { get; set; }
    }

}
