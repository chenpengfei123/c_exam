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

namespace WpfApp1.Bank
{
    /// <summary>
    /// DeleteBankQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class DeleteBankQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        string sql;
        string ques_id;
        public DeleteBankQuestion()
        {
            InitializeComponent();
        }

        private void DeleteID_Click(object sender, RoutedEventArgs e)
        {
            ques_id = quesid.Text;
            sql = "select * from bank_question where bank_id=@ques_id";
            mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@ques_id",ques_id),
                };
            DataTable dataTable = db_connect.GetTables(sql,mySqlParameter );
            if (dataTable.Rows.Count != 0)
            {
                bank_name.Text = (string)dataTable.Rows[0]["ques_name"];
                bank_answer.Text = (string)dataTable.Rows[0]["ques_answer"];


            }
            else
            {
                MessageBox.Show("请输入正确的题目编号");
            }
        }

        private void Delete_bank_Click(object sender, RoutedEventArgs e)
        {
            sql = "delete from bank_question where bank_id=@ques_id";
            int i = db_connect.AddNonQuery(sql, mySqlParameter);
            if (i > 0)
            {
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }
    }
}
