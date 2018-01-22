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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// exam.xaml 的交互逻辑
    /// </summary>
    public partial class exam : Window
    {
        public exam()
        {
            InitializeComponent();
            double screen = SystemParameters.FullPrimaryScreenHeight;
            double width = SystemParameters.FullPrimaryScreenWidth;
            welecome.Content = "欢迎您，" + BaiduAI.username;
            
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
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'A')";
                db_connect.addQuestion(sql);
            }
            else if ((bool)answer_B.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'B')";
                db_connect.addQuestion(sql);
            }
            else if ((bool)answer_C.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'C')";
                db_connect.addQuestion(sql); ;
            }
            else if ((bool)answer_D.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'D')";
                db_connect.addQuestion(sql);
            }
            else
            {
                System.Windows.MessageBox.Show("请选择正确答案");
                return;
            }
        }
  
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void add_bank_Click(object sender, RoutedEventArgs e)
        {
            if (bank_name.Text.Equals("") | bank_answer.Equals(""))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }
            String sql = "insert into bank_question(ques_name,ques_answer) values('" + bank_name.Text + "','" + bank_answer.Text + "')";
            db_connect.addQuestion(sql);
        }

    

        private void StartAnswer_Click(object sender, RoutedEventArgs e)
        {
            Single startanswer = new Single();
            startanswer.Owner = this;
            startanswer.ShowDialog();
            //this.Close();
        }

        private void answer_A_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("退出系统?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            { this.Close(); }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BaiduAI baiduAI = new BaiduAI();
            System.Windows.MessageBox.Show(baiduAI.face_deleteuser("user1")) ;
            //System.Windows.MessageBox.Show(baiduAI.face_getuser());
        }
    }
}
