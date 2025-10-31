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
using Desktop.Pages.Items;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RestaurantsPage.xaml
    /// </summary>
    public partial class RestaurantsPage : Page
    {
        private Restaurant _selectedRestaurant;
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
                RestaurantsItemsControl.ItemsSource = restaurants;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RestaurantCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is RestaurantCard card)
            {
                // Снимаем выделение со всех карточек
                foreach (var item in RestaurantsItemsControl.Items)
                {
                    if (item is Restaurant restaurant)
                    {
                        // Можно добавить логику снятия выделения если нужно
                    }
                }

                _selectedRestaurant = card.Restaurant;

                // Выделяем выбранный элемент в DataGrid
                RestaurantsGrid.SelectedItem = _selectedRestaurant;
            }
        }

        private Restaurant GetSelectedRestaurant()
        {
            return _selectedRestaurant ?? RestaurantsGrid.SelectedItem as Restaurant;
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
                MessageBox.Show($"✅ Добавлен новый ресторан через API!\nID: #{createdRestaurant.Id}\nНазвание: {createdRestaurant.Name}",
                              "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadRestaurantsFromApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка добавления ресторана: {ex.Message}", "Ошибка",
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
                    MessageBox.Show($"✏️ Ресторан обновлен через API!\nID: #{updatedRestaurant.Id}\nНовое название: {updatedRestaurant.Name}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadRestaurantsFromApi();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Ошибка обновления ресторана: {ex.Message}", "Ошибка",
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
                            MessageBox.Show($"🗑️ Ресторан удален через API!\nID: #{restaurant.Id}\nНазвание: {restaurant.Name}",
                                          "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadRestaurantsFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Ошибка удаления ресторана: {ex.Message}", "Ошибка",
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
            _selectedRestaurant = RestaurantsGrid.SelectedItem as Restaurant;
        }
    }
}