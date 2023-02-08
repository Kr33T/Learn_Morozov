using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Learn_Morozov
{
    /// <summary>
    /// Логика взаимодействия для upcomingEnteries.xaml
    /// </summary>
    public partial class upcomingEnteries : Window
    {
        DispatcherTimer timer;

        public upcomingEnteries()
        {
            InitializeComponent();
            DateTime today = DateTime.Today.AddDays(2);
            List<ClientService> csl = DBHelper.le.ClientService.Where(x => x.StartTime >= DateTime.Now).ToList();
            csl = csl.Where(x => x.StartTime <= today).OrderBy(x => x.StartTime).ToList();
            clientServiceLB.ItemsSource = csl;
            clientServiceLB.SelectedValuePath = "ID";

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(forTimer);
            timer.Interval = new TimeSpan(0,0,30);
            timer.Start();
        }

        void forTimer(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today.AddDays(2);
            List<ClientService> csl = DBHelper.le.ClientService.Where(x => x.StartTime >= DateTime.Now).ToList();
            csl = csl.Where(x => x.StartTime <= today).OrderBy(x => x.StartTime).ToList();
            clientServiceLB.ItemsSource = csl;
            clientServiceLB.SelectedValuePath = "ID";
        }

        private void nameOfServiceTB_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as TextBlock).Uid);
            ClientService cs = DBHelper.le.ClientService.FirstOrDefault(x => x.ID == id);
            (sender as TextBlock).Text = $"Наименование услуги: {cs.Service.Title}";
        }

        private void contactInfoTB_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as TextBlock).Uid);
            ClientService cs = DBHelper.le.ClientService.FirstOrDefault(x => x.ID == id);
            (sender as TextBlock).Text = $"e-mail: {cs.Client.Email}; телефон: {cs.Client.Phone}";
        }

        private void timeRemainingTB_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as TextBlock).Uid);
            ClientService cs = DBHelper.le.ClientService.FirstOrDefault(x => x.ID == id);
            DateTime now = DateTime.Now;
            DateTime service = DateTime.Parse(cs.StartTime.ToString());
            (sender as TextBlock).Text = $"Времени до начала: {(now.Subtract(service).Hours + (now.Subtract(service).Days * 24)).ToString().Replace("-", "")} часов {now.Subtract(service).Minutes.ToString().Replace("-", "")} минут";
        
            if(now.Subtract(service).Hours == 0)
            {
                (sender as TextBlock).Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private void nameOfClientTB_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as TextBlock).Uid);
            ClientService cs = DBHelper.le.ClientService.FirstOrDefault(x => x.ID == id);
            (sender as TextBlock).Text = $"Клиент: {cs.Client.FIO}";
        }

        private void dateOfServiceTB_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as TextBlock).Uid);
            ClientService cs = DBHelper.le.ClientService.FirstOrDefault(x => x.ID == id);
            (sender as TextBlock).Text = $"Время начала: {cs.StartTime}";
        }
    }
}
