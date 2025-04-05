using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Models
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }
        public DbSet<Room> Rooms => Set<Room>();
    }
}

