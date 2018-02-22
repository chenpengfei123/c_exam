using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
        public AddSingleQuestion(int subject)
        {

            InitializeComponent();
            this.subject = subject;
        }


        private void addSingle_Click(object sender, RoutedEventArgs e)
        {
            if (single_name.Text.Equals("") | single_A.Equals("") | single_B.Equals("") | single_C.Equals("") | single_D.Equals(""))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }

            String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer,ques_subject) values(@ques_name,@ques_answerA,@ques_answerB,@ques_answerC,@ques_answerD,@ques_answer,@ques_subject)";

          

          

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
                           new MySqlParameter("@ques_subject",subject )
                };
            db_connect.AddNonQuery(sql, mySqlParameter);
        }
    }
}
