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

namespace WpfApp1.Bank
{
    /// <summary>
    /// AddBankQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class AddBankQuestion : Window
    {
        public AddBankQuestion()
        {
            InitializeComponent();
        }

        private void add_bank_Click(object sender, RoutedEventArgs e)
        {
            if (bank_name.Text.Equals("") | bank_answer.Equals(""))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }
            String sql = "insert into bank_question(ques_name,ques_answer) values('" + bank_name.Text + "','" + bank_answer.Text + "')";
            db_connect.AddNonQuery(sql);
        }
    }
}
