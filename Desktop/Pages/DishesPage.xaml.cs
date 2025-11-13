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
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DishesPage.xaml
    /// </summary>
    public partial class DishesPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Dish> _allDishes;
        private ObservableCollection<Dish> _filteredDishes;
        private string _searchQuery;

        public ObservableCollection<Dish> AllDishes
        {
            get => _allDishes;
            set { _allDishes = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public ObservableCollection<Dish> FilteredDishes
        {
            get => _filteredDishes;
            set { _filteredDishes = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public DishesPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDishes();
        }

        private async Task LoadDishes()
        {
            try
            {
                var dishes = await App.ApiContext.GetAllDishesAsync();
                AllDishes = new ObservableCollection<Dish>(dishes ?? new List<Dish>());
                foreach (var d in AllDishes) d.IsSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                AllDishes = new ObservableCollection<Dish>();
            }
        }

        private void ApplyFilter()
        {
            if (AllDishes == null)
            {
                FilteredDishes = new ObservableCollection<Dish>();
                return;
            }

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredDishes = new ObservableCollection<Dish>(AllDishes);
            }
            else
            {
                var query = SearchQuery.Trim();
                var filtered = AllDishes.Where(d =>
                    (d.Name?.IndexOf(query, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                    (d.Description?.IndexOf(query, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                    (d.Category?.IndexOf(query, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                    d.Id.ToString().IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
                FilteredDishes = new ObservableCollection<Dish>(filtered);
            }
            // No need to set ItemsSource manually—handled by bindings
        }

        private Dish GetSelected() => FilteredDishes.FirstOrDefault(d => d.IsSelected);

        private void BtnAddDish_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditDish(null));
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            if (selected != null)
                NavigationService.Navigate(new AddEditDish(selected));
            else
                MessageBox.Show("Выберите блюдо", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            if (selected == null) return;
            var result = MessageBox.Show($"Удалить блюдо «{selected.Name}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var success = await App.ApiContext.DeleteDishAsync(selected.Id);
                    if (success)
                    {
                        AllDishes.Remove(selected);
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
            await LoadDishes();
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
        private void DishesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update selection state on dishes
            if (DishesGrid.SelectedItem is Dish selected)
            {
                foreach (var d in FilteredDishes) d.IsSelected = false;
                selected.IsSelected = true;
            }
        }

        private void DishCard_MouseEnter(object sender, MouseEventArgs e)
        {
            // Hover effect logic (e.g., scale or highlight)
            if (sender is Border border) border.Opacity = 0.9;
        }

        private void DishCard_MouseLeave(object sender, MouseEventArgs e)
        {
            // Exit hover logic
            if (sender is Border border) border.Opacity = 1.0;
        }

        private void DishCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Click to select/edit logic
            if (sender is Border border && border.DataContext is Dish dish)
            {
                foreach (var d in FilteredDishes) d.IsSelected = false;
                dish.IsSelected = true;
                DishesGrid.SelectedItem = dish; // Sync with grid
                BtnEditSelected_Click(null, null); // Optional: auto-edit
            }
        }
    }
}