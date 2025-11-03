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
                SetLoadingState(true);
                var users = await App.ApiClient.GetUsersAsync();
                UsersGrid.ItemsSource = users;
                UsersItemsControl.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            UsersGrid.IsEnabled = !isLoading;
            UsersItemsControl.IsEnabled = !isLoading;
        }

        private void UserCard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.DataContext is User user && user != _selectedUser)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
            }
        }

        private void UserCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.DataContext is User user && user != _selectedUser)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            }
        }

        private void UserCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                var user = border.DataContext as User;
                if (user != null)
                {
                    if (_selectedUser != null)
                    {
                        ResetUserCardSelection(_selectedUser);
                    }

                    _selectedUser = user;
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
                    border.BorderThickness = new Thickness(2);

                    UsersGrid.SelectedItem = _selectedUser;
                }
            }
        }

        private void ResetUserCardSelection(User userToReset)
        {
            if (UsersItemsControl.ItemsSource == null) return;

            try
            {
                foreach (var item in UsersItemsControl.Items)
                {
                    if (item is User user && user.Id == userToReset.Id)
                    {
                        var container = UsersItemsControl.ItemContainerGenerator.ContainerFromItem(item);
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
                                    templateBorder.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error resetting selection: {ex.Message}");
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

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

        private User GetSelectedUser()
        {
            return _selectedUser ?? UsersGrid.SelectedItem as User;
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditUserPage());
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var user = GetSelectedUser();
            if (user != null)
            {
                NavigationService.Navigate(new AddEditUserPage(user));
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

            if (_selectedUser != null)
            {
                foreach (var item in UsersItemsControl.Items)
                {
                    if (item is User user && user.Id == _selectedUser.Id)
                    {
                        var container = UsersItemsControl.ItemContainerGenerator.ContainerFromItem(item);
                        if (container != null)
                        {
                            var contentPresenter = FindVisualChild<ContentPresenter>(container);
                            if (contentPresenter != null)
                            {
                                var templateBorder = FindVisualChild<Border>(contentPresenter);
                                if (templateBorder != null)
                                {
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
    }
}