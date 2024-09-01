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
