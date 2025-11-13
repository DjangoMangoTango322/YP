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
    /// Логика взаимодействия для AddEditRestaurant.xaml
    /// </summary>
    public partial class AddEditRestaurant : Page
    {
        private readonly Restaurant _restaurant;
        private bool _isEditMode;

        public AddEditRestaurant(Restaurant restaurant = null)
        {
            InitializeComponent();
            _restaurant = restaurant ?? new Restaurant();
            _isEditMode = restaurant != null;
            LoadData();
            UpdateTitle();
        }

        private void LoadData()
        {
            NameTxt.Text = _restaurant.Name ?? string.Empty;
            AddressTxt.Text = _restaurant.Address ?? string.Empty;
            CapacityTxt.Text = _restaurant.Capacity.ToString();
            OpenTimeTxt.Text = _restaurant.Open_Time.ToString(@"hh");
            CloseTimeTxt.Text = _restaurant.Close_Time.ToString(@"hh");
            TematicTxt.SelectedItem = TematicTxt.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == _restaurant.Tematic);
            if (TematicTxt.SelectedItem == null)
            {
                TematicTxt.SelectedIndex = 0; // Default to first
            }
        }

        private void UpdateTitle()
        {
            PageTitle.Text = _isEditMode ? "🍽️ Редактирование ресторана" : "🍽️ Добавление ресторана";
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            LoadingOverlay.Visibility = Visibility.Visible;
            try
            {
                // Parse fields
                _restaurant.Name = NameTxt.Text.Trim();
                _restaurant.Address = AddressTxt.Text.Trim();
                _restaurant.Capacity = int.Parse(CapacityTxt.Text);
                _restaurant.Open_Time = TimeSpan.Parse(OpenTimeTxt.Text + ":00");
                _restaurant.Close_Time = TimeSpan.Parse(CloseTimeTxt.Text + ":00");
                _restaurant.Tematic = ((ComboBoxItem)TematicTxt.SelectedItem)?.Content?.ToString() ?? string.Empty;

                if (_isEditMode)
                {
                    await App.ApiContext.UpdateRestaurantAsync(_restaurant);
                }
                else
                {
                    await App.ApiContext.CreateRestaurantAsync(_restaurant);
                }

                MessageBox.Show(_isEditMode ? "Ресторан обновлен!" : "Ресторан добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new RestaurantsPage());
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RestaurantsPage());
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameTxt.Text))
            {
                ShowError("Название обязательно.");
                NameTxt.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(AddressTxt.Text))
            {
                ShowError("Адрес обязателен.");
                AddressTxt.Focus();
                return false;
            }
            if (!int.TryParse(CapacityTxt.Text, out int capacity) || capacity <= 0)
            {
                ShowError("Вместимость должна быть положительным числом.");
                CapacityTxt.Focus();
                return false;
            }
            if (!int.TryParse(OpenTimeTxt.Text, out int openHour) || openHour < 0 || openHour > 23)
            {
                ShowError("Время открытия должно быть от 00 до 23.");
                OpenTimeTxt.Focus();
                return false;
            }
            if (!int.TryParse(CloseTimeTxt.Text, out int closeHour) || closeHour < 0 || closeHour > 23 || closeHour <= openHour)
            {
                ShowError("Время закрытия должно быть от 00 до 23 и позже открытия.");
                CloseTimeTxt.Focus();
                return false;
            }
            if (TematicTxt.SelectedItem == null)
            {
                ShowError("Выберите тематику.");
                TematicTxt.Focus();
                return false;
            }
            HideError();
            return true;
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

        private void NumberPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _); // Only allow digits
        }

        private void TimePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _); // Only allow digits (validation on save)
        }
    }
}