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

namespace Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {
        public ObservableCollection<News> NewsItems { get; set; } = new ObservableCollection<News>();
        public NewsPage()
        {
            InitializeComponent(); 
            NewsList.ItemsSource = NewsItems; 
            LoadData();
        }

        private async void LoadData()
        {
            if (LoadingOverlay == null) return;
            LoadingOverlay.Visibility = Visibility.Visible;
            try
            {
                // Используем правильное название метода из ApiContext
                var news = await App.ApiContext.GetAllNewsAsync();
                NewsItems.Clear();
                if (news != null)
                {
                    // Сортировка по ID, чтобы новости не прыгали
                    var sorted = news.OrderBy(n => n.Id).ToList();
                    foreach (var item in sorted) NewsItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
            finally { LoadingOverlay.Visibility = Visibility.Collapsed; }
        }

        // ОБНОВЛЕНИЕ (ПАРСИНГ)
        private async void BtnParse_Click(object sender, RoutedEventArgs e)
        {
            LoadingOverlay.Visibility = Visibility.Visible;
            try
            {
                // Вызываем RefreshNewsAsync, который стучится в api/News/RefreshNews
                await App.ApiContext.ParseNewsAsync();

                // Перезагружаем список, чтобы увидеть новые новости
                LoadData();
                MessageBox.Show("Парсер на сервере успешно отработал!", "Успех");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вызова парсера: {ex.Message}");
            }
            finally { LoadingOverlay.Visibility = Visibility.Collapsed; }
        }

        // УДАЛЕНИЕ ОДНОЙ НОВОСТИ
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = NewsList.SelectedItem as News;
            if (selected == null)
            {
                MessageBox.Show("Выберите новость в списке!");
                return;
            }

            if (MessageBox.Show($"Удалить новость #{selected.Id}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LoadingOverlay.Visibility = Visibility.Visible;
                try
                {
                    // Вызываем DeleteNewsAsync
                    bool success = await App.ApiContext.DeleteNewsAsync(selected.Id);
                    if (success)
                    {
                        NewsItems.Remove(selected);
                        MessageBox.Show("Удалено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}");
                }
                finally { LoadingOverlay.Visibility = Visibility.Collapsed; }
            }
        }
    }
}