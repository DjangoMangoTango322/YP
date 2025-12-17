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
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditBooking.xaml
    /// </summary>
    public partial class AddEditBooking : Page
    {
        private readonly Booking _booking = new Booking
        {
            Status = "Ожидание",
            Created_At = DateTime.Now
        };

        private List<User> _users;
        private List<Restaurant> _restaurants;
        public AddEditBooking()
        {
            InitializeComponent();
            Loaded += AddEditBooking_Loaded;
        }
        private async void AddEditBooking_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                // Загружаем списки из API
                _users = await App.ApiContext.GetAllUsersAsync() ?? new List<User>();
                _restaurants = await App.ApiContext.GetAllRestaurantsAsync() ?? new List<Restaurant>();

                UserComboBox.ItemsSource = _users;
                RestaurantComboBox.ItemsSource = _restaurants;

                // Выбираем первый элемент по умолчанию (если есть)
                if (_users.Count > 0) UserComboBox.SelectedIndex = 0;
                if (_restaurants.Count > 0) RestaurantComboBox.SelectedIndex = 0;

                // Дата по умолчанию — завтра
                BookingDatePicker.SelectedDate = DateTime.Today.AddDays(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                _booking.User_Id = (int)UserComboBox.SelectedValue;
                _booking.Restaurant_Id = (int)RestaurantComboBox.SelectedValue;
                _booking.Number_Of_Guests = int.Parse(GuestsTxt.Text.Trim());
                _booking.Booking_Date = BookingDatePicker.SelectedDate.Value;

                await App.ApiContext.CreateBookingAsync(_booking);

                MessageBox.Show("Бронирование успешно создано!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private bool ValidateInput()
        {
            if (UserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя из списка.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (RestaurantComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите ресторан из списка.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (BookingDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату бронирования.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(GuestsTxt.Text.Trim(), out int guests) || guests < 1 || guests > 100)
            {
                MessageBox.Show("Введите количество гостей от 1 до 100.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}