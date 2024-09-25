using EventHandler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHandler.Seed
{
    public class CategorySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Conference",
                    Slug = "conference"
                },
                new Category
                {
                    Id = 2,
                    Name = "WorkShop",
                    Slug = "workshop"
                },
                new Category
                {
                    Id = 3,
                    Name = "Concert",
                    Slug = "concert"
                }
             );
        }
    }
}
