using Microsoft.EntityFrameworkCore;

public static class EventSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                Id = 1,
                OrganizerId = "acb56586-313a-423c-9027-55b8d1f04c4e",
                EventName = "Tech Conference 2024",
                Description = "A conference for tech enthusiasts.",
                StartDate = new DateTime(2024, 10, 15),
                EndDate = new DateTime(2024, 10, 17),
                Location = "New York",
                CategoryId = 1
            },
            new Event
            {
                Id = 2,
                OrganizerId = "acb56586-313a-423c-9027-55b8d1f04c4e",
                EventName = "Design Workshop",
                Description = "A workshop for aspiring designers.",
                StartDate = new DateTime(2024, 11, 10),
                EndDate = new DateTime(2024, 11, 12),
                Location = "San Francisco",
                CategoryId = 2
            }
        );
    }
}
