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
             DependencyProperty.Register("Booking", typeof(Booking), typeof(BookingCard),
                 new PropertyMetadata(null, OnBookingChanged));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(BookingCard),
                new PropertyMetadata(false, OnIsSelectedChanged));

        public Booking Booking
        {
            get { return (Booking)GetValue(BookingProperty); }
            set { SetValue(BookingProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public BookingCard()
        {
            InitializeComponent();
           
        }

        private static void OnBookingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BookingCard;
            control.DataContext = control.Booking;
            control.UpdateStatusColor();
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BookingCard;
            control.UpdateSelectionAppearance();
        }

        private void UpdateStatusColor()
        {
            if (Booking == null || StatusBorder == null) return;

            switch (Booking.Status)
            {
                case "Подтверждено":
                    StatusBorder.Background = new SolidColorBrush(Color.FromRgb(46, 125, 50));
                    break;
                case "Ожидание":
                    StatusBorder.Background = new SolidColorBrush(Color.FromRgb(251, 140, 0));
                    break;
                case "Отменено":
                    StatusBorder.Background = new SolidColorBrush(Color.FromRgb(198, 40, 40));
                    break;
                default:
                    StatusBorder.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                    break;
            }
        }

        private void UpdateSelectionAppearance()
        {
            if (IsSelected)
            {
                MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
                MainBorder.BorderThickness = new Thickness(2);
            }
            else
            {
                MainBorder.BorderBrush = Brushes.Transparent;
                MainBorder.BorderThickness = new Thickness(0);
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected; // Переключаем выделение
        }
    }
}