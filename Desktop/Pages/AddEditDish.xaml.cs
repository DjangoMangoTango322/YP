using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditDish.xaml
    /// </summary>
    public partial class AddEditDish : Page
    {
        private readonly Dish _dish;
        private bool _isEditMode;

        public AddEditDish(Dish dish = null)
        {
            InitializeComponent();
            _dish = dish ?? new Dish();
            _isEditMode = dish != null;
            LoadData();
            UpdateTitle();
        }

        private void LoadData()
        {
            NameTxt.Text = _dish.Name ?? string.Empty;
            DescriptionTxt.Text = _dish.Description ?? string.Empty;
            PriceTxt.Text = _dish.Price.ToString("F2");
            CategoryTxt.SelectedItem = CategoryTxt.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == _dish.Category);
            if (CategoryTxt.SelectedItem == null)
            {
                CategoryTxt.SelectedIndex = 0; // Default to first
            }
        }

        private void UpdateTitle()
        {
            PageTitle.Text = _isEditMode ? "🍕 Редактирование блюда" : "🍕 Добавление блюда";
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            LoadingOverlay.Visibility = Visibility.Visible;
            try
            {
                // Parse fields
                _dish.Name = NameTxt.Text.Trim();
                _dish.Description = DescriptionTxt.Text.Trim();
                _dish.Price = decimal.Parse(PriceTxt.Text);
                _dish.Category = ((ComboBoxItem)CategoryTxt.SelectedItem)?.Content?.ToString() ?? string.Empty;

                if (_isEditMode)
                {
                    await App.ApiContext.UpdateDishAsync(_dish);
                }
                else
                {
                    await App.ApiContext.CreateDishAsync(_dish);
                }

                MessageBox.Show(_isEditMode ? "Блюдо обновлено!" : "Блюдо добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new DishesPage());
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
            NavigationService.Navigate(new DishesPage());
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameTxt.Text))
            {
                ShowError("Название обязательно.");
                NameTxt.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(DescriptionTxt.Text))
            {
                ShowError("Описание обязательно.");
                DescriptionTxt.Focus();
                return false;
            }
            if (!decimal.TryParse(PriceTxt.Text, out decimal price) || price <= 0)
            {
                ShowError("Цена должна быть положительным числом.");
                PriceTxt.Focus();
                return false;
            }
            if (CategoryTxt.SelectedItem == null)
            {
                ShowError("Выберите категорию.");
                CategoryTxt.Focus();
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

        private void DecimalPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow digits, one decimal point, and negative sign
            string text = (sender as TextBox)?.Text ?? string.Empty;
            string newText = text.Insert((sender as TextBox)?.SelectionStart ?? 0, e.Text);
            e.Handled = !decimal.TryParse(newText, out _);
        }
    }
}