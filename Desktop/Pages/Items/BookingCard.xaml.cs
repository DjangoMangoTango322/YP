using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            get => (Booking)GetValue(BookingProperty);
            set => SetValue(BookingProperty, value);
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public BookingCard()
        {
            InitializeComponent();
            DataContext = this;
        }

        private static void OnBookingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BookingCard control)
                control.UpdateStatusColor();
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BookingCard control)
                control.UpdateSelection();
        }

        private void UpdateStatusColor()
        {
            if (Booking == null) return;

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

        private void UpdateSelection()
        {
            MainBorder.BorderBrush = IsSelected
                ? new SolidColorBrush(Color.FromRgb(0, 120, 215))
                : Brushes.Transparent;
            MainBorder.BorderThickness = IsSelected ? new Thickness(3) : new Thickness(0);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            e.Handled = true;
        }
    }
}