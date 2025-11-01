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

        private void BookingCard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
            }
        }

        private void BookingCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            }
        }

        private void BookingCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                // Сбрасываем выделение у всех карточек
                foreach (var item in BookingsItemsControl.Items)
                {
                    var container = BookingsItemsControl.ItemContainerGenerator.ContainerFromItem(item);
                    if (container != null)
                    {
                        var contentPresenter = FindVisualChild<ContentPresenter>(container);
                        if (contentPresenter != null)
                        {
                            var templateBorder = FindVisualChild<Border>(contentPresenter);
                            if (templateBorder != null)
                            {
                                templateBorder.BorderBrush = Brushes.Transparent;
                                templateBorder.BorderThickness = new Thickness(0);
                            }
                        }
                    }
                }

                // Выделяем текущую карточку
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
                border.BorderThickness = new Thickness(2);

                // Получаем данные бронирования
                var booking = border.DataContext as Booking;
                if (booking != null)
                {
                    _selectedBooking = booking;
                    BookingsGrid.SelectedItem = _selectedBooking;
                }
            }
        }

        // Вспомогательные методы для поиска дочерних элементов
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;
                else
                {
                    var descendant = FindVisualChild<T>(child);
                    if (descendant != null)
                        return descendant;
                }
            }
            return null;
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
                    booking.Status = "Подтверждено";
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

            // Обновляем выделение в карточках при выборе в DataGrid
            if (_selectedBooking != null)
            {
                foreach (var item in BookingsItemsControl.Items)
                {
                    if (item is Booking booking && booking.Id == _selectedBooking.Id)
                    {
                        var container = BookingsItemsControl.ItemContainerGenerator.ContainerFromItem(item);
                        if (container != null)
                        {
                            var contentPresenter = FindVisualChild<ContentPresenter>(container);
                            if (contentPresenter != null)
                            {
                                var templateBorder = FindVisualChild<Border>(contentPresenter);
                                if (templateBorder != null)
                                {
                                    // Сначала сбрасываем все выделения
                                    foreach (var otherItem in BookingsItemsControl.Items)
                                    {
                                        var otherContainer = BookingsItemsControl.ItemContainerGenerator.ContainerFromItem(otherItem);
                                        if (otherContainer != null)
                                        {
                                            var otherContentPresenter = FindVisualChild<ContentPresenter>(otherContainer);
                                            if (otherContentPresenter != null)
                                            {
                                                var otherBorder = FindVisualChild<Border>(otherContentPresenter);
                                                if (otherBorder != null)
                                                {
                                                    otherBorder.BorderBrush = Brushes.Transparent;
                                                    otherBorder.BorderThickness = new Thickness(0);
                                                }
                                            }
                                        }
                                    }

                                    // Выделяем выбранную карточку
                                    templateBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
                                    templateBorder.BorderThickness = new Thickness(2);
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        // Метод для обновления цвета статуса в реальном времени
        private void UpdateStatusColor(Border statusBorder, string status)
        {
            switch (status)
            {
                case "Подтверждено":
                    statusBorder.Background = new SolidColorBrush(Color.FromRgb(46, 125, 50));
                    break;
                case "Ожидание":
                    statusBorder.Background = new SolidColorBrush(Color.FromRgb(251, 140, 0));
                    break;
                case "Отменено":
                    statusBorder.Background = new SolidColorBrush(Color.FromRgb(198, 40, 40));
                    break;
                default:
                    statusBorder.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                    break;
            }
        }

        // Метод вызывается когда загружается элемент ItemsControl
        private void BookingsItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Обновляем цвета статусов после загрузки данных
            foreach (var item in BookingsItemsControl.Items)
            {
                var container = BookingsItemsControl.ItemContainerGenerator.ContainerFromItem(item);
                if (container != null)
                {
                    var contentPresenter = FindVisualChild<ContentPresenter>(container);
                    if (contentPresenter != null)
                    {
                        var statusBorder = FindVisualChild<Border>(contentPresenter, "StatusBorder");
                        if (statusBorder != null && item is Booking booking)
                        {
                            UpdateStatusColor(statusBorder, booking.Status);
                        }
                    }
                }
            }
        }

        // Перегруженный метод для поиска по имени
        private T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result && (child as FrameworkElement)?.Name == childName)
                    return result;
                else
                {
                    var descendant = FindVisualChild<T>(child, childName);
                    if (descendant != null)
                        return descendant;
                }
            }
            return null;
        }
    }
}