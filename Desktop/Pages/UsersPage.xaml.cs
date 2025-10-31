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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private List<User> _allUsers = new List<User>();
        private bool _isSearchPlaceholder = true;
        public UsersPage()
        {
            InitializeComponent();
            InitializeSearchBox();
            LoadUsers();
            UpdateStatistics();
        }

        private void InitializeSearchBox()
        {
            TxtSearch.Text = "Поиск по имени или email...";
            TxtSearch.Foreground = new SolidColorBrush(Colors.Gray);

            TxtSearch.GotFocus += (s, e) =>
            {
                if (_isSearchPlaceholder)
                {
                    TxtSearch.Text = "";
                    TxtSearch.Foreground = new SolidColorBrush(Colors.White);
                    _isSearchPlaceholder = false;
                }
            };

            TxtSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TxtSearch.Text))
                {
                    TxtSearch.Text = "Поиск по имени или email...";
                    TxtSearch.Foreground = new SolidColorBrush(Colors.Gray);
                    _isSearchPlaceholder = true;
                }
            };
        }

        private void LoadUsers()
        {
            _allUsers = new List<User>
            {
                new User {
                    Id = 1,
                    FirstName = "Иван",
                    LastName = "Петров",
                    Email = "admin@restaurant.com",
                    Phone = "+7 (495) 111-11-11",
                    Role = "Администратор",
                    IsActive = true,
                    CreatedAt = System.DateTime.Now.AddDays(-30)
                },
                new User {
                    Id = 2,
                    FirstName = "Мария",
                    LastName = "Сидорова",
                    Email = "maria@example.com",
                    Phone = "+7 (495) 222-22-22",
                    Role = "Пользователь",
                    IsActive = true,
                    CreatedAt = System.DateTime.Now.AddDays(-15)
                },
                new User {
                    Id = 3,
                    FirstName = "Алексей",
                    LastName = "Козлов",
                    Email = "alex@example.com",
                    Phone = "+7 (495) 333-33-33",
                    Role = "Пользователь",
                    IsActive = false,
                    CreatedAt = System.DateTime.Now.AddDays(-7)
                },
                new User {
                    Id = 4,
                    FirstName = "Елена",
                    LastName = "Иванова",
                    Email = "elena@example.com",
                    Phone = "+7 (495) 444-44-44",
                    Role = "Пользователь",
                    IsActive = true,
                    CreatedAt = System.DateTime.Now.AddDays(-3)
                },
                new User {
                    Id = 5,
                    FirstName = "Дмитрий",
                    LastName = "Смирнов",
                    Email = "dmitry@example.com",
                    Phone = "+7 (495) 555-55-55",
                    Role = "Администратор",
                    IsActive = true,
                    CreatedAt = System.DateTime.Now.AddDays(-10)
                }
            };

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filteredUsers = _allUsers.AsEnumerable();

            // Поиск (игнорируем placeholder текст)
            if (!_isSearchPlaceholder && !string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                var searchText = TxtSearch.Text.ToLower();
                filteredUsers = filteredUsers.Where(u =>
                    u.FullName.ToLower().Contains(searchText) ||
                    u.Email.ToLower().Contains(searchText));
            }

            // Фильтр по роли
            if (CmbRoleFilter.SelectedIndex > 0)
            {
                var selectedRole = (CmbRoleFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                filteredUsers = filteredUsers.Where(u => u.Role == selectedRole);
            }

            UsersGrid.ItemsSource = filteredUsers.ToList();
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            var filtered = UsersGrid.ItemsSource as IEnumerable<User> ?? _allUsers;
            TxtTotalUsers.Text = filtered.Count().ToString();
            TxtAdminCount.Text = filtered.Count(u => u.Role == "Администратор").ToString();
            TxtActiveCount.Text = filtered.Count(u => u.IsActive).ToString();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isSearchPlaceholder)
            {
                ApplyFilters();
            }
        }

        private void CmbRoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            TxtSearch.Text = "Поиск по имени или email...";
            TxtSearch.Foreground = new SolidColorBrush(Colors.Gray);
            _isSearchPlaceholder = true;
            CmbRoleFilter.SelectedIndex = 0;
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                Id = _allUsers.Count + 1,
                FirstName = "Новый",
                LastName = "Пользователь",
                Email = "new@example.com",
                Phone = "+7 (495) 000-00-00",
                Role = "Пользователь",
                IsActive = true,
                CreatedAt = System.DateTime.Now
            };
            _allUsers.Add(newUser);
            ApplyFilters();

            ShowMessage("Пользователь добавлен", "Успех");
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("Экспорт данных в CSV выполнен", "Экспорт");
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button)?.DataContext as User;
            if (user != null)
            {
                ShowMessage($"Детальная информация о пользователе:\n\n" +
                          $"ФИО: {user.FullName}\n" +
                          $"Email: {user.Email}\n" +
                          $"Телефон: {user.Phone}\n" +
                          $"Роль: {user.Role}\n" +
                          $"Статус: {(user.IsActive ? "Активен" : "Заблокирован")}\n" +
                          $"Дата регистрации: {user.CreatedAt:dd.MM.yyyy}",
                          "Информация о пользователе");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button)?.DataContext as User;
            if (user != null)
            {
                user.FirstName = "Измененное";
                user.LastName = "Имя";
                ApplyFilters();

                ShowMessage($"Пользователь {user.FullName} изменен", "Успех");
            }
        }

        private void BtnToggleActive_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button)?.DataContext as User;
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                ApplyFilters();

                ShowMessage($"Пользователь {user.FullName} {(user.IsActive ? "активирован" : "заблокирован")}",
                          "Статус изменен");
            }
        }

        private void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}