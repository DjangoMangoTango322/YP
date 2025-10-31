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
    /// Логика взаимодействия для BookingCard.xaml
    /// </summary>
    public partial class BookingCard : UserControl
    {
        public static readonly DependencyProperty BookingProperty =
           DependencyProperty.Register("Booking", typeof(Booking), typeof(BookingCard));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(BookingCard));

        public Booking Booking
        {
            get => (Booking)GetValue(BookingProperty);
            set => SetValue(BookingProperty, value);
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public Brush StatusColor
        {
            get
            {
                return Booking?.Status switch
                {
                    "Подтверждено" => new SolidColorBrush(Color.FromRgb(46, 125, 50)),
                    "Ожидание" => new SolidColorBrush(Color.FromRgb(251, 140, 0)),
                    "Отменено" => new SolidColorBrush(Color.FromRgb(198, 40, 40)),
                    _ => new SolidColorBrush(Color.FromRgb(100, 100, 100))
                };
            }
        }

        public BookingCard()
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
