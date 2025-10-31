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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsersFromApi();
        }

        private async void LoadUsersFromApi()
        {
            try
            {
                var users = await App.ApiClient.GetUsersAsync();
                UsersGrid.ItemsSource = users;
                UpdateOutput($"✅ Загружено {users.Count} пользователей из API");
            }
            catch (Exception ex)
            {
                UpdateOutput($"❌ Ошибка загрузки данных: {ex.Message}");
                MessageBox.Show($"Ошибка подключения к API: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateOutput(string message)
        {
            // Метод для обновления вывода, если нужно
        }

        private User GetSelectedUser()
        {
            return UsersGrid.SelectedItem as User;
        }

        private async void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newUser = new User
                {
                    FirstName = "Новый",
                    LastName = "Пользователь",
                    Email = "newuser@example.com",
                    Phone = "+7 (000) 000-00-00",
                    CreatedAt = DateTime.Now
                };

                var createdUser = await App.ApiClient.CreateUserAsync(newUser);
                UpdateOutput($"✅ Добавлен новый пользователь через API:\nID: #{createdUser.Id}\nИмя: {createdUser.FirstName} {createdUser.LastName}");
                LoadUsersFromApi();
            }
            catch (Exception ex)
            {
                UpdateOutput($"❌ Ошибка добавления пользователя: {ex.Message}");
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var user = GetSelectedUser();
            if (user != null)
            {
                try
                {
                    user.FirstName += " (изм.)";
                    user.Email = "changed_" + user.Email;

                    var updatedUser = await App.ApiClient.UpdateUserAsync(user.Id, user);
                    UpdateOutput($"✏️ Пользователь обновлен через API:\nID: #{updatedUser.Id}\nНовое имя: {updatedUser.FirstName}");
                    LoadUsersFromApi();
                }
                catch (Exception ex)
                {
                    UpdateOutput($"❌ Ошибка обновления пользователя: {ex.Message}");
                    MessageBox.Show($"Ошибка при обновлении пользователя: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для изменения", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var user = GetSelectedUser();
            if (user != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя \"{user.FirstName} {user.LastName}\"?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var success = await App.ApiClient.DeleteUserAsync(user.Id);
                        if (success)
                        {
                            UpdateOutput($"🗑️ Пользователь удален через API:\nID: #{user.Id}\nИмя: {user.FirstName} {user.LastName}");
                            LoadUsersFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateOutput($"❌ Ошибка удаления пользователя: {ex.Message}");
                        MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsersFromApi();
        }

        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = GetSelectedUser();
            if (user != null)
            {
                // Можно добавить вывод деталей в ItemOutputGrid
            }
        }

        // Убраны неиспользуемые методы
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) { }
        private void CmbRoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void BtnResetFilters_Click(object sender, RoutedEventArgs e) { }
        private void BtnExport_Click(object sender, RoutedEventArgs e) { }
        private void BtnView_Click(object sender, RoutedEventArgs e) { }
        private void BtnEdit_Click(object sender, RoutedEventArgs e) { }
        private void BtnToggleActive_Click(object sender, RoutedEventArgs e) { }
    }
}