using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<User> _allUsers;
        private ObservableCollection<User> _filteredUsers;
        private string _searchQuery;
        private User _selectedUser;

        public ObservableCollection<User> AllUsers
        {
            get => _allUsers;
            set
            {
                _allUsers = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public ObservableCollection<User> FilteredUsers
        {
            get => _filteredUsers;
            set
            {
                _filteredUsers = value;
                OnPropertyChanged();
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public UsersPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                var users = await App.ApiContext.GetAllUsersAsync();
                AllUsers = new ObservableCollection<User>(users ?? new List<User>());
                foreach (var u in AllUsers) u.IsSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                AllUsers = new ObservableCollection<User>();
            }
        }

        private void ApplyFilter()
        {
            if (AllUsers == null)
            {
                FilteredUsers = new ObservableCollection<User>();
                return;
            }

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredUsers = new ObservableCollection<User>(AllUsers);
            }
            else
            {
                var query = SearchQuery.Trim();
                var filtered = AllUsers.Where(u =>
                    (u.First_Name?.IndexOf(query, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                    (u.Last_Name?.IndexOf(query, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                    (u.Login?.IndexOf(query, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                    u.Id.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
                FilteredUsers = new ObservableCollection<User>(filtered);
            }
            // No need to set ItemsSource manually—handled by bindings
        }

        public void RemoveUser(User user)
        {
            AllUsers.Remove(user);
            if (FilteredUsers.Contains(user)) FilteredUsers.Remove(user);
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditUser(null)); // null для создания
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            _selectedUser = FilteredUsers.FirstOrDefault(u => u.IsSelected); // Добавил IsSelected в модель User (добавь bool IsSelected в User.cs)
            if (_selectedUser != null)
            {
                NavigationService.Navigate(new AddEditUser(_selectedUser));
            }
            else
            {
                MessageBox.Show("Выберите пользователя", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            _selectedUser = FilteredUsers.FirstOrDefault(u => u.IsSelected);
            if (_selectedUser != null)
            {
                var result = MessageBox.Show($"Удалить {_selectedUser.First_Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await App.ApiContext.DeleteUserAsync(_selectedUser.Id);
                        RemoveUser(_selectedUser);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchQuery = SearchTextBox.Text;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // TODO: Implement these stubs for full functionality
        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update selection state on users
            if (UsersGrid.SelectedItem is User selected)
            {
                foreach (var u in FilteredUsers) u.IsSelected = false;
                selected.IsSelected = true;
            }
        }

        private void UserCard_MouseEnter(object sender, MouseEventArgs e)
        {
            // Hover effect logic (e.g., scale or highlight)
            if (sender is Border border) border.Opacity = 0.9;
        }

        private void UserCard_MouseLeave(object sender, MouseEventArgs e)
        {
            // Exit hover logic
            if (sender is Border border) border.Opacity = 1.0;
        }

        private void UserCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Click to select/edit logic
            if (sender is Border border && border.DataContext is User user)
            {
                foreach (var u in FilteredUsers) u.IsSelected = false;
                user.IsSelected = true;
                UsersGrid.SelectedItem = user; // Sync with grid
                BtnEditSelected_Click(null, null); // Optional: auto-edit
            }
        }
    }
}