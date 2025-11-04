using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditDishPage.xaml
    /// </summary>
    public partial class AddEditDishPage : Page
    {
        private Dish _dish;
        private bool _isEditMode = false;
        public AddEditDishPage()
        {
            InitializeComponent();
            InitializeForm();
        }

        public AddEditDishPage(Dish dish) : this()
        {
            _dish = dish;
            _isEditMode = true;
            PageTitle.Text = " Редактирование блюда";
            LoadDishData();
        }

        private void InitializeForm()
        {
            CmbCategory.SelectedIndex = 0;
            TxtPrice.Text = "0";
        }

        private void LoadDishData()
        {
            if (_dish != null)
            {
                TxtName.Text = _dish.Name;
                TxtDescription.Text = _dish.Description;
                TxtPrice.Text = _dish.Price.ToString("0");

                // Выбираем категорию в комбобоксе
                foreach (ComboBoxItem item in CmbCategory.Items)
                {
                    if (item.Content.ToString() == _dish.Category)
                    {
                        CmbCategory.SelectedItem = item;
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
                var dish = new Dish
                {
                    Name = TxtName.Text.Trim(),
                    Description = TxtDescription.Text.Trim(),
                    Price = decimal.Parse(TxtPrice.Text),
                    Category = (CmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                if (_isEditMode && _dish != null)
                {
                    dish.Id = _dish.Id;
                    var updatedDish = await App.ApiClient.UpdateDishAsync(dish.Id, dish);
                    MessageBox.Show($"✅ Блюдо успешно обновлено!\nID: #{updatedDish.Id}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var createdDish = await App.ApiClient.CreateDishAsync(dish);
                    MessageBox.Show($"✅ Блюдо успешно создано!\nID: #{createdDish.Id}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                    NavigationService.Navigate(new DishesPage());
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
                NavigationService.Navigate(new DishesPage());
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                ShowError("Введите название блюда");
                TxtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtDescription.Text))
            {
                ShowError("Введите описание блюда");
                TxtDescription.Focus();
                return false;
            }

            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price < 0)
            {
                ShowError("Введите корректную цену (неотрицательное число)");
                TxtPrice.Focus();
                return false;
            }

            if (CmbCategory.SelectedItem == null)
            {
                ShowError("Выберите категорию блюда");
                CmbCategory.Focus();
                return false;
            }

            HideError();
            return true;
        }

        private void DecimalPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var newText = textBox.Text + e.Text;

            // Разрешаем только цифры и одну точку/запятую
            e.Handled = !(char.IsDigit(e.Text, 0) ||
                         (e.Text == "." && !newText.Contains(".") && !newText.Contains(",")) ||
                         (e.Text == "," && !newText.Contains(".") && !newText.Contains(",")));
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