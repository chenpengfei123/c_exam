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
    /// AddSingleQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class AddSingleQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        int subject;
        string answer;
        string sql_subject;
        DataTable subject_table;
        public AddSingleQuestion(int subject)
        {

            InitializeComponent();
            this.subject = subject;
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            subject_table.PrimaryKey = new DataColumn[] { subject_table.Columns["subject_id"] };
            QuestionSubject.ItemsSource = subject_table.DefaultView;
            QuestionSubject.DisplayMemberPath = "subject_name";
            QuestionSubject.SelectedIndex = subject_table.Rows.IndexOf(subject_table.Rows.Find(subject));
        }


        private void addSingle_Click(object sender, RoutedEventArgs e)
        {
            subject = (int)subject_table.Rows[QuestionSubject.SelectedIndex]["subject_id"];
            if (String.IsNullOrEmpty(single_name.Text)  | String.IsNullOrEmpty(single_A.Text) | String.IsNullOrEmpty(single_B.Text) | String.IsNullOrEmpty(single_C.Text) | String.IsNullOrEmpty(single_D.Text))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }

            String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer,ques_subject,ques_explain) values(@ques_name,@ques_answerA,@ques_answerB,@ques_answerC,@ques_answerD,@ques_answer,@ques_subject,@ques_explain)";

          

          

            if ((bool)answer_A.IsChecked)
            {
                answer = "A";
                AddSingle(sql);
            }
            else if ((bool)answer_B.IsChecked)
            {

                answer = "B";
                AddSingle(sql);
            }
            else if ((bool)answer_C.IsChecked)
            {
                answer = "C";
                AddSingle(sql);
            }
            else if ((bool)answer_D.IsChecked)
            {
                answer = "D";
                AddSingle(sql);
            }
            else
            {
                System.Windows.MessageBox.Show("请选择正确答案");
                return;
            }
        }

        private void AddSingle(string sql)
        {
            mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@ques_name",single_name.Text),
                    new MySqlParameter("@ques_answerA",single_A.Text ),
                     new MySqlParameter("@ques_answerB",single_B.Text ),
                      new MySqlParameter("@ques_answerC",single_C.Text ),
                       new MySqlParameter("@ques_answerD",single_D.Text ),
                       new MySqlParameter("@ques_answer",answer ),
                           new MySqlParameter("@ques_subject",subject ),
                           new MySqlParameter("@ques_explain",Explain.Text )
                };
            int i = db_connect.AddNonQuery(sql, mySqlParameter);
            if (i > 0)
            {
                MessageBox.Show("添加成功");
            }
            else
            {
                MessageBox.Show("添加失败");
            }
      
        }
    }
}
