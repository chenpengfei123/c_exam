using System;
using System.Collections.Generic;
using System.Data;
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
    /// Answer_Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Answer_Manager : Window
    {
        string sql_single;
        string sql_bank;
        DataTable dataTable;
        public Answer_Manager()
        {
            InitializeComponent();
            sql_single = "select  * from single_answer_stu";
            ShowSingleAnswer();

            sql_bank = "select * from bank_answer_stu";
            ShowBankAnswer();
        }
        private void ShowSingleAnswer()
        {
            dataTable = db_connect.GetTables(sql_single);
            dataTable.Columns[0].ColumnName = "题目编号";
            dataTable.Columns[1].ColumnName = "学号";
            dataTable.Columns[2].ColumnName = "答案";
            dataTable.Columns[3].ColumnName = "章节";
            dataTable.Columns[4].ColumnName = "作答时间";
            Answer_Single.ItemsSource = dataTable.DefaultView;
        }


        private void ShowBankAnswer()
        {
            dataTable = db_connect.GetTables(sql_bank);
            dataTable.Columns[0].ColumnName = "题目编号";
            dataTable.Columns[1].ColumnName = "学号";
            dataTable.Columns[2].ColumnName = "答案";
            dataTable.Columns[3].ColumnName = "章节";
           dataTable.Columns[4].ColumnName = "作答时间";
            Answer_Bank.ItemsSource = dataTable.DefaultView;
        }

        private void Sure_Click(object sender, RoutedEventArgs e)
        {
            if (!userid.Text.Trim().Equals("")&subject.Text.Trim().Equals(""))
            {
                sql_single = "select  * from single_answer_stu where stu_id="+ userid.Text.Trim();
                ShowSingleAnswer();

                sql_bank = "select * from bank_answer_stu where stu_id=" + userid.Text.Trim();
                ShowBankAnswer();
            }
            else if (userid.Text.Trim().Equals("") & !subject.Text.Trim().Equals(""))
            {
                sql_single = "select  * from single_answer_stu where subject=" + subject.Text.Trim();
                ShowSingleAnswer();

                sql_bank = "select * from bank_answer_stu where subject=" + subject.Text.Trim();
                ShowBankAnswer();
            }
            else if(!userid.Text.Trim().Equals("") & !subject.Text.Trim().Equals(""))
            {
                sql_single = "select  * from single_answer_stu where subject=" + subject.Text.Trim()+" and stu_id="+ userid.Text.Trim();
                ShowSingleAnswer();

                sql_bank = "select * from bank_answer_stu where subject=" + subject.Text.Trim() + " and stu_id=" + userid.Text.Trim();
                ShowBankAnswer();
            }
        }
    }
}
