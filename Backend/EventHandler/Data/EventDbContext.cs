using EventHandler.Models.Entities;
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

        public DbSet<UserUserRoles> UserUserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => ur.Id);
                entity.Property(ur => ur.RoleName).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TicketName).IsRequired().HasMaxLength(100);
                entity.Property(t => t.Price).HasPrecision(18, 2);
                entity.Property(t => t.Quantity).IsRequired(); 
            });

            modelBuilder.Entity<EventAttendance>(entity =>
            {
                entity.HasKey(ea => ea.Id);

                entity.Property(ea => ea.CheckedIn).IsRequired();

                entity.HasOne(ea => ea.Event)
                      .WithMany(e => e.Attendances)
                      .HasForeignKey(ea => ea.EventId)
                      .OnDelete(DeleteBehavior.Restrict); 

                entity.HasOne(ea => ea.User)
                      .WithMany(u => u.Attendances)
                      .HasForeignKey(ea => ea.UserId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Message).IsRequired();
                entity.Property(n => n.SentAt).IsRequired();
                entity.Property(n => n.ReadStatus).IsRequired();

                // Configure foreign keys
                entity.HasOne(n => n.Event)
                      .WithMany(e => e.notifications)
                      .HasForeignKey(n => n.EventId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.User)
                      .WithMany(u => u.notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Price).HasColumnType("decimal(18,2)"); // Ensure precision and scale
                entity.HasOne(t => t.Event)
                      .WithMany(e => e.tickets)
                      .HasForeignKey(t => t.EventId)
                      .OnDelete(DeleteBehavior.Restrict); // Use Restrict here
                entity.HasOne(t => t.User)
                      .WithMany(u => u.tickets)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Use Restrict here
            });



            modelBuilder.Entity<UserUserRoles>()
            .HasKey(uur => new { uur.UserId, uur.UserRoleId }); 

            modelBuilder.Entity<UserUserRoles>()
                .HasOne(uur => uur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(uur => uur.UserId);

            modelBuilder.Entity<UserUserRoles>()
                .HasOne(uur => uur.UserRole)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(uur => uur.UserRoleId);
        }



    }
}
