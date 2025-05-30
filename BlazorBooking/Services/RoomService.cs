using BookingHotel.Models;
using BlazorClient.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlazorClient.Services
{
    public interface IRoomService
    {
        Task<List<Room>> GetRoomsAsync();
        Task<Room?> GetRoomAsync(long id);
        Task<bool> CreateRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(long id, Room room);
        Task<bool> DeleteRoomAsync(long id);
    }

    public class RoomService : IRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public RoomService(HttpClient httpClient, IAuthService authService)
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

        public async Task<List<Room>> GetRoomsAsync()
        {
            await SetAuthorizationHeader();
            try
            {
                var rooms = await _httpClient.GetFromJsonAsync<List<Room>>("api/RoomsApi");
                return rooms ?? new List<Room>();
            }
            catch
            {
                return new List<Room>();
            }
        }

        public async Task<Room?> GetRoomAsync(long id)
        {
            await SetAuthorizationHeader();
            try
            {
                return await _httpClient.GetFromJsonAsync<Room>($"api/RoomsApi/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateRoomAsync(Room room)
        {
            await SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/RoomsApi", room);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRoomAsync(long id, Room room)
        {
            await SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/RoomsApi/{id}", room);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRoomAsync(long id)
        {
            await SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.DeleteAsync($"api/RoomsApi/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}