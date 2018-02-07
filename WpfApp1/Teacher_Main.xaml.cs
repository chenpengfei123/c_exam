using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// Teacher_Main.xaml 的交互逻辑
    /// </summary>
    public partial class Teacher_Main : Window
    {
        string sql;
        BaiduAI baiduAI;
        public Teacher_Main()
        {
            InitializeComponent();
            baiduAI = new BaiduAI();
            stu_manager.ItemsSource = baiduAI.face_getuser().DefaultView;
            sql = "select * from score";
            getscores.ItemsSource = db_connect.GetTables(sql).DefaultView;
            welecome.Content = "欢迎您，" + BaiduAI.username+"老师";

         
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
                db_connect.AddNonQuery(sql);
            }
            else if ((bool)answer_B.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'B')";
                db_connect.AddNonQuery(sql);
            }
            else if ((bool)answer_C.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'C')";
                db_connect.AddNonQuery(sql); ;
            }
            else if ((bool)answer_D.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'D')";
                db_connect.AddNonQuery(sql);
            }
            else
            {
                System.Windows.MessageBox.Show("请选择正确答案");
                return;
            }
        }


        private void add_bank_Click(object sender, RoutedEventArgs e)
        {
            if (bank_name.Text.Equals("") | bank_answer.Equals(""))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }
            String sql = "insert into bank_question(ques_name,ques_answer) values('" + bank_name.Text + "','" + bank_answer.Text + "')";
            db_connect.AddNonQuery(sql);
        }



        private void StartAnswer_Click(object sender, RoutedEventArgs e)
        {
            Question_Manager question_Manager = new Question_Manager();
            question_Manager.Owner = this;
            question_Manager.Show();
          
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.ShowDialog();
        }



        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            stu_manager.ItemsSource = baiduAI.face_getuser().DefaultView;
        }

        private void stu_delete(object sender, RoutedEventArgs e)
        {
            baiduAI.face_deleteuser(user_delete.Text);
            System.Windows.Forms.MessageBox.Show("删除成功");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认退出系统?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
