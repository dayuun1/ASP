using BookingHotel.Models;
using BlazorClient.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlazorClient.Services
{
    public interface IBookingService
    {
        Task<List<Booking>> GetBookingsAsync();
        Task<Booking?> GetBookingAsync(long id);
        Task<bool> CreateBookingAsync(Booking booking);
        Task<bool> UpdateBookingAsync(long id, Booking booking);
        Task<bool> DeleteBookingAsync(long id);
    }

    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public BookingService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await _authService.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Booking>> GetBookingsAsync()
        {
            await SetAuthorizationHeader();
            try
            {
                var bookings = await _httpClient.GetFromJsonAsync<List<Booking>>("api/Bookings");
                return bookings ?? new List<Booking>();
            }
            catch
            {
                return new List<Booking>();
            }
        }

        public async Task<Booking?> GetBookingAsync(long id)
        {
            await SetAuthorizationHeader();
            try
            {
                return await _httpClient.GetFromJsonAsync<Booking>($"api/Bookings/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            await SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Bookings", booking);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBookingAsync(long id, Booking booking)
        {
            await SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Bookings/{id}", booking);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBookingAsync(long id)
        {
            await SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Bookings/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}