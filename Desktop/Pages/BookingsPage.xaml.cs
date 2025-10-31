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
    /// Логика взаимодействия для BookingsPage.xaml
    /// </summary>
    public partial class BookingsPage : Page
    {
        private string connectionString = "Server=localhost;Database=RestaurantDB;Integrated Security=true;";


        public BookingsPage()
        {
            InitializeComponent();
            LoadBookingsFromDatabase();
        }

        private void LoadBookingsFromDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        SELECT 
                            b.id,
                            b.user_id,
                            b.restaurant_id,
                            b.booking_date,
                            b.booking_time,
                            b.number_of_guests,
                            b.status,
                            b.created_at,
                            u.first_name + ' ' + u.last_name as client_name,
                            r.name as restaurant_name
                        FROM bookings b
                        LEFT JOIN users u ON b.user_id = u.id
                        LEFT JOIN restaurants r ON b.restaurant_id = r.id
                        ORDER BY b.created_at DESC";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        var bookings = new List<Booking>();
                        while (reader.Read())
                        {
                            var booking = new Booking
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                RestaurantId = reader.GetInt32(reader.GetOrdinal("restaurant_id")),
                                BookingDate = reader.GetDateTime(reader.GetOrdinal("booking_date")),
                                BookingTime = (TimeSpan)reader["booking_time"],
                                NumberOfGuests = reader.GetInt32(reader.GetOrdinal("number_of_guests")),
                                Status = reader.GetString(reader.GetOrdinal("status")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("client_name")))
                            {
                                SetClientName(booking, reader.GetString(reader.GetOrdinal("client_name")));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("restaurant_name")))
                            {
                                SetRestaurantName(booking, reader.GetString(reader.GetOrdinal("restaurant_name")));
                            }

                            bookings.Add(booking);
                        }

                        BookingsGrid.ItemsSource = bookings;
                        UpdateOutput($"Загружено {bookings.Count} бронирований из базы данных");
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateOutput($"❌ Ошибка загрузки данных: {ex.Message}");
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetClientName(Booking booking, string clientName)
        {
            // Используем отражение или добавляем свойство в класс Booking
            var clientNameField = typeof(Booking).GetField("_clientName",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (clientNameField != null)
            {
                clientNameField.SetValue(booking, clientName);
            }
        }

        private void SetRestaurantName(Booking booking, string restaurantName)
        {
            var restaurantNameField = typeof(Booking).GetField("_restaurantName",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (restaurantNameField != null)
            {
                restaurantNameField.SetValue(booking, restaurantName);
            }
        }
        private string GetClientName(Booking booking)
        {
            var clientNameField = typeof(Booking).GetField("_clientName",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (clientNameField != null)
            {
                return clientNameField.GetValue(booking) as string ?? $"Клиент {booking.UserId}";
            }
            return $"Клиент {booking.UserId}";
        }

        private string GetRestaurantName(Booking booking)
        {
            var restaurantNameField = typeof(Booking).GetField("_restaurantName",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (restaurantNameField != null)
            {
                return restaurantNameField.GetValue(booking) as string ?? $"Ресторан {booking.RestaurantId}";
            }
            return $"Ресторан {booking.RestaurantId}";
        }

        private void UpdateOutput(string message)
        {
            TxtOutput.Text = message;
        }

        private Booking GetSelectedBooking()
        {
            return BookingsGrid.SelectedItem as Booking;
        }

        private void BtnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        INSERT INTO bookings (user_id, restaurant_id, booking_date, booking_time, number_of_guests, status, created_at)
                        VALUES (@user_id, @restaurant_id, @booking_date, @booking_time, @number_of_guests, @status, @created_at);
                        SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", 1);
                        command.Parameters.AddWithValue("@restaurant_id", 1);
                        command.Parameters.AddWithValue("@booking_date", DateTime.Now.AddDays(7));
                        command.Parameters.AddWithValue("@booking_time", new TimeSpan(18, 0, 0));
                        command.Parameters.AddWithValue("@number_of_guests", 2);
                        command.Parameters.AddWithValue("@status", "Ожидание");
                        command.Parameters.AddWithValue("@created_at", DateTime.Now);

                        var newId = Convert.ToInt32(command.ExecuteScalar());

                        UpdateOutput($"Добавлено новое бронирование в базу данных:\nID: #{newId}\nСтатус: Ожидание\nГостей: 2");
                        LoadBookingsFromDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateOutput($"❌ Ошибка добавления бронирования: {ex.Message}");
                MessageBox.Show($"Ошибка при добавлении бронирования: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var booking = GetSelectedBooking();
            if (booking != null)
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var query = @"
                            UPDATE bookings 
                            SET number_of_guests = @number_of_guests, 
                                status = @status,
                                booking_date = @booking_date
                            WHERE id = @id";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", booking.Id);
                            command.Parameters.AddWithValue("@number_of_guests", booking.NumberOfGuests + 1);
                            command.Parameters.AddWithValue("@status", "Изменено");
                            command.Parameters.AddWithValue("@booking_date", booking.BookingDate.AddDays(1));

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                UpdateOutput($"Бронирование обновлено в базе данных:\nID: #{booking.Id}");
                                LoadBookingsFromDatabase();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateOutput($"Ошибка обновления бронирования: {ex.Message}");
                    MessageBox.Show($"Ошибка при обновлении бронирования: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите бронирование для изменения", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
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
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "DELETE FROM bookings WHERE id = @id";

                            using (var command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@id", booking.Id);
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    UpdateOutput($"🗑️ Бронирование удалено из базы данных:\nID: #{booking.Id}");
                                    LoadBookingsFromDatabase();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateOutput($"Ошибка удаления бронирования: {ex.Message}");
                        MessageBox.Show($"Ошибка при удалении бронирования: {ex.Message}", "Ошибка",
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
            LoadBookingsFromDatabase();
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var booking = GetSelectedBooking();
            if (booking != null)
            {
                UpdateOutput($"Информация о бронировании (из БД):\n" +
                           $"ID: {booking.Id}\n" +
                           $"Клиент: {GetClientName(booking)}\n" +
                           $"Ресторан: {GetRestaurantName(booking)}\n" +
                           $"Дата: {booking.BookingDate:dd.MM.yyyy}\n" +
                           $"Время: {booking.BookingTime:hh\\:mm}\n" +
                           $"Гостей: {booking.NumberOfGuests}\n" +
                           $"Статус: {booking.Status}\n" +
                           $"Создано: {booking.CreatedAt:dd.MM.yyyy HH:mm}");
            }
        }
    }
}