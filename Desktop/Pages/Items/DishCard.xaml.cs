using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Models;

namespace Desktop.Pages.Items
{
    /// <summary>
    /// Логика взаимодействия для DishCard.xaml
    /// </summary>
    public partial class DishCard : UserControl
    {
       public static readonly DependencyProperty DishProperty =
           DependencyProperty.Register("Dish", typeof(Dish), typeof(DishCard),
               new PropertyMetadata(null, OnDishChanged));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(DishCard),
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
        }

        private static void OnDishChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DishCard;
            if (control != null && e.NewValue is Dish dish)
            {
                control.DataContext = dish; // Устанавливаем DataContext на блюдо
            }
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DishCard;
            control?.UpdateSelectionAppearance();
        }

        private void UpdateSelectionAppearance()
        {
            // Обновление внешнего вида при выделении
            if (IsSelected)
            {
                VisualStateManager.GoToElementState(this, "SelectedState", true);
            }
            else
            {
                VisualStateManager.GoToElementState(this, "NormalState", true);
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            e.Handled = true;
        }
    }
}