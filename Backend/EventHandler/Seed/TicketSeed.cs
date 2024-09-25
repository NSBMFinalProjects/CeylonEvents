using Microsoft.EntityFrameworkCore;

public static class TicketSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                Id = 1,
                EventId = 1,
                UserId = "42259ee0-748e-4bb6-9601-17bd762abdbf",
                TicketName = "VIP Pass",
                Quantity = 1,
                Price = 299.99M,
                PurchaseDate = DateTime.Now,
                Status = "Purchased"
            },
            new Ticket
            {
                Id = 2,
                EventId = 1,
                UserId = "42259ee0-748e-4bb6-9601-17bd762abdbf",
                TicketName = "General Admission",
                Quantity = 2,
                Price = 99.99M,
                PurchaseDate = DateTime.Now,
                Status = "Purchased"
            },
            new Ticket
            {
                Id = 3,
                EventId = 2,
                UserId = "42259ee0-748e-4bb6-9601-17bd762abdbf",
                TicketName = "Workshop Ticket",
                Quantity = 1,
                Price = 199.99M,
                PurchaseDate = DateTime.Now,
                Status = "Purchased"
            }
        );
    }
}
