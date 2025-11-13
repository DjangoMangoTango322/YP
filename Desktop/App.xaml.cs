using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Desktop.Models;

namespace Desktop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        private string _currentAdminLogin;
        private string _currentAdminName;

        public string CurrentAdminLogin
        {
            get => _currentAdminLogin;
            set
            {
                _currentAdminLogin = value;
                OnPropertyChanged();
            }
        }

        public string CurrentAdminName
        {
            get => _currentAdminName;
            set
            {
                _currentAdminName = value;
                OnPropertyChanged();
            }
        }

        public static App Instance { get; private set; }

        public static ApiContext ApiContext { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Instance = this;
            ApiContext = new ApiContext();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ApiContext?.Logout();
            base.OnExit(e);
        }

        public static void SetAdminData(string login, string name)
        {
            if (Instance != null)
            {
                Instance.CurrentAdminLogin = login;
                Instance.CurrentAdminName = name;
            }
        }

        public static void ClearAdminData()
        {
            if (Instance != null)
            {
                Instance.CurrentAdminLogin = null;
                Instance.CurrentAdminName = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}