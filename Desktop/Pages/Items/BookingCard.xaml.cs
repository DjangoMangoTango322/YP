using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Models;

namespace Desktop.Pages.Items
{
    public partial class BookingCard : System.Windows.Controls.UserControl
    {
        public Booking Booking
        {
            get => (Booking)GetValue(BookingProperty);
            set => SetValue(BookingProperty, value);
        }

        public static readonly DependencyProperty BookingProperty =
            DependencyProperty.Register("Booking", typeof(Booking), typeof(BookingCard), new PropertyMetadata(null));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(BookingCard), new PropertyMetadata(false));

        public BookingCard()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void BookingCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ItemsControl.ItemsControlFromItemContainer(this) is ItemsControl parent)
            {
                foreach (var item in parent.Items)
                {
                    if (item is Booking booking)
                    {
                        booking.IsSelected = false;
                    }
                }
            }

            if (Booking != null)
            {
                Booking.IsSelected = true;
            }

            e.Handled = true;
        }
    }
}