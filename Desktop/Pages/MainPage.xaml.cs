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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            UpdateUserInfo();
            ShowWelcome(); // Показываем приветствие по умолчанию
        }

        private void UpdateUserInfo()
        {
            if (App.CurrentUser != null)
            {
                TxtUserInfo.Text = $"Пользователь: {App.CurrentUser.FirstName} {App.CurrentUser.LastName}";
                TxtAdminGreeting.Text = $"Уважаемый администратор {App.CurrentUser.FirstName} {App.CurrentUser.LastName}!\n\n" +
                                      "Вы успешно вошли в систему управления ресторанами.\n" +
                                      "Для начала работы выберите нужный раздел в меню навигации.";
            }
            else
            {
                TxtUserInfo.Text = "Пользователь не авторизован";
                TxtAdminGreeting.Text = "Добро пожаловать в систему управления ресторанами!\n\n" +
                                      "Для доступа к функциям системы необходимо войти в систему.";
            }
        }

        // Метод для показа приветствия
        private void ShowWelcome()
        {
            MainFrame.Visibility = Visibility.Collapsed;
            WelcomeSection.Visibility = Visibility.Visible;
            ResetMenuButtons();
        }

        private void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
            MainFrame.Visibility = Visibility.Visible;
            WelcomeSection.Visibility = Visibility.Collapsed;
            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            // Reset all buttons
            ResetMenuButtons();

            // Highlight active button based on current page type
            if (MainFrame.Content is RestaurantsPage)
                SetActiveButton(BtnRestaurants);
            else if (MainFrame.Content is DishesPage)
                SetActiveButton(BtnDishes);
            else if (MainFrame.Content is UsersPage)
                SetActiveButton(BtnUsers);
            else if (MainFrame.Content is BookingsPage)
                SetActiveButton(BtnBookings);
            else if (MainFrame.Visibility == Visibility.Collapsed)
                SetActiveButton(BtnHome);
        }

        private void ResetMenuButtons()
        {
            var buttons = new[] { BtnHome, BtnRestaurants, BtnDishes, BtnUsers, BtnBookings };
            foreach (var button in buttons)
            {
                button.ClearValue(Button.BackgroundProperty);
                button.ClearValue(Button.ForegroundProperty);
            }
        }

        private void SetActiveButton(Button button)
        {
            button.Background = new SolidColorBrush(Color.FromArgb(255, 139, 0, 0)); // #8B0000
            button.Foreground = Brushes.White;
        }

        // Обработчики кнопок меню
        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            ShowWelcome();
        }

        private void BtnRestaurants_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new RestaurantsPage());
        }

        private void BtnDishes_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new DishesPage());
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new UsersPage());
        }

        private void BtnBookings_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(new BookingsPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Clear user data
                App.CurrentUser = null;

                // Navigate back to login
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.NavigateToLogin();
            }
        }
    }
}