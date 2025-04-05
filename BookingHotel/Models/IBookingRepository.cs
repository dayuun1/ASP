namespace BookingHotel.Models
{
    public interface IBookingRepository
    {
        IQueryable<Room> Rooms { get; }
    }

}
