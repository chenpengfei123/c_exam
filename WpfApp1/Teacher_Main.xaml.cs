using MySql.Data.MySqlClient;
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
using WpfApp1.Bank;

namespace WpfApp1
{
    /// <summary>
    /// Teacher_Main.xaml 的交互逻辑
    /// </summary>
    public partial class Teacher_Main : Window
    {
        int subject;
        MySqlParameter[] mySqlParameter;
        string sql_single;
        string sql_bank;
        DataTable dataTable;
        byte[] image;
        string sql_student;
        string sql_score;
        string sql_subject;
        BaiduAI baiduAI;
        DataTable student_table;
        DataTable score_table;
        DataTable subject_table;
        public Teacher_Main()
        {
            InitializeComponent();
            baiduAI = new BaiduAI();
            sql_student = "select stu_id,stu_name from student";
            ShowStudent();
            sql_subject = "select  * from subject";
            subject_table= db_connect.GetTables(sql_subject);
        

            sql_score = "select * from score";
            ShowScore();
            welecome.Content = "欢迎您，" + BaiduAI.username + "老师";
            myComboxBox.ItemsSource = subject_table.DefaultView;
            myComboxBox.DisplayMemberPath = "subject_name";
            myComboxBox.SelectedIndex = 0;
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

        private void ShowPicture_Click(object sender, RoutedEventArgs e)
        {
            if (user_picture.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("请输入学号");
            }
            else
            {
                string sql = "select stu_image from student where stu_id=@userid";

                mySqlParameter = new MySqlParameter[] {
         
                    new MySqlParameter("@userid",user_picture.Text)
                };
                image = db_connect.getpictures(sql,mySqlParameter );
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
                Login_normal login_Normal = new Login_normal();
                login_Normal.Show();
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

        private void myComboxBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int iCurrentIndex = this.myComboxBox.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr = subject_table.Rows[iCurrentIndex];
            subject = int.Parse(dr[0].ToString());
            sql_single = "select  ques_id,ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer from single_question where ques_subject="+ subject;
            ShowSingleQuestion();

            sql_bank = "select bank_id,ques_name,ques_answer from bank_question where ques_subject=" + subject;
            ShowBankQuestion();
        }
        private void ShowSingleQuestion()
        {
            dataTable = db_connect.GetTables(sql_single);
            dataTable.Columns[0].ColumnName = "题目编号";
            dataTable.Columns[1].ColumnName = "题目名称";
            dataTable.Columns[2].ColumnName = "选项A";
            dataTable.Columns[3].ColumnName = "选项B";
            dataTable.Columns[4].ColumnName = "选项C";
            dataTable.Columns[5].ColumnName = "选项D";
            dataTable.Columns[6].ColumnName = "正确选项";
            single_manager.ItemsSource = dataTable.DefaultView;
        }

        private void ShowBankQuestion()
        {
            dataTable = db_connect.GetTables(sql_bank);
            dataTable.Columns[0].ColumnName = "题目编号";
            dataTable.Columns[1].ColumnName = "题目名称";
            dataTable.Columns[2].ColumnName = "正确答案";
            bank_manager.ItemsSource = dataTable.DefaultView;
        }

        private void AddSingle_Click(object sender, RoutedEventArgs e)
        {
           
            AddSingleQuestion addSingleQuestion = new AddSingleQuestion(subject);
            addSingleQuestion.Owner = this;
            addSingleQuestion.Show();
        }

        private void RefreshSingle_Click(object sender, RoutedEventArgs e)
        {
            ShowSingleQuestion();
        }

        private void ChangeSingle_Click(object sender, RoutedEventArgs e)
        {
            ChangeSingleQuestion changeSingleQuestion = new ChangeSingleQuestion();
            changeSingleQuestion.Owner = this;
            changeSingleQuestion.Show();


        }

        private void DeleteSingle_Click(object sender, RoutedEventArgs e)
        {
            DeleteSingleQuestion deleteSingleQuestion = new DeleteSingleQuestion();
            deleteSingleQuestion.Owner = this;
            deleteSingleQuestion.Show();
        }

        private void AddBank_Click(object sender, RoutedEventArgs e)
        {

            AddBankQuestion addBankQuestion = new AddBankQuestion(subject);
            addBankQuestion.Owner = this;
            addBankQuestion.Show();
        }

        private void ChangeBank_Click(object sender, RoutedEventArgs e)
        {
            ChangeBankQuestion changeBankQuestion = new ChangeBankQuestion();
            changeBankQuestion.Owner = this;
            changeBankQuestion.Show();
        }

        private void DeleteBank_Click(object sender, RoutedEventArgs e)
        {
            DeleteBankQuestion deleteBankQuestion = new DeleteBankQuestion();
            deleteBankQuestion.Owner = this;
            deleteBankQuestion.Show();
        }

        private void RefreshBank_Click(object sender, RoutedEventArgs e)
        {
            ShowBankQuestion();
        }
    }
    }

