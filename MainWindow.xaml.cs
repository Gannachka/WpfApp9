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
using Vitvor._7_8WPF;

namespace WpfApp7_8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            Cursor = new Cursor(@"D:\4 семестр\ООП\WPF7-8withMVVM-master\S.Cursor\arrow.cur");
            DataContext = new TaskVM(this);
            InitializeComponent();
        }

        private void customControl_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have just click your custom control");
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы нажали на МОЮ кнопку");
        }
    }
}
