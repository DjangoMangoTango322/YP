using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditUserPage.xaml
    /// </summary>
    public partial class AddEditUserPage : Page
    {
        private User _user;
        private bool _isEditMode = false;

        public AddEditUserPage()
        {
            InitializeComponent();
        }
        public AddEditUserPage(User user) : this()
        {
            _user = user;
            _isEditMode = true;
            PageTitle.Text = "👥 Редактирование пользователя";
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (_user != null)
            {
                TxtFirstName.Text = _user.FirstName;
                TxtLastName.Text = _user.LastName;
                TxtEmail.Text = _user.Email;
                TxtPhone.Text = _user.Phone;
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            SetLoadingState(true);

            try
            {
                var user = new User
                {
                    FirstName = TxtFirstName.Text.Trim(),
                    LastName = TxtLastName.Text.Trim(),
                    Email = TxtEmail.Text.Trim(),
                    Phone = TxtPhone.Text.Trim()
                };

                if (_isEditMode && _user != null)
                {
                    user.Id = _user.Id;
                    var updatedUser = await App.ApiClient.UpdateUserAsync(user.Id, user);
                    MessageBox.Show($"✅ Пользователь успешно обновлен!\nID: #{updatedUser.Id}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var createdUser = await App.ApiClient.CreateUserAsync(user);
                    MessageBox.Show($"✅ Пользователь успешно создан!\nID: #{createdUser.Id}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                    NavigationService.Navigate(new UsersPage());
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
                NavigationService.Navigate(new UsersPage());
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                ShowError("Введите имя пользователя");
                TxtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                ShowError("Введите фамилию пользователя");
                TxtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtEmail.Text) || !IsValidEmail(TxtEmail.Text))
            {
                ShowError("Введите корректный email адрес");
                TxtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtPhone.Text))
            {
                ShowError("Введите телефон пользователя");
                TxtPhone.Focus();
                return false;
            }

            HideError();
            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
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
    }
}
