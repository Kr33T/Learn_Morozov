﻿using System;
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
using static System.Net.Mime.MediaTypeNames;

namespace Learn_Morozov
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 



    //пройтисб по тз, сделать подсветку скидок, сделать кнопку выбора фото при добавлении услуги, сместить сорт и фильтр, добавить лого, стилизировать, шрифты, посмотреть по проверкам, сделать, чтобы можно было открывать только одно окно
    public partial class MainWindow : Window
    {
        public static bool isAdmin = false;
        List<Service> services;
        bool isWindowOpened = false;

        public MainWindow()
        {
            InitializeComponent();
            DBHelper.le = new learn_entities();
            services = DBHelper.le.Service.ToList();
            serviceLV.ItemsSource = services;
            sortByCostCB.SelectedIndex = 0;
            filterBydiscount.SelectedIndex = 0;
            searchCB.SelectedIndex = 0;
        }

        void clearSort()
        {
            sortByCostCB.SelectedIndex = 0;
            filterBydiscount.SelectedIndex = 0;
            searchCB.SelectedIndex = 0;
            searchTB.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!isWindowOpened)
            {
                adminConfirmation admin = new adminConfirmation();
                isWindowOpened = true;
                admin.Show();

                admin.Closing += (obj, args) =>
                {
                    if (isAdmin)
                    {
                        adminModeBTN.Visibility = Visibility.Collapsed;
                        addAndUpcomingBTNS.Visibility = Visibility.Visible;
                        clearSort();
                        serviceLV.ItemsSource = DBHelper.le.Service.ToList();
                    }
                    else
                    {
                        adminModeBTN.Visibility = Visibility.Visible;
                        addAndUpcomingBTNS.Visibility = Visibility.Collapsed;
                        clearSort();
                        serviceLV.ItemsSource = DBHelper.le.Service.ToList();
                    }
                    isWindowOpened = false;
                };
            }
            else
            {
                MessageBox.Show("Окно уже открыто");
            }
        }

        private void upcomingEnteriesBTN_Click(object sender, RoutedEventArgs e)
        {
            upcomingEnteries window = new upcomingEnteries();
            window.Show();
        }

        public static bool edited = false;

        private void addServiceBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!isWindowOpened)
            {
                addNewService window = new addNewService();
                isWindowOpened = true;
                window.Show();

                window.Closing += (obj, args) =>
                {
                    if (edited)
                    {
                        clearSort();
                        serviceLV.ItemsSource = DBHelper.le.Service.ToList();
                    }
                    edited = false;
                    isWindowOpened = false;
                };
            }
            else
            {
                MessageBox.Show("Окно уже открыто");
            }
        }

        private void previewImg_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as System.Windows.Controls.Image).Uid);
            Service s = DBHelper.le.Service.FirstOrDefault(x => x.ID == id);
            if (!String.IsNullOrEmpty(s.MainImagePath))
            {
                string dir = Environment.CurrentDirectory.Replace("bin\\Debug", s.MainImagePath);
                (sender as System.Windows.Controls.Image).Source = new BitmapImage(new Uri(dir));
            }
            else
            {
                string dir = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\empty.jpg");
                (sender as System.Windows.Controls.Image).Source = new BitmapImage(new Uri(dir));
            }
        }

        private void nameOfServiceTB_Loaded(object sender, RoutedEventArgs e)
        {
            int tbUid = Convert.ToInt32((sender as TextBlock).Uid);
            Service service = DBHelper.le.Service.FirstOrDefault(x => x.ID == tbUid);
            (sender as TextBlock).Text = service.Title;
        }

        private void priceOfServiceTB_Loaded(object sender, RoutedEventArgs e)
        {

            int tbUid = Convert.ToInt32((sender as TextBlock).Uid);
            Service service = DBHelper.le.Service.FirstOrDefault(x => x.ID == tbUid);
            if (!String.IsNullOrEmpty(service.Discount.ToString()) && service.Discount != 0)
            {
                decimal cost = service.Cost - (service.Cost / 100 * Convert.ToDecimal(service.Discount));
                (sender as TextBlock).Text = Math.Round(cost, 0) + " рублей за " + (service.DurationInSeconds / 60) + " минут(ы)";
                (sender as TextBlock).Margin = new Thickness(10, 0, 0, 0);
            }
            else
            {
                (sender as TextBlock).Text = Math.Round(service.Cost, 0) + " рублей за " + (service.DurationInSeconds / 60) + " минут(ы)";
            }
        }

        private void discountForService_Loaded(object sender, RoutedEventArgs e)
        {
            int tbUid = Convert.ToInt32((sender as TextBlock).Uid);
            Service service = DBHelper.le.Service.FirstOrDefault(x => x.ID == tbUid);
            if (!String.IsNullOrEmpty(service.Discount.ToString()) && service.Discount != 0)
            {
                (sender as TextBlock).Text = "* скидка " + service.Discount + "%";
            }
        }

        private void editBTN_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBTN_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void costOfServiceTB_Loaded(object sender, RoutedEventArgs e)
        {
            int tbUid = Convert.ToInt32((sender as TextBlock).Uid);
            Service service = DBHelper.le.Service.FirstOrDefault(x => x.ID == tbUid);
            if (!String.IsNullOrEmpty(service.Discount.ToString()) && service.Discount != 0)
            {
                (sender as TextBlock).Text = Math.Round(service.Cost, 0).ToString();
                (sender as TextBlock).TextDecorations = TextDecorations.Strikethrough;
            }
        }

        private void recordClientBTN_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void buttonsSP_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.isAdmin)
            {
                (sender as StackPanel).Visibility = Visibility.Visible;
            }
        }

        private void sortByCostCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        void filter()
        {
            services = DBHelper.le.Service.ToList();
            switch (sortByCostCB.SelectedIndex)
            {
                case 1:
                    services = services.OrderByDescending(x => x.Cost).ToList();
                    break;
                case 2:
                    services = services.OrderBy(x => x.Cost).ToList();
                    break;
            }
            switch(filterBydiscount.SelectedIndex)
            {
                case 1:
                    services = services.Where(x => x.Discount < 5 || x.Discount == null).ToList();
                    break;
                case 2:
                    services = services.Where(x => x.Discount >= 5 && x.Discount < 15).ToList();
                    break;
                case 3:
                    services = services.Where(x => x.Discount >= 15 && x.Discount < 30).ToList();
                    break;
                case 4:
                    services = services.Where(x => x.Discount >= 30 && x.Discount < 70).ToList();
                    break;
                case 5:
                    services = services.Where(x => x.Discount >= 70 && x.Discount < 100).ToList();
                    break;
            }
            if(!String.IsNullOrEmpty(searchTB.Text))
            {
                switch (searchCB.SelectedIndex)
                {
                    case 0:
                        services = services.Where(x => x.Description.ToLower().Contains(searchTB.Text.ToLower())).ToList();
                        break;
                    case 1:
                        services = services.Where(x => x.Title.ToLower().Contains(searchTB.Text.ToLower())).ToList();
                        break;
                }
            }
            serviceLV.ItemsSource = services;
        }

        private void filterBydiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            filter();
        }

        private void editBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!isWindowOpened)
            {
                int id = Convert.ToInt32((sender as Button).Uid);
                Service srvc = DBHelper.le.Service.FirstOrDefault(x => x.ID == id);
                addNewService window = new addNewService(srvc);
                isWindowOpened = true;
                window.Show();

                window.Closing += (obj, args) =>
                {
                    if (edited)
                    {
                        clearSort();
                        serviceLV.ItemsSource = DBHelper.le.Service.ToList();
                    }
                    edited = false;
                    isWindowOpened = false;
                };
            }
            else
            {
                MessageBox.Show("Окно уже открыто");
            }
        }

        private void deleteBTN_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Хотите удалить эту запись?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                int id = Convert.ToInt32((sender as Button).Uid);
                Service s = DBHelper.le.Service.FirstOrDefault(x => x.ID == id);
                DBHelper.le.Service.Remove(s);
                DBHelper.le.SaveChanges();

                clearSort();
                serviceLV.ItemsSource = DBHelper.le.Service.ToList();
            }
        }

        private void recordClientBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!isWindowOpened)
            {
                int id = Convert.ToInt32((sender as Button).Uid);
                Service s = DBHelper.le.Service.FirstOrDefault(x => x.ID == id);
                signingUpForeTheService window = new signingUpForeTheService(s);
                isWindowOpened = true;
                window.Show();

                window.Closing += (obj, args) =>
                {
                    isWindowOpened = false;
                };
            }
        }
    }
}
