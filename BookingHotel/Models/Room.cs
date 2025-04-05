using System.ComponentModel.DataAnnotations.Schema;

namespace BookingHotel.Models
{
    public class Room
    {
        public long? RoomID { get; set; }
        public string Number { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
        public string Class { get; set; } = String.Empty;
    }

}
