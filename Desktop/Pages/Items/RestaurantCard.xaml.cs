using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Desktop.Models;

namespace Desktop.Pages.Items
{
    /// <summary>
    /// Логика взаимодействия для RestaurantCard.xaml
    /// </summary>
    public partial class RestaurantCard : UserControl
    {
        public static readonly DependencyProperty RestaurantProperty =
            DependencyProperty.Register(
                "Restaurant",
                typeof(Restaurant),
                typeof(RestaurantCard),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                "IsSelected",
                typeof(bool),
                typeof(RestaurantCard),
                new PropertyMetadata(false, OnIsSelectedChanged));

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
            DataContext = this;
            Loaded += (s, e) => UpdateVisualState(); // Применить начальное состояние
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RestaurantCard control)
            {
                control.UpdateVisualState();
            }
        }

        private void UpdateVisualState()
        {
            var state = IsSelected ? "SelectedState" : "NormalState";
            VisualStateManager.GoToState(this, state, useTransitions: true);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            e.Handled = true;
        }
    }
}