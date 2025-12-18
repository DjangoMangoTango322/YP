using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    public partial class AddEditBooking : Page
    {
        private readonly Booking _booking = new Booking
        {
            Status = "Ожидание",
            Created_At = DateTime.Now
        };

        private List<User> _users;
        private List<Restaurant> _restaurants;
        private bool _isEditMode;

        public AddEditBooking(Booking booking = null)
        {
            InitializeComponent();
            _booking = booking ?? new Booking
            {
                Status = "Ожидание",
                Created_At = DateTime.Now
            };
            _isEditMode = booking != null;
            Loaded += AddEditBooking_Loaded;
        }

        private async void AddEditBooking_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                _users = await App.ApiContext.GetAllUsersAsync() ?? new List<User>();
                _restaurants = await App.ApiContext.GetAllRestaurantsAsync() ?? new List<Restaurant>();

                UserComboBox.ItemsSource = _users;
                UserComboBox.SelectedValuePath = "Id";

                RestaurantComboBox.ItemsSource = _restaurants;
                RestaurantComboBox.SelectedValuePath = "Id";

                if (_isEditMode)
                {
                    UserComboBox.SelectedValue = _booking.User_Id;
                    RestaurantComboBox.SelectedValue = _booking.Restaurant_Id;
                    BookingDatePicker.SelectedDate = _booking.Booking_Date;
                    GuestsTxt.Text = _booking.Number_Of_Guests.ToString();
                }
                else
                {
                    if (_users.Count > 0) UserComboBox.SelectedIndex = 0;
                    if (_restaurants.Count > 0) RestaurantComboBox.SelectedIndex = 0;
                    BookingDatePicker.SelectedDate = DateTime.Today.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        // Метод Save_Click — добавь его!
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

                if (_isEditMode)
                {
                    await App.ApiContext.UpdateBookingAsync(_booking);
                    MessageBox.Show("Бронирование обновлено!", "Успех");
                }
                else
                {
                    await App.ApiContext.CreateBookingAsync(_booking);
                    MessageBox.Show("Бронирование создано!", "Успех");
                }

                NavigationService.Navigate(new BookingsPage());  // ← Новая страница с данными из API
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка");
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
            // Твоя валидация из исходного кода
            if (UserComboBox.SelectedItem == null) { MessageBox.Show("Выберите пользователя"); return false; }
            if (RestaurantComboBox.SelectedItem == null) { MessageBox.Show("Выберите ресторан"); return false; }
            if (BookingDatePicker.SelectedDate == null) { MessageBox.Show("Выберите дату"); return false; }
            if (!int.TryParse(GuestsTxt.Text.Trim(), out int guests) || guests < 1) { MessageBox.Show("Гости от 1"); return false; }
            return true;
        }
    }
}