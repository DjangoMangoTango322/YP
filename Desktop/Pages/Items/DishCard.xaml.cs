using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Desktop.Models;

namespace Desktop.Pages.Items
{
    /// <summary>
    /// Логика взаимодействия для DishCard.xaml
    /// </summary>
    public partial class DishCard : UserControl
    {
        public static readonly DependencyProperty DishProperty =
            DependencyProperty.Register(
                "Dish",
                typeof(Dish),
                typeof(DishCard),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                "IsSelected",
                typeof(bool),
                typeof(DishCard),
                new PropertyMetadata(false, OnIsSelectedChanged));

        public Dish Dish
        {
            get => (Dish)GetValue(DishProperty);
            set => SetValue(DishProperty, value);
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public DishCard()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (s, e) => UpdateVisualState(); // Применяем начальное состояние
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DishCard control)
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