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
    /// Логика взаимодействия для DishesPage.xaml
    /// </summary>
    public partial class DishesPage : Page
    {
        private Dish _selectedDish;
        public DishesPage()
        {
            InitializeComponent();
            LoadDishesFromApi();
        }

        private async void LoadDishesFromApi()
        {
            try
            {
                var dishes = await App.ApiClient.GetDishesAsync();
                DishesGrid.ItemsSource = dishes;
                DishesItemsControl.ItemsSource = dishes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DishCard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
            }
        }

        private void DishCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            }
        }

        private void DishCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                // Сбрасываем выделение у всех карточек
                foreach (var item in DishesItemsControl.Items)
                {
                    var container = DishesItemsControl.ItemContainerGenerator.ContainerFromItem(item);
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

                // Получаем данные блюда
                var dish = border.DataContext as Dish;
                if (dish != null)
                {
                    _selectedDish = dish;
                    DishesGrid.SelectedItem = _selectedDish;
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

        private Dish GetSelectedDish()
        {
            return _selectedDish ?? DishesGrid.SelectedItem as Dish;
        }

        private void BtnAddDish_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditDishPage());
        }

        private void BtnEditSelected_Click(object sender, RoutedEventArgs e)
        {
            var dish = GetSelectedDish();
            if (dish != null)
            {
                NavigationService.Navigate(new AddEditDishPage(dish));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите блюдо для изменения", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void BtnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var dish = GetSelectedDish();
            if (dish != null)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить блюдо \"{dish.Name}\"?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var success = await App.ApiClient.DeleteDishAsync(dish.Id);
                        if (success)
                        {
                            MessageBox.Show($"🗑️ Блюдо удалено через API!\nID: #{dish.Id}\nНазвание: {dish.Name}",
                                          "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadDishesFromApi();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Ошибка удаления блюда: {ex.Message}", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите блюдо для удаления", "Внимание",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadDishesFromApi();
        }

        private void DishesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDish = DishesGrid.SelectedItem as Dish;

            // Обновляем выделение в карточках при выборе в DataGrid
            if (_selectedDish != null)
            {
                foreach (var item in DishesItemsControl.Items)
                {
                    if (item is Dish dish && dish.Id == _selectedDish.Id)
                    {
                        var container = DishesItemsControl.ItemContainerGenerator.ContainerFromItem(item);
                        if (container != null)
                        {
                            var contentPresenter = FindVisualChild<ContentPresenter>(container);
                            if (contentPresenter != null)
                            {
                                var templateBorder = FindVisualChild<Border>(contentPresenter);
                                if (templateBorder != null)
                                {
                                    // Сначала сбрасываем все выделения
                                    foreach (var otherItem in DishesItemsControl.Items)
                                    {
                                        var otherContainer = DishesItemsControl.ItemContainerGenerator.ContainerFromItem(otherItem);
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

        private void DishesItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Дополнительная логика при загрузке, если нужна
        }
    }
}