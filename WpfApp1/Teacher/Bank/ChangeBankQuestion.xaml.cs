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
    /// ChangeBlankQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeBlankQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        string sql;
        string ques_id;
        public ChangeBlankQuestion()
        {
            InitializeComponent();
        }
        public ChangeBlankQuestion(String blank_id)
        {
            InitializeComponent();
            ques_id = blank_id;
           quesid.Text =ques_id  ;
            GetBlankInfo();
        }
        private void ChangeID_Click(object sender, RoutedEventArgs e)
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
                Explain.Text = !dataTable.Rows[0].IsNull("ques_explain") ? dataTable.Rows[0]["ques_explain"].ToString() : "暂无解析";

            }
            else
            {
                MessageBox.Show("请输入正确的题目编号");
            }
        }

        private void Change_blank_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(blank_name.Text) | String.IsNullOrEmpty(blank_answer.Text) )
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }
            else
            {
                 sql = "update blank_question set  ques_name=@ques_name,ques_answer=@ques_answer,ques_explain=@ques_explain where ques_id=@ques_id";
                mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("ques_name",blank_name.Text),
                    new MySqlParameter("ques_answer",blank_answer.Text ),
                           new MySqlParameter("@ques_id",ques_id ),
                              new MySqlParameter("@ques_explain",Explain.Text )
                };
                int i = db_connect.AddNonQuery(sql, mySqlParameter);
                if (i > 0)
                {
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show("修改失败");
                }
            }
        }
    }
}
