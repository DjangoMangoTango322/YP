using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для AddEditUser.xaml
    /// </summary>
    public partial class AddEditUser : Page
    {
        private User _user;
        private readonly bool _isEditMode;

        public AddEditUser(User user = null)
        {
            InitializeComponent();
            _user = user ?? new User();
            _isEditMode = user != null;
            LoadData();
            UpdateTitle();
        }

        private void LoadData()
        {
            FirstName.Text = _user.First_Name ?? string.Empty;
            LastName.Text = _user.Last_Name ?? string.Empty;
            Phone.Text = _user.Phone ?? string.Empty;
            Login.Text = _user.Login ?? string.Empty;

            // Пароль не загружаем — поле остаётся пустым
            Password.Password = string.Empty;

            PageTitle.Text = _isEditMode ? "👤 Редактирование пользователя" : "👤 Добавление пользователя";
        }

        private void UpdateTitle()
        {
            PageTitle.Text = _isEditMode ? "👤 Редактирование пользователя" : "👤 Добавление пользователя";
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                _user.First_Name = FirstName.Text.Trim();
                _user.Last_Name = LastName.Text.Trim();
                _user.Phone = Phone.Text.Trim();
                _user.Login = Login.Text.Trim();

                // Меняем пароль ТОЛЬКО если введён новый
                string newPassword = Password.Password.Trim();
                if (!string.IsNullOrEmpty(newPassword))
                {
                    _user.Password = newPassword;
                }
                // Если поле пустое — сервер не получит новый пароль и оставит старый

                if (_isEditMode)
                {
                    await App.ApiContext.UpdateUserAsync(_user);
                    MessageBox.Show("Пользователь обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await App.ApiContext.CreateUserAsync(_user);
                    MessageBox.Show("Пользователь добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(FirstName.Text))
            {
                MessageBox.Show("Введите имя.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                FirstName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(LastName.Text))
            {
                MessageBox.Show("Введите фамилию.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                LastName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(Phone.Text))
            {
                MessageBox.Show("Введите телефон.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                Phone.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(Login.Text))
            {
                MessageBox.Show("Введите логин.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                Login.Focus();
                return false;
            }

            string newPassword = Password.Password.Trim();
            if (!_isEditMode && string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Введите пароль для нового пользователя.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                Password.Focus();
                return false;
            }

            return true;
        }
    }
}