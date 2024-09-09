using EventHandler.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventHandler.Data
{
    public class EventDbContext : IdentityDbContext<AppUser>
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {

        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Category> Categorys { get; set; }

        public DbSet<Requests> requests { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
        .HasMany(e => e.Events)
        .WithOne(e => e.Organizer)
        .HasForeignKey(e => e.OrganizerId)
        .OnDelete(DeleteBehavior.Cascade); 

            // AppUser to Tickets (Cascade delete allowed)
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Tickets)
                .WithOne(t => t.AppUser)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Ticket to Event (No cascade delete to avoid conflict)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Restrict);  

            // Ticket to AppUser (Cascade delete allowed)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AppUser)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);  

            // AppUser to Requests (Cascade delete allowed)
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Requests)
                .WithOne(r => r.AppUser)
                .HasForeignKey<Requests>(r => r.userId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Category to Events (No cascade delete to avoid conflict)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Events)
                .WithOne(e => e.category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);  

            
            modelBuilder.Entity<Event>()
                .HasMany(e => e.tickets)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Requests>()
                .HasKey(r => r.ReqId);

           
            modelBuilder.Entity<Ticket>()
                .Property(t => t.TicketName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Quantity)
                .IsRequired();

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(50);
                


        }



    }
}
