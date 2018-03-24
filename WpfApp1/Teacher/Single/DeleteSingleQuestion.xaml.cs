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
    /// DeleteSingleQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class DeleteSingleQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        string sql;
        string ques_id;
        public DeleteSingleQuestion()
        {
            InitializeComponent();
        }

        private void DeleteID_Click(object sender, RoutedEventArgs e)
        {
            ques_id = quesid.Text;
            sql = "select * from single_question where ques_id=@quesid";
            mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@quesid",ques_id),

                };

            DataTable dataTable = db_connect.GetTables(sql,mySqlParameter );
            if (dataTable.Rows.Count != 0)
            {
                single_name.Text = (string)dataTable.Rows[0]["ques_name"];
                single_A.Text = (string)dataTable.Rows[0]["ques_answerA"];
                single_B.Text = (string)dataTable.Rows[0]["ques_answerB"];
                single_C.Text = (string)dataTable.Rows[0]["ques_answerC"];
                single_D.Text = (string)dataTable.Rows[0]["ques_answerD"];
                switch ((string)dataTable.Rows[0]["ques_answer"])
                {
                    case "A":
                        answer_A.IsChecked = true;
                        break;
                    case "B":
                        answer_B.IsChecked = true;
                        break;
                    case "C":
                        answer_C.IsChecked = true;
                        break;
                    case "D":
                        answer_D.IsChecked = true;
                        break;
                }
            }
            else
            {
                MessageBox.Show("请输入正确的题目编号");
            }
        }

        private void DeleteSingle_Click(object sender, RoutedEventArgs e)
        {
            sql = "delete from single_question where ques_id=@quesid";
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
