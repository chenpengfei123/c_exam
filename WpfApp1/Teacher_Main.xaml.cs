using System;
using System.Collections.Generic;
using System.Data;
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
        byte[] image;
        string sql_student;
        string sql_score;
        BaiduAI baiduAI;
        DataTable student_table;
        DataTable score_table;
        public Teacher_Main()
        {
            InitializeComponent();
            baiduAI = new BaiduAI();
            sql_student = "select stu_id,stu_name from student";
            ShowStudent();

            sql_score = "select * from score";
            ShowScore();
            welecome.Content = "欢迎您，" + BaiduAI.username + "老师";


        }

        private void ShowScore()
        {
            score_table = db_connect.GetTables(sql_score);
            score_table.Columns[0].ColumnName = "学号";
            score_table.Columns[1].ColumnName = "姓名";
            score_table.Columns[2].ColumnName = "章节";
            score_table.Columns[3].ColumnName = "选择题得分";
            score_table.Columns[4].ColumnName = "填空题得分";
            score_table.Columns[5].ColumnName = "总分";
            getscores.ItemsSource = score_table.DefaultView;
        }

        private void ShowStudent()
        {
            student_table = db_connect.GetTables(sql_student);
            student_table.Columns[0].ColumnName = "学号";
            student_table.Columns[1].ColumnName = "姓名";       
            stu_manager.ItemsSource = student_table.DefaultView;
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
            changePassword.Owner = this;
            changePassword.ShowDialog();
        }



        private void RefreshStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowStudent();
        }
        private void RefreshScore_Click(object sender, RoutedEventArgs e)
        {
            ShowScore();
        }

        private void stu_delete(object sender, RoutedEventArgs e)
        {
            if (user_picture.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("请输入学号");
            }
            else
            {
            string sql = "select stu_image from student where stu_id=" + user_picture.Text;
                image = db_connect.getpictures(sql);
                if (image != null)
                {
                    MemoryStream imageStream = new MemoryStream(image);
                    BitmapImage bit = new BitmapImage();
                    bit.BeginInit();
                    bit.StreamSource = imageStream;
                    bit.EndInit();
                    image2.Source = bit;
                }

            }
            
        }

        private void Window_Closing(object sender,System.ComponentModel.CancelEventArgs e)
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

        private void AnswerManager_Click(object sender, RoutedEventArgs e)
        {
            Answer_Manager answer = new Answer_Manager();
            answer.Owner = this;
            answer.ShowDialog();
        }
    }
}
