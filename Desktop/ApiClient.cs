using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Desktop.Models;


namespace Desktop
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl = "http://localhost:5000/api/")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl + endpoint);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse<T>>(content).Data;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка подключения к API: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка API: {ex.Message}");
            }
        }

        private async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_baseUrl + endpoint, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent).Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка API: {ex.Message}");
            }
        }

        private async Task<T> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(_baseUrl + endpoint, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent).Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка API: {ex.Message}");
            }
        }

        private async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_baseUrl + endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка API: {ex.Message}");
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await GetAsync<List<User>>("users");
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await GetAsync<User>($"users/{id}");
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await PostAsync<User>("users", user);
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            return await PutAsync<User>($"users/{id}", user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await DeleteAsync($"users/{id}");
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return await GetAsync<List<Restaurant>>("restaurants");
        }

        public async Task<Restaurant> GetRestaurantAsync(int id)
        {
            return await GetAsync<Restaurant>($"restaurants/{id}");
        }

        public async Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant)
        {
            return await PostAsync<Restaurant>("restaurants", restaurant);
        }

        public async Task<Restaurant> UpdateRestaurantAsync(int id, Restaurant restaurant)
        {
            return await PutAsync<Restaurant>($"restaurants/{id}", restaurant);
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            return await DeleteAsync($"restaurants/{id}");
        }

        public async Task<List<Dish>> GetDishesAsync()
        {
            return await GetAsync<List<Dish>>("dishes");
        }

        public async Task<Dish> GetDishAsync(int id)
        {
            return await GetAsync<Dish>($"dishes/{id}");
        }

        public async Task<Dish> CreateDishAsync(Dish dish)
        {
            return await PostAsync<Dish>("dishes", dish);
        }

        public async Task<Dish> UpdateDishAsync(int id, Dish dish)
        {
            return await PutAsync<Dish>($"dishes/{id}", dish);
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            return await DeleteAsync($"dishes/{id}");
        }

        public async Task<List<Booking>> GetBookingsAsync()
        {
            return await GetAsync<List<Booking>>("bookings");
        }

        public async Task<Booking> GetBookingAsync(int id)
        {
            return await GetAsync<Booking>($"bookings/{id}");
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            return await PostAsync<Booking>("bookings", booking);
        }

        public async Task<Booking> UpdateBookingAsync(int id, Booking booking)
        {
            return await PutAsync<Booking>($"bookings/{id}", booking);
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            return await DeleteAsync($"bookings/{id}");
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var authData = new { email, password };
            return await PostAsync<User>("auth/login", authData);
        }

        public async Task<User> RegisterAsync(User user)
        {
            return await PostAsync<User>("auth/register", user);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

