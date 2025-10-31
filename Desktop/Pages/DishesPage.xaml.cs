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
    /// Логика взаимодействия для DishesPage.xaml
    /// </summary>
    public partial class DishesPage : Page
    {
        private List<Dish> _allDishes = new List<Dish>();

        public DishesPage()
        {
            InitializeComponent();
            LoadRestaurants();
            LoadDishes();
        }

        private void LoadRestaurants()
        {
            var restaurants = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "Итальянский дворик" },
                new Restaurant { Id = 2, Name = "Суши-бар Токио" },
                new Restaurant { Id = 3, Name = "Гриль-хаус" },
                new Restaurant { Id = 4, Name = "Все рестораны" }
            };

            CmbRestaurants.ItemsSource = restaurants;
            CmbRestaurants.SelectedIndex = 3; // Select "Все рестораны"
        }

        private void LoadDishes()
        {
            _allDishes = new List<Dish>
            {
                new Dish {
                    Id = 1,
                    Name = "Паста Карбонара",
                    Description = "Спагетти с беконом, яйцом и пармезаном",
                    Price = 450,
                    Category = "Основные блюда",
                    RestaurantId = 1,
                    RestaurantName = "Итальянский дворик"
                },
                new Dish {
                    Id = 2,
                    Name = "Пицца Маргарита",
                    Description = "Классическая итальянская пицца",
                    Price = 380,
                    Category = "Пицца",
                    RestaurantId = 1,
                    RestaurantName = "Итальянский дворик"
                },
                new Dish {
                    Id = 3,
                    Name = "Ролл Филадельфия",
                    Description = "Лосось, сливочный сыр, огурец",
                    Price = 520,
                    Category = "Суши",
                    RestaurantId = 2,
                    RestaurantName = "Суши-бар Токио"
                },
                new Dish {
                    Id = 4,
                    Name = "Стейк Рибай",
                    Description = "Мраморная говядина прожарки medium",
                    Price = 1200,
                    Category = "Гриль",
                    RestaurantId = 3,
                    RestaurantName = "Гриль-хаус"
                },
                new Dish {
                    Id = 5,
                    Name = "Цезарь с креветками",
                    Description = "Салат с листьями айсберг, креветками и соусом цезарь",
                    Price = 320,
                    Category = "Салаты",
                    RestaurantId = 1,
                    RestaurantName = "Итальянский дворик"
                }
            };

            FilterDishes();
        }

        private void FilterDishes()
        {
            if (CmbRestaurants.SelectedItem is Restaurant selectedRestaurant)
            {
                if (selectedRestaurant.Name == "Все рестораны")
                {
                    DishesGrid.ItemsSource = _allDishes;
                }
                else
                {
                    DishesGrid.ItemsSource = _allDishes.Where(d => d.RestaurantId == selectedRestaurant.Id).ToList();
                }
            }
        }

        private void CmbRestaurants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterDishes();
        }

        private void BtnAddDish_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("Функция добавления блюда", "Информация");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadDishes();
            ShowMessage("Список блюд обновлен", "Успех");
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var dish = (sender as Button)?.DataContext as Dish;
            if (dish != null)
            {
                ShowMessage($"Редактирование: {dish.Name}", "Информация");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dish = (sender as Button)?.DataContext as Dish;
            if (dish != null)
            {
                var result = ShowConfirmation($"Удалить блюдо '{dish.Name}'?");
                if (result == MessageBoxResult.Yes)
                {
                    _allDishes.RemoveAll(d => d.Id == dish.Id);
                    FilterDishes();
                    ShowMessage("Блюдо удалено", "Успех");
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