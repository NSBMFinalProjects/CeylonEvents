using Microsoft.EntityFrameworkCore;

namespace EventHandler.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {

        }

        public DbSet<Event> Events { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<EventAttendance> EventAttendances { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }  
    }
}
