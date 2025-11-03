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
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditRestaurantPage.xaml
    /// </summary>
    public partial class AddEditRestaurantPage : Page
    {
        private Restaurant _restaurant;
        private bool _isEditMode = false;
        public AddEditRestaurantPage()
        {
            InitializeComponent();
            InitializeForm();
        }
        public AddEditRestaurantPage(Restaurant restaurant) : this()
        {
            _restaurant = restaurant;
            _isEditMode = true;
            PageTitle.Text = "🍽️ Редактирование ресторана";
            LoadRestaurantData();
        }

        private void InitializeForm()
        {
            CmbTematic.SelectedIndex = 0;
        }

        private void LoadRestaurantData()
        {
            if (_restaurant != null)
            {
                TxtName.Text = _restaurant.Name;
                TxtAddress.Text = _restaurant.Address;
                TxtCapacity.Text = _restaurant.Capacity.ToString();

                TxtOpenHours.Text = _restaurant.OpenTime.Hours.ToString("00");
                TxtOpenMinutes.Text = _restaurant.OpenTime.Minutes.ToString("00");
                TxtCloseHours.Text = _restaurant.CloseTime.Hours.ToString("00");
                TxtCloseMinutes.Text = _restaurant.CloseTime.Minutes.ToString("00");

                // Выбираем тематику в комбобоксе
                foreach (ComboBoxItem item in CmbTematic.Items)
                {
                    if (item.Content.ToString() == _restaurant.Tematic)
                    {
                        CmbTematic.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            SetLoadingState(true);

            try
            {
                var restaurant = new Restaurant
                {
                    Name = TxtName.Text.Trim(),
                    Address = TxtAddress.Text.Trim(),
                    Capacity = int.Parse(TxtCapacity.Text),
                    OpenTime = new TimeSpan(int.Parse(TxtOpenHours.Text), int.Parse(TxtOpenMinutes.Text), 0),
                    CloseTime = new TimeSpan(int.Parse(TxtCloseHours.Text), int.Parse(TxtCloseMinutes.Text), 0),
                    Tematic = (CmbTematic.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                if (_isEditMode && _restaurant != null)
                {
                    restaurant.Id = _restaurant.Id;
                    var updatedRestaurant = await App.ApiClient.UpdateRestaurantAsync(restaurant.Id, restaurant);
                    MessageBox.Show($"✅ Ресторан успешно обновлен!\nID: #{updatedRestaurant.Id}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var createdRestaurant = await App.ApiClient.CreateRestaurantAsync(restaurant);
                    MessageBox.Show($"✅ Ресторан успешно создан!\nID: #{createdRestaurant.Id}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Возвращаемся назад
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                    NavigationService.Navigate(new RestaurantsPage());
            }
            catch (Exception ex)
            {
                ShowError($"❌ Ошибка сохранения: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new RestaurantsPage());
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                ShowError("Введите название ресторана");
                TxtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                ShowError("Введите адрес ресторана");
                TxtAddress.Focus();
                return false;
            }

            if (!int.TryParse(TxtCapacity.Text, out int capacity) || capacity <= 0)
            {
                ShowError("Введите корректную вместимость (положительное число)");
                TxtCapacity.Focus();
                return false;
            }

            if (!IsValidTime(TxtOpenHours.Text, TxtOpenMinutes.Text))
            {
                ShowError("Введите корректное время открытия");
                TxtOpenHours.Focus();
                return false;
            }

            if (!IsValidTime(TxtCloseHours.Text, TxtCloseMinutes.Text))
            {
                ShowError("Введите корректное время закрытия");
                TxtCloseHours.Focus();
                return false;
            }

            if (CmbTematic.SelectedItem == null)
            {
                ShowError("Выберите тематику ресторана");
                CmbTematic.Focus();
                return false;
            }

            HideError();
            return true;
        }

        private bool IsValidTime(string hours, string minutes)
        {
            if (!int.TryParse(hours, out int h) || !int.TryParse(minutes, out int m))
                return false;

            return h >= 0 && h <= 23 && m >= 0 && m <= 59;
        }

        private void NumberPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void TimePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void SetLoadingState(bool isLoading)
        {
            LoadingOverlay.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            IsEnabled = !isLoading;
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            ErrorBorder.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorBorder.Visibility = Visibility.Collapsed;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HideError();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HideError();
        }
    }
}