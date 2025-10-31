using System;
using System.Collections.Generic;
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

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RestaurantsPage.xaml
    /// </summary>
    public partial class RestaurantsPage : Page
    {
        private List<Restaurant> _restaurants = new List<Restaurant>();
        public RestaurantsPage()
        {
            InitializeComponent();
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            _restaurants = new List<Restaurant>
            {
                new Restaurant {
                    Id = 1,
                    Name = "Итальянский дворик",
                    Address = "ул. Пушкина, 1",
                    Capacity = 50,
                    OpenTime = "10:00",
                    CloseTime = "23:00",
                    Tematic = "Итальянская кухня"
                },
                new Restaurant {
                    Id = 2,
                    Name = "Суши-бар Токио",
                    Address = "ул. Лермонтова, 15",
                    Capacity = 30,
                    OpenTime = "11:00",
                    CloseTime = "22:00",
                    Tematic = "Японская кухня"
                },
                new Restaurant {
                    Id = 3,
                    Name = "Гриль-хаус",
                    Address = "пр. Мира, 25",
                    Capacity = 80,
                    OpenTime = "12:00",
                    CloseTime = "24:00",
                    Tematic = "Американская кухня"
                }
            };

            RestaurantsGrid.ItemsSource = _restaurants;
        }

        private void BtnAddRestaurant_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("Функция добавления ресторана", "Информация");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRestaurants();
            ShowMessage("Список ресторанов обновлен", "Успех");
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = (sender as Button)?.DataContext as Restaurant;
            if (restaurant != null)
            {
                ShowMessage($"Редактирование: {restaurant.Name}", "Информация");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = (sender as Button)?.DataContext as Restaurant;
            if (restaurant != null)
            {
                var result = ShowConfirmation($"Удалить ресторан '{restaurant.Name}'?");
                if (result == MessageBoxResult.Yes)
                {
                    _restaurants.RemoveAll(r => r.Id == restaurant.Id);
                    RestaurantsGrid.ItemsSource = null;
                    RestaurantsGrid.ItemsSource = _restaurants;
                    ShowMessage("Ресторан удален", "Успех");
                }
            }
        }

        private void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private MessageBoxResult ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}