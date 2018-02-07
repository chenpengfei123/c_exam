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
    /// Question_Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Question_Manager : Window
    {
        string sql;
        public Question_Manager()
        {
            InitializeComponent();
            sql = "select * from single_question";
            single_manager.ItemsSource = db_connect.GetTables(sql).DefaultView;
            sql = "select * from bank_question";
            bank_manager.ItemsSource = db_connect.GetTables(sql).DefaultView;
        }
    }
}
