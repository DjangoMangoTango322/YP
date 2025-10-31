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
    /// Логика взаимодействия для BookingsPage.xaml
    /// </summary>
    public partial class BookingsPage : Page
    {
        private List<Booking> _allBookings = new List<Booking>();

        public BookingsPage()
        {
            InitializeComponent();
            LoadRestaurants();
            LoadBookings();
            UpdateStatistics();
        }

        private void LoadRestaurants()
        {
            var restaurants = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "Итальянский дворик" },
                new Restaurant { Id = 2, Name = "Суши-бар Токио" },
                new Restaurant { Id = 3, Name = "Гриль-хаус" }
            };

            CmbRestaurantFilter.ItemsSource = restaurants;
        }

        private void LoadBookings()
        {
            _allBookings = new List<Booking>
            {
                new Booking {
                    Id = 1,
                    UserId = 2,
                    UserName = "Мария Сидорова",
                    RestaurantId = 1,
                    RestaurantName = "Итальянский дворик",
                    BookingDate = System.DateTime.Today.AddDays(1),
                    BookingTime = "19:00",
                    NumberOfGuests = 4,
                    Status = "confirmed",
                    CreatedAt = System.DateTime.Now.AddHours(-2)
                },
                new Booking {
                    Id = 2,
                    UserId = 3,
                    UserName = "Алексей Козлов",
                    RestaurantId = 2,
                    RestaurantName = "Суши-бар Токио",
                    BookingDate = System.DateTime.Today.AddDays(2),
                    BookingTime = "20:30",
                    NumberOfGuests = 2,
                    Status = "pending",
                    CreatedAt = System.DateTime.Now.AddHours(-1)
                },
                new Booking {
                    Id = 3,
                    UserId = 4,
                    UserName = "Елена Иванова",
                    RestaurantId = 3,
                    RestaurantName = "Гриль-хаус",
                    BookingDate = System.DateTime.Today,
                    BookingTime = "18:00",
                    NumberOfGuests = 6,
                    Status = "cancelled",
                    CreatedAt = System.DateTime.Now.AddDays(-1)
                },
                new Booking {
                    Id = 4,
                    UserId = 5,
                    UserName = "Дмитрий Смирнов",
                    RestaurantId = 1,
                    RestaurantName = "Итальянский дворик",
                    BookingDate = System.DateTime.Today.AddDays(3),
                    BookingTime = "21:00",
                    NumberOfGuests = 3,
                    Status = "pending",
                    CreatedAt = System.DateTime.Now.AddMinutes(-30)
                }
            };

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filteredBookings = _allBookings.AsEnumerable();

            // Фильтр по дате
            if (DateFrom.SelectedDate.HasValue)
            {
                filteredBookings = filteredBookings.Where(b => b.BookingDate >= DateFrom.SelectedDate.Value);
            }

            if (DateTo.SelectedDate.HasValue)
            {
                filteredBookings = filteredBookings.Where(b => b.BookingDate <= DateTo.SelectedDate.Value);
            }

            // Фильтр по ресторану
            if (CmbRestaurantFilter.SelectedItem is Restaurant selectedRestaurant)
            {
                filteredBookings = filteredBookings.Where(b => b.RestaurantName == selectedRestaurant.Name);
            }

            // Фильтр по статусу
            if (CmbStatusFilter.SelectedIndex > 0)
            {
                var selectedStatus = (CmbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                filteredBookings = filteredBookings.Where(b => b.Status == selectedStatus);
            }

            BookingsGrid.ItemsSource = filteredBookings.ToList();
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            var filtered = BookingsGrid.ItemsSource as IEnumerable<Booking> ?? _allBookings;
            TxtTotalBookings.Text = filtered.Count().ToString();
            TxtConfirmedCount.Text = filtered.Count(b => b.Status == "confirmed").ToString();
            TxtPendingCount.Text = filtered.Count(b => b.Status == "pending").ToString();
            TxtCancelledCount.Text = filtered.Count(b => b.Status == "cancelled").ToString();
        }

        private void DateFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void RestaurantFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            DateFrom.SelectedDate = null;
            DateTo.SelectedDate = null;
            CmbRestaurantFilter.SelectedIndex = -1;
            CmbStatusFilter.SelectedIndex = 0;
        }

        private void BtnCreateBooking_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("Функция создания бронирования", "Информация");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBookings();
            ShowMessage("Список бронирований обновлен", "Успех");
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as Button)?.DataContext as Booking;
            if (booking != null)
            {
                booking.Status = "confirmed";
                ApplyFilters();
                ShowMessage("Бронирование подтверждено", "Успех");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as Button)?.DataContext as Booking;
            if (booking != null)
            {
                var result = ShowConfirmation($"Отменить бронирование #{booking.Id}?");
                if (result == MessageBoxResult.Yes)
                {
                    booking.Status = "cancelled";
                    ApplyFilters();
                    ShowMessage("Бронирование отменено", "Успех");
                }
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as Button)?.DataContext as Booking;
            if (booking != null)
            {
                ShowMessage($"Детали бронирования #{booking.Id}:\n\n" +
                          $"Клиент: {booking.UserName}\n" +
                          $"ID клиента: {booking.UserId}\n" +
                          $"Ресторан: {booking.RestaurantName}\n" +
                          $"ID ресторана: {booking.RestaurantId}\n" +
                          $"Дата: {booking.BookingDate:dd.MM.yyyy}\n" +
                          $"Время: {booking.BookingTime}\n" +
                          $"Гостей: {booking.NumberOfGuests}\n" +
                          $"Статус: {booking.Status}\n" +
                          $"Создано: {booking.CreatedAt:dd.MM.yyyy HH:mm}",
                          "Информация о бронировании");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as Button)?.DataContext as Booking;
            if (booking != null)
            {
                ShowMessage($"Редактирование бронирования #{booking.Id}", "Информация");
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