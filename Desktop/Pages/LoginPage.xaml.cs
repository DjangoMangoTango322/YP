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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            TxtEmail.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = TxtEmail.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Пожалуйста, введите email");
                TxtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Пожалуйста, введите пароль");
                TxtPassword.Focus();
                return;
            }

            // Mock authentication
            if ((email == "admin@restaurant.com" || email == "admin") && password == "admin")
            {
                ClearError();

                // Success animation
                var animation = new DoubleAnimation(1, 1.05, TimeSpan.FromSeconds(0.1));
                animation.AutoReverse = true;
                BtnLogin.BeginAnimation(Button.OpacityProperty, animation);

                // Navigate to main system
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowMenu();
                }
            }
            else
            {
                ShowError("Неверный email или пароль");
                TxtPassword.SelectAll();
                TxtPassword.Focus();
            }
        }

        private void LinkRegister_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new RegisterPage());
            }
        }

        private void ShowError(string message)
        {
            TxtError.Text = message;
            ErrorBorder.Visibility = Visibility.Visible;

            var animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            ErrorBorder.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void ClearError()
        {
            var animation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            animation.Completed += (s, e) => ErrorBorder.Visibility = Visibility.Collapsed;
            ErrorBorder.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void TxtEmail_KeyDown(object sender, KeyEventArgs e)
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
                BtnLogin_Click(sender, e);
            }
        }
    }
}