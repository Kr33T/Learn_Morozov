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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Learn_Morozov
{
    /// <summary>
    /// Логика взаимодействия для signingUpForeTheService.xaml
    /// </summary>
    public partial class signingUpForeTheService : Window
    {
        Service s;

        public signingUpForeTheService(Service s)
        {
            InitializeComponent();
            this.s = s;
            serviceDetailsL.Text = s.Title + $" ({Math.Round(Convert.ToDouble(s.DurationInSeconds / 60), 0)} мин.)";

            clientCB.ItemsSource = DBHelper.le.Client.ToList();
            clientCB.SelectedValuePath = "ID";
            clientCB.DisplayMemberPath = "FIO";
            clientCB.SelectedIndex = 0;
        }

        private void cancelBTN_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Хотите отменить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
                this.Close();
        }

        DateTime service = new DateTime();

        void checkDate()
        {
            if (!Regex.IsMatch(timeOfServiceTB.Text, @"[0-2][0-9]:[0-5][0-9]"))
            {
                timeOfServiceTB.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                timeOfEndingTB.Text = "Неправильно указано время оказания услуги!";
            }
            else
            {
                if (timeOfServiceTB.Text[0] == '2' && (timeOfServiceTB.Text[1] >= '4' && timeOfServiceTB.Text[1] <= '9'))
                {
                    timeOfServiceTB.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    timeOfEndingTB.Text = "Неправильно указано время оказания услуги!";
                }
                else
                {
                    try
                    {
                        timeOfServiceTB.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        service = DateTime.Parse(dateOfServiceDP.Text + " " + timeOfServiceTB.Text);
                        timeOfEndingTB.Text = $"Дата окончания оказания услуги: {service.AddSeconds(s.DurationInSeconds)}";
                    }
                    catch
                    {

                    }
                }
            }
            if (timeOfServiceTB.Text.Length > 5)
            {
                timeOfServiceTB.Text = timeOfServiceTB.Text.Substring(0, 5);
                timeOfServiceTB.SelectionStart = 5;
            }
        }

        private void timeOfServiceTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkDate();
        }

        private void dateOfServiceDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            checkDate();
        }

        private void addBTN_Click(object sender, RoutedEventArgs e)
        {
            if (dateOfServiceDP.SelectedDate != null)
            {
                if (!String.IsNullOrEmpty(timeOfServiceTB.Text))
                {
                    try
                    {
                        ClientService cs = new ClientService();
                        cs.ClientID = Convert.ToInt32(clientCB.SelectedValue);
                        cs.ServiceID = s.ID;
                        cs.StartTime = service;
                        DBHelper.le.ClientService.Add(cs);
                        DBHelper.le.SaveChanges();
                        this.Close();
                    }
                    catch { }
                }
                else
                {
                    MessageBox.Show("Заполните время начала оказания услуги!");
                }
            }
            else
            {
                MessageBox.Show("Выберите дату!");
            }
        }
    }
}
