using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private string connectionString = "Server=localhost;Database=RestaurantDB;Integrated Security=true;";
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
            // Возврат на страницу входа
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

        private void AttemptRegistration()
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

            // Попытка регистрации
            System.Threading.Tasks.Task.Run(() =>
            {
                bool isRegistered = RegisterUser(firstName, lastName, email, phone, password);

                Dispatcher.Invoke(() =>
                {
                    SetLoadingState(false);

                    if (isRegistered)
                    {
                        ShowSuccess("Пользователь успешно зарегистрирован!");
                        ClearFields();
                    }
                });
            });
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

        private bool RegisterUser(string firstName, string lastName, string email, string phone, string password)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверяем, не существует ли уже пользователь с таким email
                    var checkQuery = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@email", email);
                        int existingUsers = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (existingUsers > 0)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                ShowError("Пользователь с таким email уже существует");
                            });
                            return false;
                        }
                    }

                    // Регистрируем нового пользователя с паролем
                    var insertQuery = @"
                        INSERT INTO users (first_name, last_name, email, phone, password, created_at)
                        VALUES (@first_name, @last_name, @email, @phone, @password, @created_at);
                        SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@first_name", firstName);
                        command.Parameters.AddWithValue("@last_name", lastName);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@phone", phone);
                        command.Parameters.AddWithValue("@password", password); // В реальном приложении используйте хеширование!
                        command.Parameters.AddWithValue("@created_at", DateTime.Now);

                        var newId = Convert.ToInt32(command.ExecuteScalar());

                        if (newId > 0)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                ShowSuccess($"Пользователь успешно зарегистрирован!\nID: {newId}");
                            });
                            return true;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Dispatcher.Invoke(() =>
                {
                    ShowError($"Ошибка базы данных: {sqlEx.Message}");
                });
                return false;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    ShowError($"Ошибка регистрации: {ex.Message}");
                });
                return false;
            }

            return false;
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