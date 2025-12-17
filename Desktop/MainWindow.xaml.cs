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
using Desktop;
using Desktop.Pages;

namespace Desktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AdminLogin());
        }

        public void ShowMenu()
        {
            MenuPanel.Visibility = Visibility.Visible;
        }

        public void HideMenu()
        {
            MenuPanel.Visibility = Visibility.Collapsed;
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
            SetActiveButton(BtnHome);
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
            SetActiveButton(BtnUsers);
        }

        private void BtnRestaurants_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RestaurantsPage());
            SetActiveButton(BtnRestaurants);
        }

        private void BtnDishes_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DishesPage());
            SetActiveButton(BtnDishes);
        }

        private void BtnBookings_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BookingsPage());
            SetActiveButton(BtnBookings);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            App.ApiContext.Logout();
            App.ClearAdminData();
            HideMenu();
            MainFrame.Navigate(new AdminLogin());
        }

        public void SetActiveButton(Button button)
        {
            // Сброс всех кнопок
            BtnHome.Background = (Brush)FindResource("PrimaryBrush");
            BtnUsers.Background = (Brush)FindResource("PrimaryBrush");
            BtnRestaurants.Background = (Brush)FindResource("PrimaryBrush");
            BtnDishes.Background = (Brush)FindResource("PrimaryBrush");
            BtnBookings.Background = (Brush)FindResource("PrimaryBrush");

            // Активная кнопка
            button.Background = (Brush)FindResource("AccentBrush");
        }
    }
}

//: Window
//    {
//        public static MainWindow Instance { get; private set; }
//public MainWindow()
//{
//    InitializeComponent();
//    Instance = this;
//    this.Resources = Application.Current.Resources;
//    NavigateToLogin();
//}
//public void ShowMenu()
//{
//    MenuPanel.Visibility = Visibility.Visible;
//}

//public void HideMenu()
//{
//    MenuPanel.Visibility = Visibility.Collapsed;
//}
//public void NavigateToMainPage()
//{
//    MainFrame.Navigate(new MainPage());
//}
//public void NavigateToLogin()
//{
//    MainFrame.Navigate(new LoginPage());
//}
//    }
//}