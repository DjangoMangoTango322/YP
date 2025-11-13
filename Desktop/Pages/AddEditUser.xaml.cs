using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditUser.xaml
    /// </summary>
    public partial class AddEditUser : Page
    {
        private User _user;

        public AddEditUser(User user)
        {
            InitializeComponent();
            _user = user ?? new User();
            LoadData();
        }

        private void LoadData()
        {
            FirstName.Text = _user.First_Name;
            LastName.Text = _user.Last_Name;
            Phone.Text = _user.Phone;
            Login.Text = _user.Login;
            // Password не загружаем для безопасности
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            _user.First_Name = FirstName.Text;
            _user.Last_Name = LastName.Text;
            _user.Phone = Phone.Text;
            _user.Login = Login.Text;
            _user.Password = Password.Password; // Если новый пароль

            try
            {
                if (_user.Id == 0)
                {
                    await App.ApiContext.CreateUserAsync(_user);
                }
                else
                {
                    await App.ApiContext.UpdateUserAsync(_user);
                }
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}