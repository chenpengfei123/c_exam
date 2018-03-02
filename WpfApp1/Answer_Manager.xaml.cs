using MySql.Data.MySqlClient;
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
        MySqlParameter[] mySqlParameter;
        string sql_single;
        string sql_bank;
        DataTable dataTable_single;
        DataTable dataTable_bank;
        public Answer_Manager()
        {
            InitializeComponent();
            sql_single = "select  * from single_answer_stu";
    
            ShowSingleAnswer();

            sql_bank = "select * from bank_answer_stu";
          
            ShowBankAnswer();
        }
        private void ShowSingleAnswer(params MySqlParameter[] commandParameters)
        {
            dataTable_single = db_connect.GetTables(sql_single,commandParameters );
            dataTable_single.Columns[0].ColumnName = "题目编号";
            dataTable_single.Columns[1].ColumnName = "学号";
            dataTable_single.Columns[2].ColumnName = "答案";
            dataTable_single.Columns[3].ColumnName = "章节";
            dataTable_single.Columns[4].ColumnName = "作答时间";
            Answer_Single.ItemsSource = dataTable_single.DefaultView;
        }


        private void ShowBankAnswer(params MySqlParameter[] commandParameters)
        {
            dataTable_bank = db_connect.GetTables(sql_bank, commandParameters);
            dataTable_bank.Columns[0].ColumnName = "题目编号";
            dataTable_bank.Columns[1].ColumnName = "学号";
            dataTable_bank.Columns[2].ColumnName = "答案";
            dataTable_bank.Columns[3].ColumnName = "章节";
            dataTable_bank.Columns[4].ColumnName = "作答时间";
            Answer_Bank.ItemsSource = dataTable_bank.DefaultView;
        }

        private void Sure_Click(object sender, RoutedEventArgs e)
        {
            if (!userid.Text.Trim().Equals("")&subject.Text.Trim().Equals(""))
            {
               
                sql_single = "select  * from single_answer_stu where stu_id=@userid";
                sql_bank = "select * from bank_answer_stu where stu_id=@userid";

                mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",userid.Text.Trim())
                };

                ShowSingleAnswer(mySqlParameter );
        
                ShowBankAnswer(mySqlParameter );
            }
            else if (userid.Text.Trim().Equals("") & !subject.Text.Trim().Equals(""))
            {
                sql_single = "select  * from single_answer_stu where subject=@subject";
                sql_bank = "select * from bank_answer_stu where subject=@subject";

                mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@subject",subject.Text.Trim())
                };
                ShowSingleAnswer(mySqlParameter );
                ShowBankAnswer(mySqlParameter );
            }
            else if(!userid.Text.Trim().Equals("") & !subject.Text.Trim().Equals(""))
            {
                sql_single = "select  * from single_answer_stu where subject=@subject and stu_id=@userid";

                sql_bank = "select * from bank_answer_stu where subject=@subject and stu_id=@userid";
                mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@userid",userid.Text.Trim()),
                    new MySqlParameter("@subject",subject.Text.Trim())
                };
                ShowSingleAnswer(mySqlParameter );
                ShowBankAnswer(mySqlParameter );
            }
        }
    }
}
