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
    /// Логика взаимодействия для DishesPage.xaml
    /// </summary>
    public partial class DishesPage : Page
    {
        private string connectionString = "Server=localhost;Database=RestaurantDB;Integrated Security=true;";
        public DishesPage()
        {
            InitializeComponent();
            LoadDishesFromDatabase();
        }

        private void LoadDishesFromDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        SELECT 
                            d.id,
                            d.name,
                            d.description,
                            d.price,
                            d.category,
                            r.name as restaurant_name
                        FROM dishes d
                        LEFT JOIN restaurant_dishes rd ON d.id = rd.dish_id
                        LEFT JOIN restaurants r ON rd.restaurant_id = r.id
                        ORDER BY d.name";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        var dishes = new List<Dish>();
                        while (reader.Read())
                        {
                            var dish = new Dish
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("price")),
                                Category = reader.GetString(reader.GetOrdinal("category"))
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("restaurant_name")))
                            {
                                SetRestaurantName(dish, reader.GetString(reader.GetOrdinal("restaurant_name")));
                            }

                            dishes.Add(dish);
                        }

                        DishesGrid.ItemsSource = dishes;
                        UpdateOutput($"Загружено {dishes.Count} блюд из базы данных");
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateOutput($"Ошибка загрузки данных: {ex.Message}");
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetRestaurantName(Dish dish, string restaurantName)
        {
            var restaurantNameField = typeof(Dish).GetField("_restaurantName",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (restaurantNameField != null)
            {
                restaurantNameField.SetValue(dish, restaurantName);
            }
        }

        private string GetRestaurantName(Dish dish)
        {
            var restaurantNameField = typeof(Dish).GetField("_restaurantName",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (restaurantNameField != null)
            {
                return restaurantNameField.GetValue(dish) as string ?? "Неизвестный ресторан";
            }
            return "Неизвестный ресторан";
        }

        private void UpdateOutput(string message)
        {
            TxtOutput.Text = message;
        }

        private Dish GetSelectedDish()
        {
            return DishesGrid.SelectedItem as Dish;
        }

        private void BtnAddDish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"
                        INSERT INTO dishes (name, description, price, category)
                        VALUES (@name, @description, @price, @category);
                        SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", "Новое блюдо");
                        command.Parameters.AddWithValue("@description", "Описание нового блюда");
                        command.Parameters.AddWithValue("@price", 500.00m);
                        command.Parameters.AddWithValue("@category", "Основное");

                        var newId = Convert.ToInt32(command.ExecuteScalar());
                        var linkQuery = "INSERT INTO restaurant_dishes (restaurant_id, dish_id) VALUES (1, @dish_id)";
                        using (var linkCommand = new SqlCommand(linkQuery, connection))
                        {
                            linkCommand.Parameters.AddWithValue("@dish_id", newId);
                            linkCommand.ExecuteNonQuery();
                        }

                        UpdateOutput($"Добавлено новое блюдо в базу данных:\nID: #{newId}\nНазвание: Новое блюдо\nЦена: 500₽");
                        LoadDishesFromDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateOutput($"Ошибка добавления блюда: {ex.Message}");
                MessageBox.Show($"Ошибка при добавлении блюда: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var dish = GetSelectedDish();
            if (dish != null)
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var query = @"
                            UPDATE dishes 
                            SET name = @name, 
                                price = @price,
                                category = @category
                            WHERE id = @id";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", dish.Id);
                            command.Parameters.AddWithValue("@name", dish.Name + " (изм.)");
                            command.Parameters.AddWithValue("@price", dish.Price + 100);
                            command.Parameters.AddWithValue("@category", dish.Category);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                UpdateOutput($"Блюдо обновлено в базе данных:\nID: #{dish.Id}\nНовое название: {dish.Name} (изм.)\nНовая цена: {dish.Price + 100}₽");
                                LoadDishesFromDatabase();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateOutput($"Ошибка обновления блюда: {ex.Message}");
                    MessageBox.Show($"Ошибка при обновлении блюда: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите блюдо для изменения", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var dish = GetSelectedDish();
            if (dish != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить блюдо \"{dish.Name}\"?",
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
                            var deleteLinksQuery = "DELETE FROM restaurant_dishes WHERE dish_id = @id";
                            using (var linkCommand = new SqlCommand(deleteLinksQuery, connection))
                            {
                                linkCommand.Parameters.AddWithValue("@id", dish.Id);
                                linkCommand.ExecuteNonQuery();
                            }
                            var deleteDishQuery = "DELETE FROM dishes WHERE id = @id";
                            using (var dishCommand = new SqlCommand(deleteDishQuery, connection))
                            {
                                dishCommand.Parameters.AddWithValue("@id", dish.Id);
                                int rowsAffected = dishCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    UpdateOutput($"Блюдо удалено из базы данных:\nID: #{dish.Id}\nНазвание: {dish.Name}");
                                    LoadDishesFromDatabase();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateOutput($"Ошибка удаления блюда: {ex.Message}");
                        MessageBox.Show($"Ошибка при удалении блюда: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите блюдо для удаления", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadDishesFromDatabase();
        }

        private void DishesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dish = GetSelectedDish();
            if (dish != null)
            {
                UpdateOutput($"Информация о блюде (из БД):\n" +
                           $"ID: {dish.Id}\n" +
                           $"Название: {dish.Name}\n" +
                           $"Описание: {dish.Description}\n" +
                           $"Цена: {dish.Price}₽\n" +
                           $"Категория: {dish.Category}\n" +
                           $"Ресторан: {GetRestaurantName(dish)}");
            }
        }
    }
}