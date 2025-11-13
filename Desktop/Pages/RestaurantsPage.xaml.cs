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
    /// Логика взаимодействия для RestaurantsPage.xaml
    /// </summary>
    public partial class RestaurantsPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Restaurant> _allRestaurants;
        private ObservableCollection<Restaurant> _filteredRestaurants;
        private string _searchQuery;

        public ObservableCollection<Restaurant> AllRestaurants
        {
            get => _allRestaurants;
            set { _allRestaurants = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public ObservableCollection<Restaurant> FilteredRestaurants
        {
            get => _filteredRestaurants;
            set { _filteredRestaurants = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public RestaurantsPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRestaurants();
        }

        private async Task LoadRestaurants()
        {
            try
            {
                var restaurants = await App.ApiContext.GetAllRestaurantsAsync();
                AllRestaurants = new ObservableCollection<Restaurant>(restaurants ?? new List<Restaurant>());
                foreach (var r in AllRestaurants) r.IsSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                AllRestaurants = new ObservableCollection<Restaurant>();
            }
        }

        private void ApplyFilter()
        {
            if (AllRestaurants == null)
            {
                FilteredRestaurants = new ObservableCollection<Restaurant>();
                return;
            }

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredRestaurants = new ObservableCollection<Restaurant>(AllRestaurants);
            }
            else
            {
                var query = SearchQuery.Trim();
                var filtered = AllRestaurants.Where(r =>
                    (r.Name != null && r.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (r.Address != null && r.Address.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (r.Tematic != null && r.Tematic.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    r.Id.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
                FilteredRestaurants = new ObservableCollection<Restaurant>(filtered);
            }
            // No need to set ItemsSource manually—handled by bindings
        }

        private Restaurant GetSelected() => FilteredRestaurants.FirstOrDefault(r => r.IsSelected);

        private void BtnAddRestaurant_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditRestaurant(null));
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            if (selected != null)
                NavigationService.Navigate(new AddEditRestaurant(selected));
            else
                MessageBox.Show("Выберите ресторан", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            if (selected == null) return;
            var result = MessageBox.Show($"Удалить ресторан «{selected.Name}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var success = await App.ApiContext.DeleteRestaurantAsync(selected.Id);
                    if (success)
                    {
                        AllRestaurants.Remove(selected);
                        ApplyFilter();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadRestaurants();
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
        private void RestaurantsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update selection state on restaurants
            if (RestaurantsGrid.SelectedItem is Restaurant selected)
            {
                foreach (var r in FilteredRestaurants) r.IsSelected = false;
                selected.IsSelected = true;
            }
        }

        private void RestaurantCard_MouseEnter(object sender, MouseEventArgs e)
        {
            // Hover effect logic (e.g., scale or highlight)
            if (sender is Border border) border.Opacity = 0.9;
        }

        private void RestaurantCard_MouseLeave(object sender, MouseEventArgs e)
        {
            // Exit hover logic
            if (sender is Border border) border.Opacity = 1.0;
        }

        private void RestaurantCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Click to select/edit logic
            if (sender is Border border && border.DataContext is Restaurant restaurant)
            {
                foreach (var r in FilteredRestaurants) r.IsSelected = false;
                restaurant.IsSelected = true;
                RestaurantsGrid.SelectedItem = restaurant; // Sync with grid
                BtnEditSelected_Click(null, null); // Optional: auto-edit
            }
        }
    }
}