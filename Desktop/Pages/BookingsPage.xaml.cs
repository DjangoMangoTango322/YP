using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Desktop.Models;
using Desktop.Converters;  // ← добавь это, если Visual Studio ругается на IdToNameConverter

namespace Desktop.Pages
{
    public partial class BookingsPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Booking> _allBookings;
        private ObservableCollection<Booking> _filteredBookings;
        private string _searchQuery;

        public ObservableCollection<Booking> AllBookings
        {
            get => _allBookings;
            set { _allBookings = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public ObservableCollection<Booking> FilteredBookings
        {
            get => _filteredBookings;
            set { _filteredBookings = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private readonly Dictionary<int, string> _userLastNames = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _restaurantNames = new Dictionary<int, string>();

        public BookingsPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadBookings();
        }

        public async Task LoadBookings()
        {
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                var bookings = await App.ApiContext.GetAllBookingsAsync();
                var users = await App.ApiContext.GetAllUsersAsync();
                var restaurants = await App.ApiContext.GetAllRestaurantsAsync();

                _userLastNames.Clear();
                foreach (var user in users ?? new List<User>())
                {
                    _userLastNames[user.Id] = user.Last_Name;
                }

                _restaurantNames.Clear();
                foreach (var rest in restaurants ?? new List<Restaurant>())
                {
                    _restaurantNames[rest.Id] = rest.Name;
                }

                AllBookings = new ObservableCollection<Booking>(bookings ?? new List<Booking>());
                foreach (var b in AllBookings)
                {
                    b.IsSelected = false;
                }

                if (Resources["UserConverter"] is IdToNameConverter userConv)
                {
                    userConv.NameDictionary = _userLastNames;
                }

                if (Resources["RestaurantConverter"] is IdToNameConverter restConv)
                {
                    restConv.NameDictionary = _restaurantNames;
                }

                ApplyFilter();  // Обновляем FilteredBookings
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                AllBookings = new ObservableCollection<Booking>();
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void ApplyFilter()
        {
            if (AllBookings == null)
            {
                FilteredBookings = new ObservableCollection<Booking>();
                return;
            }

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredBookings = new ObservableCollection<Booking>(AllBookings);
            }
            else
            {
                var query = SearchQuery.Trim().ToLowerInvariant();
                var filtered = AllBookings.Where(b =>
                    b.Id.ToString().Contains(query) ||
                    (_userLastNames.ContainsKey(b.User_Id) && _userLastNames[b.User_Id].ToLowerInvariant().Contains(query)) ||
                    (_restaurantNames.ContainsKey(b.Restaurant_Id) && _restaurantNames[b.Restaurant_Id].ToLowerInvariant().Contains(query)) ||
                    b.Booking_Date.ToString("dd.MM.yyyy").Contains(query) ||
                    b.Number_Of_Guests.ToString().Contains(query) ||
                    (b.Status?.ToLowerInvariant().Contains(query) ?? false)
                ).ToList();

                FilteredBookings = new ObservableCollection<Booking>(filtered);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchQuery = SearchTextBox.Text;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _ = LoadBookings();
        }

        private void BtnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditBooking());
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var selected = FilteredBookings.FirstOrDefault(b => b.IsSelected);
            if (selected == null)
            {
                MessageBox.Show("Выберите бронирование для редактирования.", "Внимание");
                return;
            }
            NavigationService.Navigate(new AddEditBooking(selected));  // Теперь работает!
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var selected = FilteredBookings.FirstOrDefault(b => b.IsSelected);
            if (selected == null)
            {
                MessageBox.Show("Выберите бронирование для удаления.", "Внимание");
                return;
            }

            var result = MessageBox.Show($"Удалить бронирование №{selected.Id}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                LoadingOverlay.Visibility = Visibility.Visible;
                try
                {
                    bool success = await App.ApiContext.DeleteBookingAsync(selected.Id);
                    if (success)
                    {
                        AllBookings.Remove(selected);
                        ApplyFilter();
                        MessageBox.Show("Бронирование удалено.", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить.", "Ошибка");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка");
                }
                finally
                {
                    LoadingOverlay.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is Booking selected)
            {
                foreach (var b in FilteredBookings)
                {
                    b.IsSelected = false;
                }
                selected.IsSelected = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}