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
    /// ChangeBankQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeBankQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        string sql;
        string ques_id;
        public ChangeBankQuestion()
        {
            InitializeComponent();
        }

        private void ChangeID_Click(object sender, RoutedEventArgs e)
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
                Explain.Text = !dataTable.Rows[0].IsNull("ques_explain") ? dataTable.Rows[0]["ques_explain"].ToString() : "暂无解析";

            }
            else
            {
                MessageBox.Show("请输入正确的题目编号");
            }
        }

        private void Change_bank_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(bank_name.Text) | String.IsNullOrEmpty(bank_answer.Text) )
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }
            else
            {
                 sql = "update bank_question set  ques_name=@ques_name,ques_answer=@ques_answer,ques_explain=@ques_explain where bank_id=@ques_id";
                mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("ques_name",bank_name.Text),
                    new MySqlParameter("ques_answer",bank_answer.Text ),
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
