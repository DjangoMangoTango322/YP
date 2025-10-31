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
    /// Логика взаимодействия для BookingsPage.xaml
    /// </summary>
    public partial class BookingsPage : Page
    {
        private Booking _selectedBooking;
        public BookingsPage()
        {
            InitializeComponent();
            LoadBookingsFromApi();
        }

        private async void LoadBookingsFromApi()
        {
            try
            {
                var bookings = await App.ApiClient.GetBookingsAsync();
                BookingsGrid.ItemsSource = bookings;
                BookingsItemsControl.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BookingCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is BookingCard card)
            {
                _selectedBooking = card.Booking;
                BookingsGrid.SelectedItem = _selectedBooking;
            }
        }

        private Booking GetSelectedBooking()
        {
            return _selectedBooking ?? BookingsGrid.SelectedItem as Booking;
        }

        private async void BtnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newBooking = new Booking
                {
                    UserId = 1,
                    RestaurantId = 1,
                    BookingDate = DateTime.Now.AddDays(7),
                    BookingTime = new TimeSpan(18, 0, 0),
                    NumberOfGuests = 2,
                    Status = "Ожидание"
                };

                var createdBooking = await App.ApiClient.CreateBookingAsync(newBooking);
                MessageBox.Show($"✅ Добавлено новое бронирование через API!\nID: #{createdBooking.Id}\nСтатус: {createdBooking.Status}\nГостей: {createdBooking.NumberOfGuests}",
                              "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBookingsFromApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка добавления бронирования: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var booking = GetSelectedBooking();
            if (booking != null)
            {
                try
                {
                    booking.NumberOfGuests += 1;
                    booking.Status = "Изменено";
                    booking.BookingDate = booking.BookingDate.AddDays(1);

                    var updatedBooking = await App.ApiClient.UpdateBookingAsync(booking.Id, booking);
                    MessageBox.Show($"✏️ Бронирование обновлено через API!\nID: #{updatedBooking.Id}\nСтатус: {updatedBooking.Status}\nГостей: {updatedBooking.NumberOfGuests}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadBookingsFromApi();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Ошибка обновления бронирования: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите бронирование для изменения", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var booking = GetSelectedBooking();
            if (booking != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить бронирование #{booking.Id}?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var success = await App.ApiClient.DeleteBookingAsync(booking.Id);
                        if (success)
                        {
                            MessageBox.Show($"🗑️ Бронирование удалено через API!\nID: #{booking.Id}",
                                          "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadBookingsFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Ошибка удаления бронирования: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите бронирование для удаления", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBookingsFromApi();
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedBooking = BookingsGrid.SelectedItem as Booking;
        }
    }
}