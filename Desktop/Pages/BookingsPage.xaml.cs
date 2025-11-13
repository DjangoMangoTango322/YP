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
using System.Windows.Media;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookingsPage.xaml
    /// </summary>
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

        public BookingsPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadBookings();
        }

        private async Task LoadBookings()
        {
            try
            {
                var bookings = await App.ApiContext.GetAllBookingsAsync();
                AllBookings = new ObservableCollection<Booking>(bookings ?? new List<Booking>());
                foreach (var b in AllBookings) b.IsSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                AllBookings = new ObservableCollection<Booking>();
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
                var query = SearchQuery.Trim();
                var filtered = AllBookings.Where(b =>
                    (b.Status != null && b.Status.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    b.Id.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.User_Id.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    b.Restaurant_Id.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
                FilteredBookings = new ObservableCollection<Booking>(filtered);
            }
            // No need to set ItemsSource manually—handled by bindings
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadBookings();
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
        private void BtnAddBooking_Click(object sender, RoutedEventArgs e) { /* Add logic */ }
        private void BtnEditSelected_Click(object sender, RoutedEventArgs e) { /* Edit logic */ }
        private void BtnDeleteSelected_Click(object sender, RoutedEventArgs e) { /* Delete logic */ }
        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) { /* Selection logic */ }
        private void BookingCard_MouseEnter(object sender, MouseEventArgs e) { /* Hover logic */ }
        private void BookingCard_MouseLeave(object sender, MouseEventArgs e) { /* Hover exit logic */ }
        private void BookingCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { /* Click logic */ }
    }
}