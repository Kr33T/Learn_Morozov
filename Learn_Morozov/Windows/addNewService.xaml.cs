using Learn_Morozov.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Learn_Morozov
{
    /// <summary>
    /// Логика взаимодействия для addNewService.xaml
    /// </summary>
    public partial class addNewService : Window
    {
        Service service;
        bool isEditing = false;

        public addNewService()
        {
            InitializeComponent();
        }

        public addNewService(Service s)
        {
            InitializeComponent();
            service = s;
            isEditing = true;
            buttonsSP.Visibility = Visibility.Visible;
            newImageSP.Visibility = Visibility.Hidden;
            saveChangesBTN.Content = "Изменить";

            serviceID.Content = s.ID;
            serviceTitleTB.Text = s.Title;
            serviceCostTB.Text = s.Cost.ToString();
            serviceDiscountTB.Text = s.Discount.ToString();
            serviceDurationTB.Text = s.DurationInSeconds.ToString();
            serviceDescriptionTB.Text = s.Description;

            if (!String.IsNullOrEmpty(s.MainImagePath))
            {
                string dir = Environment.CurrentDirectory.Replace("bin\\Debug", s.MainImagePath);
                imageI.Source = new BitmapImage(new Uri(dir));
            }
            else
            {
                string dir = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\empty.jpg");
                imageI.Source = new BitmapImage(new Uri(dir));
            }
        }

        private void deletePhotoBTN_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Хотите удалить изображение для этой услуги?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                service.MainImagePath = "Resources\\empty.jpg";
                DBHelper.le.SaveChanges();

                if (!String.IsNullOrEmpty(service.MainImagePath))
                {
                    string dir = Environment.CurrentDirectory.Replace("bin\\Debug", service.MainImagePath);
                    imageI.Source = new BitmapImage(new Uri(dir));
                }
                else
                {
                    string dir = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\empty.jpg");
                    imageI.Source = new BitmapImage(new Uri(dir));
                }
            }
        }

        private void cancelBTN_Click(object sender, RoutedEventArgs e)
        {
            cancel();
        }
        
        void cancel()
        {
            var res = MessageBox.Show("Хотите отменить изменения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void saveChangesBTN_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        void save()
        {
            if (isEditing)
            {
                bool isOccuredAnError = false;
                string lastTitle = service.Title;
                List<Service> check = DBHelper.le.Service.ToList();
                check.Remove(service);
                if(serviceTitleTB.Text != lastTitle)
                {
                    if (check.Where(x => x.Title == serviceTitleTB.Text).Count() != 0)
                    {
                        MessageBox.Show("Такая услуга уже существует!");
                        isOccuredAnError = true;
                    }
                    else
                    {
                        service.Title = serviceTitleTB.Text;
                    }
                }
                if (!String.IsNullOrEmpty(serviceCostTB.Text))
                {
                    if (Convert.ToDecimal(serviceCostTB.Text) > 0)
                    {
                        service.Cost = Convert.ToDecimal(serviceCostTB.Text);
                    }
                    else
                    {
                        MessageBox.Show("Цена не может отрицательной!");
                        isOccuredAnError = true;
                    }
                }
                else
                {
                    MessageBox.Show("Цена не может быть пустой!");
                    isOccuredAnError = true;
                }

                if (!String.IsNullOrEmpty(serviceDiscountTB.Text))
                {
                    if (Convert.ToDouble(serviceDiscountTB.Text) >= 0 && Convert.ToDouble(serviceDiscountTB.Text) <= 100)
                    {
                        service.Discount = Convert.ToDouble(serviceDiscountTB.Text);
                    }
                    else
                    {
                        MessageBox.Show("Скидка не может быть отрицательной или более 100%!");
                        isOccuredAnError = true;
                    }
                }
                else
                {
                    service.Discount = 0;
                }

                if (!String.IsNullOrEmpty(serviceDurationTB.Text))
                {
                    if(Convert.ToInt32(serviceDurationTB.Text) > 0 && Convert.ToInt32(serviceDurationTB.Text) <= 14400)
                    {
                        service.DurationInSeconds = Convert.ToInt32(serviceDurationTB.Text);
                    }
                    else
                    {
                        MessageBox.Show("Продолжительность не может быть отрицательной или более 14400 секунд (4 часа)!");
                        isOccuredAnError = true;
                    }
                }
                else
                {
                    MessageBox.Show("Продолжительность не может быть нулевой!");
                    isOccuredAnError = true;
                }
                service.Description = serviceDescriptionTB.Text;
                if (!isOccuredAnError)
                {
                    DBHelper.le.SaveChanges();
                    MainWindow.edited = true;
                    this.Close();
                }
            }
            else
            {
                bool flag = false;
                Service addService = new Service();

                if (DBHelper.le.Service.Where(x => x.Title == serviceTitleTB.Text).Count() == 0)
                {
                    addService.Title = serviceTitleTB.Text;
                }
                else
                {
                    MessageBox.Show("Такая услуга уже существует!");
                    flag = true;
                }

                if (!String.IsNullOrEmpty(serviceCostTB.Text))
                {
                    addService.Cost = Convert.ToDecimal(serviceCostTB.Text);
                }
                else
                {
                    MessageBox.Show("Цена не может быть пустой!");
                    flag = true;
                }
                if (!String.IsNullOrEmpty(serviceDiscountTB.Text))
                {
                    addService.Discount = Convert.ToDouble(serviceDiscountTB.Text);
                }
                else
                {
                    addService.Discount = 0;
                }
                if (!String.IsNullOrEmpty(serviceDurationTB.Text))
                {
                    addService.DurationInSeconds = Convert.ToInt32(serviceDurationTB.Text);
                }
                else
                {
                    MessageBox.Show("Продолжительность не может быть нулевой!");
                    flag |= true;
                }
                addService.Description = serviceDescriptionTB.Text;
                //image
                if (!flag)
                {
                    DBHelper.le.Service.Add(addService);
                    DBHelper.le.SaveChanges();
                    MainWindow.edited = true;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    save();
                    break;
                case Key.Escape:
                    cancel();
                    break;
            }
        }

        private void openGaleryBTN_Click(object sender, RoutedEventArgs e)
        {
            serviceGallery window = new serviceGallery(service);
            window.Show();

            window.Closing += (obj, args) =>
            {
                if (!String.IsNullOrEmpty(service.MainImagePath))
                {
                    string dir = Environment.CurrentDirectory.Replace("bin\\Debug", service.MainImagePath);
                    imageI.Source = new BitmapImage(new Uri(dir));
                }
                else
                {
                    string dir = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\empty.jpg");
                    imageI.Source = new BitmapImage(new Uri(dir));
                }
            };
        }

        private void addNewBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
