using System.Windows.Controls;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = App.Instance;
        }
    }
}
