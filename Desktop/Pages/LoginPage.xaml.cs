using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
       
        public LoginPage()
        {
            InitializeComponent();
            TxtLogin.Focus();

            TxtLogin.TextChanged += TxtLogin_TextChanged;
            TxtPassword.PasswordChanged += TxtPassword_PasswordChanged;

            AutoFillTestData();
        }

        private void AutoFillTestData()
        {
            TxtLogin.Text = "admin@restaurant.com";
            TxtPassword.Password = "admin123";
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void TxtLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TxtPassword.Focus();
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptLogin();
            }
        }

        private void LinkRegister_Click(object sender, RoutedEventArgs e)
        {
            ShowError("Функция регистрации временно недоступна");
        }

        private async void AttemptLogin()
        {
            string login = TxtLogin.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrWhiteSpace(login))
            {
                ShowError("Введите логин");
                TxtLogin.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Введите пароль");
                TxtPassword.Focus();
                return;
            }

            if (CheckTestCredentials(login, password))
            {
                NavigateToMainPage();
                return;
            }

            SetLoadingState(true);

            try
            {
                var user = await App.ApiClient.AuthenticateAsync(login, password);
                if (user != null)
                {
                    App.CurrentUser = user;
                    NavigateToMainPage();
                }
                else
                {
                    ShowError("Неверный логин или пароль");
                    TxtPassword.SelectAll();
                    TxtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка входа: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private bool CheckTestCredentials(string login, string password)
        {
            var testCredentials = new[]
            {
                new { Login = "admin", Password = "admin123" },
                new { Login = "admin@restaurant.com", Password = "admin123" },
                new { Login = "administrator", Password = "123456" },
                new { Login = "test", Password = "test" }
            };

            foreach (var cred in testCredentials)
            {
                if (login == cred.Login && password == cred.Password)
                {
                    App.CurrentUser = new User
                    {
                        Id = 1,
                        FirstName = "Администратор",
                        LastName = "Системы",
                        Email = "admin@restaurant.com",
                        Phone = "+7 (495) 123-45-67",
                        CreatedAt = DateTime.Now
                    };
                    return true;
                }
            }

            return false;
        }

        private void NavigateToMainPage()
        {
            try
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.NavigateToMainPage();

                    TxtLogin.Clear();
                    TxtPassword.Clear();
                    HideError();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка перехода: {ex.Message}");
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            BtnLogin.Content = isLoading ? "⏳ Вход..." : "Войти в систему";
            BtnLogin.IsEnabled = !isLoading;
            TxtLogin.IsEnabled = !isLoading;
            TxtPassword.IsEnabled = !isLoading;
          
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

        private void TxtLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            HideError();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HideError();
        }
    }
}