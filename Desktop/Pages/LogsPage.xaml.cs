using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;          // ← для Mouse и Cursors
using System.Windows.Threading;      // ← для DispatcherTimer
using Desktop.Models;

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogsPage.xaml
    /// </summary>
    public partial class LogsPage : Page
    {
        private readonly HttpClient _httpClient;
        private readonly DispatcherTimer _timer;
        private EventHandler _tickHandler;
        public LogsPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7237/"),  // твой адрес бэкенда
                Timeout = TimeSpan.FromSeconds(15)
            };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(12)  // каждые 12 секунд
            };

            _tickHandler = async (s, e) => await LoadLogsAsync();
            _timer.Tick += _tickHandler;
            _timer.Start();

            _ = LoadLogsAsync();
        }
        private async Task LoadLogsAsync()
        {
            try
            {
                LogsGrid.IsEnabled = false;
                Mouse.OverrideCursor = Cursors.Wait;

                var logs = await _httpClient.GetFromJsonAsync<List<Log>>("api/Audit/all");

                if (logs != null && logs.Count > 0)
                {
                    // Сортируем по времени по убыванию (самые свежие сверху)
                    logs.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));
                    LogsGrid.ItemsSource = logs;

                    LastUpdateText.Text = $"Последнее обновление: {DateTime.Now:HH:mm:ss}";
                }
                else
                {
                    LastUpdateText.Text = "Логи пусты";
                }
            }
            catch (Exception ex)
            {
                LastUpdateText.Text = "Ошибка загрузки";
                MessageBox.Show($"Не удалось загрузить логи:\n{ex.Message}");
            }
            finally
            {
                LogsGrid.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }
        }
        private async void RefreshLogs_Click(object sender, RoutedEventArgs e)
        {
            await LoadLogsAsync();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Tick -= _tickHandler;
                _timer.Stop();
            }
        }
    }
}
 