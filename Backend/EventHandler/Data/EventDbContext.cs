using EventHandler.Models.Entities;
using EventHandler.Seed;
using Microsoft.AspNetCore.Identity;
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

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Buyer", NormalizedName = "BUYER" },
                new IdentityRole { Id = "3", Name = "Organiser", NormalizedName = "ORGANISER" }
            );

            var hasher = new PasswordHasher<AppUser>();

            var organiserUser = new AppUser
            {
                Id = "organiser-id-001",
                FirstName = "John",
                LastName = "Doe",
                NIC = "123456789321456",
                UserName = "organiser",
                NormalizedUserName = "ORGANISER",
                Email = "organiser@example.com",
                NormalizedEmail = "ORGANISER@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Organiser@123") 

            };

            modelBuilder.Entity<AppUser>().HasData(organiserUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>
               {
                   UserId = "organiser-id-001",  
                   RoleId = "3"                 
               }
            );

            var adminUser = new AppUser
            {
                Id = "Admin-id-001",
                FirstName = "kavindu",
                LastName = "dilshan",
                NIC = "2001065080956",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123")

            };

            modelBuilder.Entity<AppUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId= "Admin-id-001",
                    RoleId = "1"
                }
                );



            CategorySeed.Seed(modelBuilder);
        }



    }
}
