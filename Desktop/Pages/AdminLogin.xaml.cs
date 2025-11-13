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

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminLogin.xaml
    /// </summary>
    public partial class AdminLogin : Page
    {
        public AdminLogin()
        {
            InitializeComponent();
            App.ApiContext.Logout(); // Очистка при загрузке
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text.Trim();
            string password = Password.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoginButton.IsEnabled = false;
            LoginButton.Content = "Вход...";

            try
            {
                bool success = await App.ApiContext.LoginAdminAsync(login, password);
                if (success)
                {
                    if (Window.GetWindow(this) is MainWindow mainWindow)
                    {
                        mainWindow.ShowMenu();
                        mainWindow.MainFrame.Navigate(new HomePage());
                        mainWindow.SetActiveButton(mainWindow.BtnHome);
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoginButton.IsEnabled = true;
                LoginButton.Content = "Войти";
            }
        }
    }
}