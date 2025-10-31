using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Desktop.Models;

namespace Desktop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public static ApiClient ApiClient { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ApiClient = new ApiClient("http://localhost:5000/api/");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ApiClient?.Dispose();
            base.OnExit(e);
        }
    }
}
