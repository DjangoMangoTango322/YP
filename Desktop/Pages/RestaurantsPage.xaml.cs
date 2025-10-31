using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RestaurantsPage.xaml
    /// </summary>
    public partial class RestaurantsPage : Page
    {
        public RestaurantsPage()
        {
            InitializeComponent();
            LoadRestaurantsFromApi();
        }

        private async void LoadRestaurantsFromApi()
        {
            try
            {
                var restaurants = await App.ApiClient.GetRestaurantsAsync();
                RestaurantsGrid.ItemsSource = restaurants;
                UpdateOutput($"✅ Загружено {restaurants.Count} ресторанов из API");
            }
            catch (Exception ex)
            {
                UpdateOutput($"❌ Ошибка загрузки данных: {ex.Message}");
                MessageBox.Show($"Ошибка подключения к API: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateOutput(string message)
        {
            // Метод для обновления вывода
        }

        private Restaurant GetSelectedRestaurant()
        {
            return RestaurantsGrid.SelectedItem as Restaurant;
        }

        private async void BtnAddRestaurant_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newRestaurant = new Restaurant
                {
                    Name = "Новый ресторан",
                    Address = "Новый адрес",
                    Capacity = 50,
                    OpenTime = new TimeSpan(10, 0, 0),
                    CloseTime = new TimeSpan(22, 0, 0),
                    Tematic = "Общая"
                };

                var createdRestaurant = await App.ApiClient.CreateRestaurantAsync(newRestaurant);
                UpdateOutput($"✅ Добавлен новый ресторан через API:\nID: #{createdRestaurant.Id}\nНазвание: {createdRestaurant.Name}");
                LoadRestaurantsFromApi();
            }
            catch (Exception ex)
            {
                UpdateOutput($"❌ Ошибка добавления ресторана: {ex.Message}");
                MessageBox.Show($"Ошибка при добавлении ресторана: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = GetSelectedRestaurant();
            if (restaurant != null)
            {
                try
                {
                    restaurant.Name += " (изм.)";
                    restaurant.Capacity += 10;

                    var updatedRestaurant = await App.ApiClient.UpdateRestaurantAsync(restaurant.Id, restaurant);
                    UpdateOutput($"✏️ Ресторан обновлен через API:\nID: #{updatedRestaurant.Id}\nНовое название: {updatedRestaurant.Name}");
                    LoadRestaurantsFromApi();
                }
                catch (Exception ex)
                {
                    UpdateOutput($"❌ Ошибка обновления ресторана: {ex.Message}");
                    MessageBox.Show($"Ошибка при обновлении ресторана: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите ресторан для изменения", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = GetSelectedRestaurant();
            if (restaurant != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить ресторан \"{restaurant.Name}\"?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var success = await App.ApiClient.DeleteRestaurantAsync(restaurant.Id);
                        if (success)
                        {
                            UpdateOutput($"🗑️ Ресторан удален через API:\nID: #{restaurant.Id}\nНазвание: {restaurant.Name}");
                            LoadRestaurantsFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateOutput($"❌ Ошибка удаления ресторана: {ex.Message}");
                        MessageBox.Show($"Ошибка при удалении ресторана: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите ресторан для удаления", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRestaurantsFromApi();
        }

        private void RestaurantsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var restaurant = GetSelectedRestaurant();
            if (restaurant != null)
            {
                // Можно добавить вывод деталей в ItemOutputGrid
            }
        }
    }
}