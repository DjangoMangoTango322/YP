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
    /// Логика взаимодействия для RestaurantCard.xaml
    /// </summary>
    public partial class RestaurantCard : UserControl
    {
        public static readonly DependencyProperty RestaurantProperty =
            DependencyProperty.Register("Restaurant", typeof(Restaurant), typeof(RestaurantCard));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(RestaurantCard));

        public Restaurant Restaurant
        {
            get => (Restaurant)GetValue(RestaurantProperty);
            set => SetValue(RestaurantProperty, value);
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public RestaurantCard()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
        }
    }
}
