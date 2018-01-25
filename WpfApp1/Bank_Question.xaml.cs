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

namespace WpfApp1
{
    /// <summary>
    /// Bank_Question.xaml 的交互逻辑
    /// </summary>
    public partial class Bank_Question : Window
    {
        public Bank_Question()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            CountDown countdown = new CountDown(endtime,this);
        }

        private void Button_back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bank_question(object sender, RoutedEventArgs e)
        {
          
            Window bank = new Bank_Question();
            bank.Show();
            Close();
        }

        private void single_question(object sender, RoutedEventArgs e)
        {
            Window single = new Single();
            single.Show();
            Close();
        }
    }
}
