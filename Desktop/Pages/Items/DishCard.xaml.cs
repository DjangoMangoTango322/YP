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
    /// Логика взаимодействия для DishCard.xaml
    /// </summary>
    public partial class DishCard : UserControl
    {
        public static readonly DependencyProperty DishProperty =
           DependencyProperty.Register("Dish", typeof(Dish), typeof(DishCard));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(DishCard));

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
            this.DataContext = this;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
        }
    }
}
