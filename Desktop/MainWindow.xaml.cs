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
using Desktop.Pages;

namespace Desktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            // Apply custom styling
            this.Resources = Application.Current.Resources;

            // Start with login page
            NavigateToLogin();
        }

        public void ShowMenu()
        {
            // Menu is always visible now, but we update navigation
            UpdateMenuButtons();
            NavigateToPage(new RestaurantsPage());
        }

        public void HideMenu()
        {
            NavigateToLogin();
        }

        private void NavigateToLogin()
        {
            MainFrame.Navigate(new LoginPage());
        }

        private void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            // Reset all buttons
            ResetMenuButtons();

            // Highlight active button
            if (MainFrame.Content is RestaurantsPage)
                SetActiveButton(BtnRestaurants);
            else if (MainFrame.Content is DishesPage)
                SetActiveButton(BtnDishes);
            else if (MainFrame.Content is UsersPage)
                SetActiveButton(BtnUsers);
            else if (MainFrame.Content is BookingsPage)
                SetActiveButton(BtnBookings);
        }

        private void ResetMenuButtons()
        {
            var buttons = new[] { BtnRestaurants, BtnDishes, BtnUsers, BtnBookings };
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
                NavigateToLogin();
                ResetMenuButtons();
            }
        }
    }
}
