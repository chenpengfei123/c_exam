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
        int subject;
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
            if ((bool)answer_A.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer,ques_subject) values('" + single_name.Text + "','"  + single_A.Text + "','"  + single_B.Text + "','"  + single_C.Text + "','"  + single_D.Text + "', 'A',"+subject+")";
                db_connect.AddNonQuery(sql);
            }
            else if ((bool)answer_B.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer,ques_subject) values('" + single_name.Text + "','" + single_A.Text + "','" + single_B.Text + "','" + single_C.Text + "','" + single_D.Text + "', 'B'," + subject + ")";
                db_connect.AddNonQuery(sql);
            }
            else if ((bool)answer_C.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer,ques_subject) values('" + single_name.Text + "','" + single_A.Text + "','" + single_B.Text + "','" + single_C.Text + "','" + single_D.Text + "', 'C'," + subject + ")";
                db_connect.AddNonQuery(sql); ;
            }
            else if ((bool)answer_D.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer,ques_subject) values('" + single_name.Text + "','" + single_A.Text + "','" + single_B.Text + "','" + single_C.Text + "','" + single_D.Text + "', 'D'," + subject + ")";
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
