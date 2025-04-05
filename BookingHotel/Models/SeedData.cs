using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;


namespace BookingHotel.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            BookingDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<BookingDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Rooms.Any())
            {
                context.Rooms.AddRange(
                new Room
                {
                    Number = "101",
                    Description = "Broken shower",
                    Price = 275,
                    Class = "Lux"
                },
                new Room
                {
                    Number = "101a",
                    Description = "No description",
                    Price = 150,
                    Class = "Normal"
                },
                new Room
                {
                    Number = "230",
                    Description = "Broken TV",
                    Price = 100,
                    Class = "Cheap"
                },
                new Room
                {
                    Number = "134",
                    Description = "No description",
                    Price = 400,
                    Class = "Super Lux"
                });
                context.SaveChanges();
            }
        }
    }
}
