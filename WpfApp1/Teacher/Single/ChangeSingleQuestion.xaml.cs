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
    /// ChangeSingleQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeSingleQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        string sql;
        string answer;
        string ques_id;
        string sql_subject;
        DataTable subject_table;
        public ChangeSingleQuestion( string singleId)
        {
            InitializeComponent();
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            subject_table.PrimaryKey = new DataColumn[] {subject_table.Columns["subject_id"] };
            QuestionSubject.ItemsSource = subject_table.DefaultView;
            QuestionSubject.DisplayMemberPath = "subject_name";
            ques_id = singleId;
            GetSingleInfo();
            quesid.Text = ques_id;
        }
        public ChangeSingleQuestion()
        {
            InitializeComponent();
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            subject_table.PrimaryKey = new DataColumn[] { subject_table.Columns["subject_id"] };
            QuestionSubject.ItemsSource = subject_table.DefaultView;
            QuestionSubject.DisplayMemberPath = "subject_name";
 
        }




        private void ChangeID_Click(object sender, RoutedEventArgs e)
        {
            ques_id = quesid.Text;
            GetSingleInfo();
        }

        private void GetSingleInfo()
        {
            sql = "select * from single_question where ques_id=@quesid";
            mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@quesid",ques_id),

                };

            DataTable dataTable = db_connect.GetTables(sql, mySqlParameter);
            if (dataTable.Rows.Count != 0)
            {

                QuestionSubject.SelectedIndex = subject_table.Rows.IndexOf(subject_table.Rows.Find(dataTable.Rows[0]["ques_subject"]));

                single_name.Text = (string)dataTable.Rows[0]["ques_name"];
                single_A.Text = (string)dataTable.Rows[0]["ques_answerA"];
                single_B.Text = (string)dataTable.Rows[0]["ques_answerB"];
                single_C.Text = (string)dataTable.Rows[0]["ques_answerC"];
                single_D.Text = (string)dataTable.Rows[0]["ques_answerD"];
                Explain.Text = !dataTable.Rows[0].IsNull("ques_explain") ? dataTable.Rows[0]["ques_explain"].ToString() : "暂无解析";

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

        private void ChangeSingle_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(single_name.Text) | String.IsNullOrEmpty(single_A.Text) | String.IsNullOrEmpty(single_B.Text) | String.IsNullOrEmpty(single_C.Text) | String.IsNullOrEmpty(single_D.Text))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }

            String sql = "update single_question set  ques_name=@ques_name,ques_answerA=@ques_answerA,ques_answerB=@ques_answerB,ques_answerC = @ques_answerC,ques_answerD = @ques_answerD,ques_answer = @ques_answer,ques_subject=@ques_subject,ques_explain=@ques_explain where ques_id =@ques_id";

         
            if ((bool)answer_A.IsChecked)
            {


                answer = "A";
                ChangeSingle(sql);
            }
            else if ((bool)answer_B.IsChecked)
            {

                answer = "B";
                ChangeSingle(sql);
            }
            else if ((bool)answer_C.IsChecked)
            {

                answer = "C";
                ChangeSingle(sql);
            }
            else if ((bool)answer_D.IsChecked)
            {

                answer = "D";
                ChangeSingle(sql);
            }
            else
            {
                System.Windows.MessageBox.Show("请选择正确答案");
                return;
            }
        }

        private void ChangeSingle(string sql)
        {
            mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@ques_name",single_name.Text),
                    new MySqlParameter("@ques_answerA",single_A.Text ),
                     new MySqlParameter("@ques_answerB",single_B.Text ),
                      new MySqlParameter("@ques_answerC",single_C.Text ),
                       new MySqlParameter("@ques_answerD",single_D.Text ),
                       new MySqlParameter("@ques_answer",answer ),
                           new MySqlParameter("@ques_id",ques_id ),
                                new MySqlParameter("@ques_subject",subject_table.Rows[QuestionSubject.SelectedIndex]["subject_id"] ),
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
