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

namespace Learn_Morozov.Windows
{
    /// <summary>
    /// Логика взаимодействия для serviceGallery.xaml
    /// </summary>
    public partial class serviceGallery : Window
    {
        Service s;

        public serviceGallery(Service s)
        {
            InitializeComponent();
            this.s = s;
            refreshGallery();
        }

        void refreshGallery()
        {
            imageL.ItemsSource = DBHelper.le.ServicePhoto.Where(x => x.ServiceID == s.ID).ToList();
            imageL.SelectedValuePath = "ID";
        }

        private void selectBTN_Click(object sender, RoutedEventArgs e)
        {
            if (imageL.SelectedItem != null)
            {
                int id = Convert.ToInt32(imageL.SelectedValue.ToString());
                ServicePhoto sp = DBHelper.le.ServicePhoto.FirstOrDefault(x => x.ID == id);
                s.MainImagePath = sp.PhotoPath;
                DBHelper.le.SaveChanges();

                this.Close();
            }
        }

        private void deleteBTN_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(imageL.SelectedValue.ToString());
            ServicePhoto sp = DBHelper.le.ServicePhoto.FirstOrDefault(x => x.ID == id);
            DBHelper.le.ServicePhoto.Remove(sp);
            DBHelper.le.SaveChanges();

            refreshGallery();
        }

        private void addBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.ShowDialog();
                string Path = OFD.FileName;
                string file = Path.Substring(Path.LastIndexOf('\\', Path.Length - 1));
                string dir = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\");
                if (!File.Exists($"{dir}{file}"))
                {
                    File.Copy(Path, $"{dir}{file}");
                }
                ServicePhoto sp = new ServicePhoto();
                sp.ServiceID = s.ID;
                sp.PhotoPath = $"Resources\\{file}";
                DBHelper.le.ServicePhoto.Add(sp);
                DBHelper.le.SaveChanges();

                refreshGallery();
            }
            catch { }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ImageI_Loaded(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as System.Windows.Controls.Image).Uid);
            ServicePhoto sp = DBHelper.le.ServicePhoto.FirstOrDefault(x => x.ID == id);
            if (!String.IsNullOrEmpty(sp.PhotoPath))
            {
                string dir = Environment.CurrentDirectory.Replace("bin\\Debug", sp.PhotoPath);
                (sender as System.Windows.Controls.Image).Source = new BitmapImage(new Uri(dir));
            }
            else
            {
                string dir = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\empty.jpg");
                (sender as System.Windows.Controls.Image).Source = new BitmapImage(new Uri(dir));
            }
        }
    }
}
