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

namespace WpfApp1.Blank
{
    /// <summary>
    /// DeleteBlankQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class DeleteBlankQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        string sql;
        string ques_id;
        public DeleteBlankQuestion()
        {
            InitializeComponent();
        }
        public DeleteBlankQuestion(string blank_id)
        {
            InitializeComponent();
            ques_id = blank_id;
            quesid.Text = ques_id;
            GetBlankInfo();
        }
        private void DeleteID_Click(object sender, RoutedEventArgs e)
        {
            ques_id = quesid.Text;
            GetBlankInfo();
        }

        private void GetBlankInfo()
        {
            sql = "select * from blank_question where ques_id=@ques_id";
            mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@ques_id",ques_id),
                };
            DataTable dataTable = db_connect.GetTables(sql, mySqlParameter);
            if (dataTable.Rows.Count != 0)
            {
                blank_name.Text = (string)dataTable.Rows[0]["ques_name"];
                blank_answer.Text = (string)dataTable.Rows[0]["ques_answer"];


            }
            else
            {
                MessageBox.Show("请输入正确的题目编号");
            }
        }

        private void Delete_blank_Click(object sender, RoutedEventArgs e)
        {
            sql = "delete from blank_question where ques_id=@ques_id";
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
