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
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private bool _disposed = false;

        public ApiClient(string baseUrl = "http://localhost:5000/api/")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private async Task<bool> IsServerReachable()
        {
            try
            {
                // Попробуем получить базовый URL без /api/
                var baseUri = new Uri(_baseUrl);
                var healthCheckUri = new Uri(baseUri, "../health"); // или просто базовый URL
                var response = await _httpClient.GetAsync(healthCheckUri);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Если health endpoint недоступен, попробуем просто подключиться к API
                try
                {
                    var response = await _httpClient.GetAsync(_baseUrl);
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }
        }

        private async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                if (!await IsServerReachable())
                {
                    throw new Exception("Сервер недоступен. Проверьте подключение к сети и запущен ли сервер.");
                }

                var response = await _httpClient.GetAsync(_baseUrl + endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}. Content: {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(content);
                    if (apiResponse != null && apiResponse.Success)
                        return apiResponse.Data;
                    else
                        throw new Exception(apiResponse?.Message ?? "Unknown API error");
                }
                catch (JsonException)
                {
                    // Если не ApiResponse, пробуем как прямой объект
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка подключения к API: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Исправление: проверяем Timeout через _httpClient.Timeout
                throw new Exception($"Таймаут подключения ({_httpClient.Timeout.TotalSeconds} сек)");
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
                if (!await IsServerReachable())
                {
                    throw new Exception("Сервер недоступен. Проверьте подключение к сети и запущен ли сервер.");
                }

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_baseUrl + endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}. Content: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);
                    if (apiResponse != null && apiResponse.Success)
                        return apiResponse.Data;
                    else
                        throw new Exception(apiResponse?.Message ?? "Unknown API error");
                }
                catch (JsonException)
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch (TaskCanceledException)
            {
                throw new Exception($"Таймаут подключения ({_httpClient.Timeout.TotalSeconds} сек)");
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
                if (!await IsServerReachable())
                {
                    throw new Exception("Сервер недоступен. Проверьте подключение к сети и запущен ли сервер.");
                }

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(_baseUrl + endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}. Content: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);
                    if (apiResponse != null && apiResponse.Success)
                        return apiResponse.Data;
                    else
                        throw new Exception(apiResponse?.Message ?? "Unknown API error");
                }
                catch (JsonException)
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch (TaskCanceledException)
            {
                throw new Exception($"Таймаут подключения ({_httpClient.Timeout.TotalSeconds} сек)");
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
                if (!await IsServerReachable())
                {
                    throw new Exception("Сервер недоступен. Проверьте подключение к сети и запущен ли сервер.");
                }

                var response = await _httpClient.DeleteAsync(_baseUrl + endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseContent);
                        return apiResponse?.Success ?? response.IsSuccessStatusCode;
                    }
                    catch
                    {
                        return response.IsSuccessStatusCode;
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}. Content: {errorContent}");
            }
            catch (TaskCanceledException)
            {
                throw new Exception($"Таймаут подключения ({_httpClient.Timeout.TotalSeconds} сек)");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка API: {ex.Message}");
            }
        }

        // Users
        public async Task<List<User>> GetUsersAsync() => await GetAsync<List<User>>("users");
        public async Task<User> GetUserAsync(int id) => await GetAsync<User>($"users/{id}");
        public async Task<User> CreateUserAsync(User user) => await PostAsync<User>("users", user);
        public async Task<User> UpdateUserAsync(int id, User user) => await PutAsync<User>($"users/{id}", user);
        public async Task<bool> DeleteUserAsync(int id) => await DeleteAsync($"users/{id}");

        // Restaurants
        public async Task<List<Restaurant>> GetRestaurantsAsync() => await GetAsync<List<Restaurant>>("restaurants");
        public async Task<Restaurant> GetRestaurantAsync(int id) => await GetAsync<Restaurant>($"restaurants/{id}");
        public async Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant) => await PostAsync<Restaurant>("restaurants", restaurant);
        public async Task<Restaurant> UpdateRestaurantAsync(int id, Restaurant restaurant) => await PutAsync<Restaurant>($"restaurants/{id}", restaurant);
        public async Task<bool> DeleteRestaurantAsync(int id) => await DeleteAsync($"restaurants/{id}");

        // Dishes
        public async Task<List<Dish>> GetDishesAsync() => await GetAsync<List<Dish>>("dishes");
        public async Task<Dish> GetDishAsync(int id) => await GetAsync<Dish>($"dishes/{id}");
        public async Task<Dish> CreateDishAsync(Dish dish) => await PostAsync<Dish>("dishes", dish);
        public async Task<Dish> UpdateDishAsync(int id, Dish dish) => await PutAsync<Dish>($"dishes/{id}", dish);
        public async Task<bool> DeleteDishAsync(int id) => await DeleteAsync($"dishes/{id}");

        // Bookings
        public async Task<List<Booking>> GetBookingsAsync() => await GetAsync<List<Booking>>("bookings");
        public async Task<Booking> GetBookingAsync(int id) => await GetAsync<Booking>($"bookings/{id}");
        public async Task<Booking> CreateBookingAsync(Booking booking) => await PostAsync<Booking>("bookings", booking);
        public async Task<Booking> UpdateBookingAsync(int id, Booking booking) => await PutAsync<Booking>($"bookings/{id}", booking);
        public async Task<bool> DeleteBookingAsync(int id) => await DeleteAsync($"bookings/{id}");

        // Auth
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _httpClient?.Dispose();
            }
            _disposed = true;
        }

        ~ApiClient()
        {
            Dispose(false);
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}