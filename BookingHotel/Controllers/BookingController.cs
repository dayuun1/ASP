using BookingHotel.Extensions;
using BookingHotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository repository;

        public BookingController(IBookingRepository repo)
        {
            repository = repo;
        }   
        private const string SessionKey = "BookingInfo";

        
        [HttpPost]
        public IActionResult AddToBooking(int roomId, string guestName, DateTime dateFrom, DateTime dateTo)
        {
            var booking = HttpContext.Session.GetObject<List<BookingInfo>>(SessionKey) ?? new List<BookingInfo>();

            booking.Add(new BookingInfo
            {
                RoomID = roomId,
                GuestName = guestName,
                DateFrom = dateFrom,
                DateTo = dateTo
            });

            HttpContext.Session.SetObject(SessionKey, booking);
            return RedirectToAction("ShowBooking");
        }

        public IActionResult ShowBooking()
        {
            var booking = HttpContext.Session.GetObject<List<BookingInfo>>(SessionKey) ?? new List<BookingInfo>();
            return View(booking);
        }

        public IActionResult Pay()
        {
            var booking = HttpContext.Session.GetObject<List<BookingInfo>>(SessionKey);
            HttpContext.Session.Remove(SessionKey);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ClearBooking()
        {
            HttpContext.Session.Remove(SessionKey);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            var bookings = repository.Bookings.Include(b => b.Room).ToList();
            return View(bookings);
        }

        public IActionResult Details(long id)
        {
            var booking = repository.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        public IActionResult Create()
        {
            ViewBag.Rooms = repository.Rooms.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                bool isOverlapping = repository.Bookings.Any(b =>
                    b.RoomID == booking.RoomID &&
                    (
                        (booking.DateFrom >= b.DateFrom && booking.DateFrom < b.DateTo) ||
                        (booking.DateTo > b.DateFrom && booking.DateTo <= b.DateTo) ||
                        (booking.DateFrom <= b.DateFrom && booking.DateTo >= b.DateTo)
                    )
                );

                if (isOverlapping)
                {
                    ModelState.AddModelError("", "Ця кімната вже заброньована на вибрані дати.");
                }
                else
                {
                    repository.CreateBooking(booking);
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Rooms = repository.Rooms.ToList();
            return View(booking);
        }

        public IActionResult Edit(long id)
        {
            var booking = repository.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (booking == null) return NotFound();
            ViewBag.Rooms = repository.Rooms.ToList();
            return View(booking);
        }

        [HttpPost]
        public IActionResult Edit(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var existingBooking = repository.Bookings.FirstOrDefault(b => b.BookingID == booking.BookingID);
                if (existingBooking != null)
                {
                    existingBooking.GuestName = booking.GuestName;
                    existingBooking.DateFrom = booking.DateFrom;
                    existingBooking.DateTo = booking.DateTo;
                    existingBooking.RoomID = booking.RoomID;
                    repository.UpdateBooking(existingBooking);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Rooms = repository.Rooms.ToList();
            return View(booking);
        }

        public IActionResult Delete(long id)
        {
            var booking = repository.Bookings.Include(b => b.Room).FirstOrDefault(b => b.BookingID == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(long id)
        {
            var booking = repository.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (booking != null)
            {
                repository.DeleteBooking(booking);
            }
            return RedirectToAction("Index");
        }
    }
}