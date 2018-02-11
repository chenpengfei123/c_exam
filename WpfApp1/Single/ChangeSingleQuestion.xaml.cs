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
    /// ChangeSingleQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeSingleQuestion : Window
    {
        string sql;
        string ques_id;
        public ChangeSingleQuestion()
        {
            InitializeComponent();
        }

        private void ChangeID_Click(object sender, RoutedEventArgs e)
        {
             ques_id = quesid.Text;
            sql = "select * from single_question where ques_id=" + ques_id;
          DataTable dataTable=  db_connect.GetTables(sql);
            if (dataTable.Rows.Count!=0)
            {
                single_name.Text = (string)dataTable.Rows[0]["ques_name"];
                single_A.Text = (string)dataTable.Rows[0]["ques_answerA"];
                single_B.Text = (string)dataTable.Rows[0]["ques_answerB"];
                single_C.Text = (string)dataTable.Rows[0]["ques_answerC"];
                single_D.Text = (string)dataTable.Rows[0]["ques_answerD"];
                switch ((string )dataTable.Rows[0]["ques_answer"])
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

        private void ChangeSingle_Click(object sender, RoutedEventArgs e)
        {
            if (single_name.Text.Equals("") | single_A.Equals("") | single_B.Equals("") | single_C.Equals("") | single_D.Equals(""))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }
            if ((bool)answer_A.IsChecked)
            {
                String sql = "update single_question set  ques_name='" + single_name.Text + "',ques_answerA='" + single_A.Text + "',ques_answerB='" + single_B.Text + "',ques_answerC='" + single_C.Text + "',ques_answerD='" + single_D.Text + "',ques_answer='A'where ques_id=" + ques_id;
                
                db_connect.AddNonQuery(sql);
            }
            else if ((bool)answer_B.IsChecked)
            {
                String sql = "update single_question set  ques_name='" + single_name.Text + "',ques_answerA='" + single_A.Text + "',ques_answerB='" + single_B.Text + "',ques_answerC='" + single_C.Text + "',ques_answerD='" + single_D.Text + "',ques_answer='B'where ques_id=" + ques_id;
                db_connect.AddNonQuery(sql);
            }
            else if ((bool)answer_C.IsChecked)
            {
                String sql = "update single_question set  ques_name='" + single_name.Text + "',ques_answerA='" + single_A.Text + "',ques_answerB='" + single_B.Text + "',ques_answerC='" + single_C.Text + "',ques_answerD='" + single_D.Text + "',ques_answer='C'where ques_id=" + ques_id;
                db_connect.AddNonQuery(sql); ;
            }
            else if ((bool)answer_D.IsChecked)
            {
                String sql = "update single_question set  ques_name='" + single_name.Text + "',ques_answerA='" + single_A.Text + "',ques_answerB='" + single_B.Text + "',ques_answerC='" + single_C.Text + "',ques_answerD='" + single_D.Text + "',ques_answer='D'where ques_id=" + ques_id;
                db_connect.AddNonQuery(sql);
            }
            else
            {
                System.Windows.MessageBox.Show("请选择正确答案");
                return;
            }
        }
    }
}
