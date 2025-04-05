using Microsoft.AspNetCore.Mvc;
using BookingHotel.Models;
using BookingHotel.Models.ViewModels;

namespace BookingHotel.Controllers
{
        public class HomeController : Controller
        {
            private IBookingRepository repository;

            public HomeController(IBookingRepository repo)
            {
                repository = repo;
            }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 2; 

            var rooms = repository.Rooms
                .OrderBy(r => r.RoomID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var model = new ListViewModel
            {
                Rooms = rooms, 
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Rooms.Count()
                }
            };

            return View(model);
        }
    }
}
