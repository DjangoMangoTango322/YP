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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private User _selectedUser;
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
                UsersItemsControl.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UserCard card)
            {
                _selectedUser = card.User;
                UsersGrid.SelectedItem = _selectedUser;
            }
        }

        private User GetSelectedUser()
        {
            return _selectedUser ?? UsersGrid.SelectedItem as User;
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
                    Phone = "+7 (999) 999-99-99"
                };

                var createdUser = await App.ApiClient.CreateUserAsync(newUser);
                MessageBox.Show($"✅ Добавлен новый пользователь через API!\nID: #{createdUser.Id}\nИмя: {createdUser.FirstName} {createdUser.LastName}",
                              "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUsersFromApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка добавления пользователя: {ex.Message}", "Ошибка",
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
                    user.LastName += " (изм.)";

                    var updatedUser = await App.ApiClient.UpdateUserAsync(user.Id, user);
                    MessageBox.Show($"✏️ Пользователь обновлен через API!\nID: #{updatedUser.Id}\nНовое имя: {updatedUser.FirstName} {updatedUser.LastName}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsersFromApi();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Ошибка обновления пользователя: {ex.Message}", "Ошибка",
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
                            MessageBox.Show($"🗑️ Пользователь удален через API!\nID: #{user.Id}\nИмя: {user.FirstName} {user.LastName}",
                                          "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadUsersFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Ошибка удаления пользователя: {ex.Message}", "Ошибка",
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
            _selectedUser = UsersGrid.SelectedItem as User;
        }
    }
}