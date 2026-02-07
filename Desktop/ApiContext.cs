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
    public class ApiContext
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7237/api/"; // Из YP-RestAPI, настрой по своему

        public ApiContext()
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<bool> LoginAdminAsync(string login, string password)
        {
            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("login", login),
                    new KeyValuePair<string, string>("password", password)
                });
                var response = await _httpClient.PostAsync(_baseUrl + "AdministratorController/LoginAdministrator", content);
                if (response.IsSuccessStatusCode)
                {
                    var admin = JsonConvert.DeserializeObject<Administrator>(await response.Content.ReadAsStringAsync());
                    App.SetAdminData(login, admin.Name);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Logout()
        {
            // Очистка, если нужно токен или сессия
        }

        // Users methods
        public async Task<List<User>> GetAllUsersAsync() => await GetAsync<List<User>>("UserController/GetAllUsers");
        public async Task<User> GetUserByIdAsync(int id) => await GetAsync<User>($"UserController/GetUserById/{id}");
        public async Task CreateUserAsync(User user) => await PostAsync("UserController/AddUser", user);
        public async Task UpdateUserAsync(User user) => await PostAsync("UserController/UpdateUser", user);
        public async Task<bool> DeleteUserAsync(int id) => await DeleteAsync($"UserController/DeleteUser/{id}");

        // Restaurants
        public async Task<List<Restaurant>> GetAllRestaurantsAsync() => await GetAsync<List<Restaurant>>("RestaurantController/GetAllRestaurants");
        public async Task<Restaurant> GetRestaurantByIdAsync(int id) => await GetAsync<Restaurant>($"RestaurantController/GetRestaurantById/{id}");
        public async Task CreateRestaurantAsync(Restaurant restaurant) => await PostAsync("RestaurantController/CreateRestaurant", restaurant);
        public async Task UpdateRestaurantAsync(Restaurant restaurant) => await PostAsync("RestaurantController/UpdateRestaurant", restaurant);
        public async Task<bool> DeleteRestaurantAsync(int id) => await DeleteAsync($"RestaurantController/DeleteRestaurant/{id}");

        // Dishes
        public async Task<List<Dish>> GetAllDishesAsync() => await GetAsync<List<Dish>>("DishController/GetAllDishes");
        public async Task<Dish> GetDishByIdAsync(int id) => await GetAsync<Dish>($"DishController/GetDishById/{id}");
        public async Task CreateDishAsync(Dish dish) => await PostAsync("DishController/CreateDish", dish);
        public async Task UpdateDishAsync(Dish dish) => await PostAsync("DishController/UpdateDish", dish);
        public async Task<bool> DeleteDishAsync(int id) => await DeleteAsync($"DishController/DeleteDish/{id}");

        // Bookings
        public async Task<List<Booking>> GetAllBookingsAsync() => await GetAsync<List<Booking>>("BookingController/GetAllBookings");
        public async Task<Booking> GetBookingByIdAsync(int id) => await GetAsync<Booking>($"BookingController/GetBookingById/{id}");
        public async Task CreateBookingAsync(Booking booking) => await PostAsync("BookingController/CreateBooking", booking);
        public async Task UpdateBookingAsync(Booking booking) => await PostAsync("BookingController/UpdateBooking", booking);
        public async Task<bool> DeleteBookingAsync(int id) => await DeleteAsync($"BookingController/DeleteBooking/{id}");

        // Admins (similar)
        public async Task<List<Administrator>> GetAllAdminsAsync() => await GetAsync<List<Administrator>>("AdministratorController/GetAllAdmins");
        // ... другие методы для админов, если нужно

        // Generic methods (как в YP-master)
        private async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(_baseUrl + endpoint);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private async Task PostAsync(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl + endpoint, content);
            response.EnsureSuccessStatusCode();
        }

        private async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(_baseUrl + endpoint);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<Log>> GetAllLogsAsync()
        {
            return await GetAsync<List<Log>>("UserController/GetAllLogs"); // Убедитесь, что в UserController есть такой метод
        }
    }
}