using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RestaurantsPage.xaml
    /// </summary>
    public partial class RestaurantsPage : Page
    {
        private Restaurant _selectedRestaurant;
        public RestaurantsPage()
        {
            InitializeComponent();
            LoadRestaurantsFromApi();
        }

        private async void LoadRestaurantsFromApi()
        {
            try
            {
                var restaurants = await App.ApiClient.GetRestaurantsAsync();
                RestaurantsGrid.ItemsSource = restaurants;
                RestaurantsItemsControl.ItemsSource = restaurants;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RestaurantCard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
            }
        }

        private void RestaurantCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            }
        }

        private void RestaurantCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                // Сбрасываем выделение у всех карточек
                foreach (var item in RestaurantsItemsControl.Items)
                {
                    var container = RestaurantsItemsControl.ItemContainerGenerator.ContainerFromItem(item);
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
                            }
                        }
                    }
                }

                // Выделяем текущую карточку
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
                border.BorderThickness = new Thickness(2);

                // Получаем данные ресторана
                var restaurant = border.DataContext as Restaurant;
                if (restaurant != null)
                {
                    _selectedRestaurant = restaurant;
                    RestaurantsGrid.SelectedItem = _selectedRestaurant;
                }
            }
        }

        // Вспомогательные методы для поиска дочерних элементов
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
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

        private Restaurant GetSelectedRestaurant()
        {
            return _selectedRestaurant ?? RestaurantsGrid.SelectedItem as Restaurant;
        }

        private void BtnAddRestaurant_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditRestaurantPage());
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = GetSelectedRestaurant();
            if (restaurant != null)
            {
                NavigationService.Navigate(new AddEditRestaurantPage(restaurant));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите ресторан для изменения", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = GetSelectedRestaurant();
            if (restaurant != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить ресторан \"{restaurant.Name}\"?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var success = await App.ApiClient.DeleteRestaurantAsync(restaurant.Id);
                        if (success)
                        {
                            MessageBox.Show($"🗑️ Ресторан удален через API!\nID: #{restaurant.Id}\nНазвание: {restaurant.Name}",
                                          "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadRestaurantsFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Ошибка удаления ресторана: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите ресторан для удаления", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRestaurantsFromApi();
        }

        private void RestaurantsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRestaurant = RestaurantsGrid.SelectedItem as Restaurant;

            // Обновляем выделение в карточках при выборе в DataGrid
            if (_selectedRestaurant != null)
            {
                foreach (var item in RestaurantsItemsControl.Items)
                {
                    if (item is Restaurant restaurant && restaurant.Id == _selectedRestaurant.Id)
                    {
                        var container = RestaurantsItemsControl.ItemContainerGenerator.ContainerFromItem(item);
                        if (container != null)
                        {
                            var contentPresenter = FindVisualChild<ContentPresenter>(container);
                            if (contentPresenter != null)
                            {
                                var templateBorder = FindVisualChild<Border>(contentPresenter);
                                if (templateBorder != null)
                                {
                                    // Сначала сбрасываем все выделения
                                    foreach (var otherItem in RestaurantsItemsControl.Items)
                                    {
                                        var otherContainer = RestaurantsItemsControl.ItemContainerGenerator.ContainerFromItem(otherItem);
                                        if (otherContainer != null)
                                        {
                                            var otherContentPresenter = FindVisualChild<ContentPresenter>(otherContainer);
                                            if (otherContentPresenter != null)
                                            {
                                                var otherBorder = FindVisualChild<Border>(otherContentPresenter);
                                                if (otherBorder != null)
                                                {
                                                    otherBorder.BorderBrush = Brushes.Transparent;
                                                    otherBorder.BorderThickness = new Thickness(0);
                                                }
                                            }
                                        }
                                    }

                                    // Выделяем выбранную карточку
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

        private void RestaurantsItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Дополнительная логика при загрузке, если нужна
        }

    }
}