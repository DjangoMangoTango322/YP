using System;
using System.Collections.Generic;
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
        public RegisterPage()
        {
            InitializeComponent();
            TxtFirstName.Focus();

            // Устанавливаем максимальную длину для предотвращения переполнения
            TxtFirstName.MaxLength = 50;
            TxtLastName.MaxLength = 50;
            TxtEmail.MaxLength = 100;
            TxtPhone.MaxLength = 20;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            // Симуляция успешной регистрации
            ShowMessage("Администратор успешно зарегистрирован!", "Успех");

            // Возврат на страницу входа
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new LoginPage());
            }
        }

        private bool ValidateForm()
        {
            ClearError();

            // Проверка имени
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                ShowError("Пожалуйста, введите имя");
                TxtFirstName.Focus();
                return false;
            }

            if (TxtFirstName.Text.Length < 2)
            {
                ShowError("Имя должно содержать минимум 2 символа");
                TxtFirstName.Focus();
                return false;
            }

            // Проверка фамилии
            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                ShowError("Пожалуйста, введите фамилию");
                TxtLastName.Focus();
                return false;
            }

            if (TxtLastName.Text.Length < 2)
            {
                ShowError("Фамилия должна содержать минимум 2 символа");
                TxtLastName.Focus();
                return false;
            }

            // Проверка email
            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                ShowError("Пожалуйста, введите email");
                TxtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(TxtEmail.Text))
            {
                ShowError("Пожалуйста, введите корректный email адрес");
                TxtEmail.Focus();
                return false;
            }

            // Проверка телефона
            if (string.IsNullOrWhiteSpace(TxtPhone.Text))
            {
                ShowError("Пожалуйста, введите телефон");
                TxtPhone.Focus();
                return false;
            }

            // Проверка пароля
            if (TxtPassword.Password.Length == 0)
            {
                ShowError("Пожалуйста, введите пароль");
                TxtPassword.Focus();
                return false;
            }

            if (TxtPassword.Password.Length < 6)
            {
                ShowError("Пароль должен содержать минимум 6 символов");
                TxtPassword.Focus();
                return false;
            }

            if (TxtConfirmPassword.Password.Length == 0)
            {
                ShowError("Пожалуйста, подтвердите пароль");
                TxtConfirmPassword.Focus();
                return false;
            }

            if (TxtPassword.Password != TxtConfirmPassword.Password)
            {
                ShowError("Пароли не совпадают");
                TxtConfirmPassword.Focus();
                TxtConfirmPassword.SelectAll();
                return false;
            }

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

        private void LinkLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new LoginPage());
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MoveToNextField(sender as UIElement);
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == TxtConfirmPassword)
                {
                    BtnRegister_Click(sender, e);
                }
                else
                {
                    MoveToNextField(sender as UIElement);
                }
            }
        }

        private void MoveToNextField(UIElement currentElement)
        {
            var elements = new UIElement[]
            {
                TxtFirstName, TxtLastName, TxtEmail, TxtPhone,
                TxtPassword, TxtConfirmPassword, BtnRegister
            };

            for (int i = 0; i < elements.Length - 1; i++)
            {
                if (elements[i] == currentElement)
                {
                    elements[i + 1].Focus();
                    break;
                }
            }
        }

        private void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}