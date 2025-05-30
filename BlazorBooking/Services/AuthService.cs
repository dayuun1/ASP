using BookingHotel.Models;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorClient.Services
{
    public interface IAuthService
    {
        Task<bool> Login(UserLoginModel loginModel);
        Task<bool> Register(UserRegistrationModel registerModel);
        Task Logout();
        Task<string?> GetToken();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(UserLoginModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/login", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (tokenResponse?.Token != null)
                    {
                        await _localStorage.SetItemAsync("authToken", tokenResponse.Token);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Register(UserRegistrationModel registerModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/register", registerModel);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }

        public async Task<string?> GetToken()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}