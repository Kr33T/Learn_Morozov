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

namespace Learn_Morozov
{
    /// <summary>
    /// Логика взаимодействия для adminConfirmation.xaml
    /// </summary>
    public partial class adminConfirmation : Window
    {
        public adminConfirmation()
        {
            InitializeComponent();
            passwordTB.Focus();
        }

        private void confirmBTN_Click(object sender, RoutedEventArgs e)
        {
            confirm();
        }

        private void confirm()
        {
            if (passwordTB.Password == "0000")
            {
                MainWindow.isAdmin = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Некорректный пароль!");
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                confirm();
            }
        }
    }
}
