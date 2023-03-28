using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string[] diff = { "Легкий", "Нормальный", "Сложный", "Невозможный" };
        public Window1()
        {
            InitializeComponent();
            difficult_cmb.ItemsSource = diff;
        }


        private void play_btn_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow win2 = new MainWindow(Array.IndexOf(diff, difficult_cmb.Text)+4);
            
            win2.Show();
            this.Close();
        }

        private void rules_btn_Click(object sender, RoutedEventArgs e)
        {
            Window2 win2 = new Window2();
            win2.Show();
            this.Close();
        }
        private void developers_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Кустов Тимофей\n\rШадрин Андрей");
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
