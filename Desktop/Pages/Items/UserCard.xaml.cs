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
using Desktop.Models;

namespace Desktop.Pages.Items
{
    /// <summary>
    /// Логика взаимодействия для UserCard.xaml
    /// </summary>
    public partial class UserCard : UserControl
    {
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(User), typeof(UserCard),
                new PropertyMetadata(null, OnUserChanged));

        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public UserCard()
        {
            InitializeComponent();
           
        }

        private static void OnUserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UserCard;
            control.DataContext = control.User;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            MainBorder.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            MainBorder.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
            MainBorder.BorderThickness = new Thickness(2);
        }
    }
}


