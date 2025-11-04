using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
            TxtFirstName.Focus();

            // Подписываемся на события изменения текста
            TxtFirstName.TextChanged += TextBox_TextChanged;
            TxtLastName.TextChanged += TextBox_TextChanged;
            TxtEmail.TextChanged += TextBox_TextChanged;
            TxtPhone.TextChanged += TextBox_TextChanged;
            TxtPassword.PasswordChanged += PasswordBox_PasswordChanged;
            TxtConfirmPassword.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            AttemptRegistration();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MoveToNextField(sender);
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptRegistration();
            }
        }

        private void LinkLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void MoveToNextField(object currentField)
        {
            if (currentField == TxtFirstName)
                TxtLastName.Focus();
            else if (currentField == TxtLastName)
                TxtEmail.Focus();
            else if (currentField == TxtEmail)
                TxtPhone.Focus();
            else if (currentField == TxtPhone)
                TxtPassword.Focus();
            else if (currentField == TxtPassword)
                TxtConfirmPassword.Focus();
            else if (currentField == TxtConfirmPassword)
                AttemptRegistration();
        }

        private async void AttemptRegistration()
        {
            string firstName = TxtFirstName.Text.Trim();
            string lastName = TxtLastName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string phone = TxtPhone.Text.Trim();
            string password = TxtPassword.Password;
            string confirmPassword = TxtConfirmPassword.Password;

            // Валидация
            if (!ValidateInput(firstName, lastName, email, phone, password, confirmPassword))
                return;

            // Показываем индикатор загрузки
            SetLoadingState(true);

            try
            {
                // Создаем объект для регистрации (если у User нет Password, возможно нужно использовать другой подход)
                // Вариант 1: Если API ожидает отдельный DTO для регистрации
                var registrationData = new
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Phone = phone,
                    Password = password
                };

                // Вариант 2: Если нужно использовать существующий User класс
                var newUser = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Phone = phone
                    // Password не добавляем, так как его нет в классе User
                };

                // Если RegisterUserAsync не существует, используем CreateUserAsync или другой доступный метод
                // Замените RegisterUserAsync на правильный метод из вашего ApiClient
                var createdUser = await App.ApiClient.CreateUserAsync(newUser);

                ShowSuccess($"✅ Пользователь успешно зарегистрирован через API!\nID: #{createdUser.Id}\nEmail: {createdUser.Email}");
                ClearFields();
            }
            catch (Exception ex)
            {
                ShowError($"❌ Ошибка регистрации через API: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private bool ValidateInput(string firstName, string lastName, string email, string phone, string password, string confirmPassword)
        {
            // Проверка имени
            if (string.IsNullOrWhiteSpace(firstName))
            {
                ShowError("Введите имя");
                TxtFirstName.Focus();
                return false;
            }

            if (firstName.Length < 2)
            {
                ShowError("Имя должно содержать минимум 2 символа");
                TxtFirstName.Focus();
                return false;
            }

            // Проверка фамилии
            if (string.IsNullOrWhiteSpace(lastName))
            {
                ShowError("Введите фамилию");
                TxtLastName.Focus();
                return false;
            }

            if (lastName.Length < 2)
            {
                ShowError("Фамилия должна содержать минимум 2 символа");
                TxtLastName.Focus();
                return false;
            }

            // Проверка email
            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Введите email");
                TxtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(email))
            {
                ShowError("Введите корректный email");
                TxtEmail.Focus();
                return false;
            }

            // Проверка телефона
            if (string.IsNullOrWhiteSpace(phone))
            {
                ShowError("Введите телефон");
                TxtPhone.Focus();
                return false;
            }

            // Проверка пароля
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Введите пароль");
                TxtPassword.Focus();
                return false;
            }

            if (password.Length < 6)
            {
                ShowError("Пароль должен содержать минимум 6 символов");
                TxtPassword.Focus();
                return false;
            }

            // Проверка подтверждения пароля
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowError("Подтвердите пароль");
                TxtConfirmPassword.Focus();
                return false;
            }

            if (password != confirmPassword)
            {
                ShowError("Пароли не совпадают");
                TxtConfirmPassword.Focus();
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
            BtnRegister.Content = isLoading ? "⏳ Регистрация..." : "Зарегистрировать";
            BtnRegister.IsEnabled = !isLoading;
            TxtFirstName.IsEnabled = !isLoading;
            TxtLastName.IsEnabled = !isLoading;
            TxtEmail.IsEnabled = !isLoading;
            TxtPhone.IsEnabled = !isLoading;
            TxtPassword.IsEnabled = !isLoading;
            TxtConfirmPassword.IsEnabled = !isLoading;
            LinkLogin.IsEnabled = !isLoading;
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            TxtError.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 107, 107));
            ErrorBorder.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(90, 0, 0));
            ErrorBorder.Visibility = Visibility.Visible;
        }

        private void ShowSuccess(string message)
        {
            TxtError.Text = message;
            TxtError.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 255, 107));
            ErrorBorder.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 90, 0));
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HideError();
        }

        private void ClearFields()
        {
            TxtFirstName.Clear();
            TxtLastName.Clear();
            TxtEmail.Clear();
            TxtPhone.Clear();
            TxtPassword.Clear();
            TxtConfirmPassword.Clear();
            TxtFirstName.Focus();
        }

        // Метод для тестирования (можно удалить в продакшене)
        public void FillTestData()
        {
            TxtFirstName.Text = "Тест";
            TxtLastName.Text = "Пользователь";
            TxtEmail.Text = "test@example.com";
            TxtPhone.Text = "+7 (999) 123-45-67";
            TxtPassword.Password = "password123";
            TxtConfirmPassword.Password = "password123";
        }
    }
}