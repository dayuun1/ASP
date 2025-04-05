using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingHotel.Models
{
    public class EFBookingRepository : IBookingRepository
    {
        private BookingDbContext context;
        public EFBookingRepository(BookingDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Room> Rooms => context.Rooms;
    }

}
