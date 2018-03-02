using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// exam.xaml 的交互逻辑
    /// </summary>
    public partial class student_main : Window
    {
        MySqlParameter[] mySqlParameter;
        int subjectID;
     public static string subjectName;
        string sql_subject;
        string sql;
        BaiduAI baiduAI;
        byte[] image;
        DataTable subject_table;
        DataTable score_table;
        public student_main()
        {
            InitializeComponent();
            baiduAI = new BaiduAI();


            ShowScore();

            welecome.Content = "欢迎您，" + BaiduAI.username+"同学";
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            sql = "select stu_image from student where stu_id=@userid";
            mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",BaiduAI.userid)
                };
            image = db_connect.getpictures(sql,mySqlParameter );
            if (image!=null)
            {
                MemoryStream imageStream = new MemoryStream(image);
                BitmapImage bit = new BitmapImage();
                bit.BeginInit();
                bit.StreamSource = imageStream;
                bit.EndInit();
                image1.Source = bit;
            }

            PracticeSubject.ItemsSource = subject_table.DefaultView;
            PracticeSubject.DisplayMemberPath = "subject_name";
            PracticeSubject.SelectedIndex = 0;
            CollectionSubject.ItemsSource=subject_table.DefaultView;
            CollectionSubject.DisplayMemberPath = "subject_name";
            CollectionSubject.SelectedIndex = 0;
          

        }
        private void ShowScore( )
        {
            string sql_scores = "select  * from score where stu_id='" + BaiduAI.userid + "'";
            score_table = db_connect.GetTables(sql_scores);
            score_table.Columns[0].ColumnName = "学号";
            score_table.Columns[1].ColumnName = "姓名";
            score_table.Columns[2].ColumnName = "章节";
            score_table.Columns[3].ColumnName = "选择题得分";
            score_table.Columns[4].ColumnName = "填空题得分";
            score_table.Columns[5].ColumnName = "总分";
            getscores.ItemsSource = score_table.DefaultView;
        }

        private void StartExam_Click(object sender, RoutedEventArgs e)
        {
            if (image!=null)
            {
                int iCurrentIndex = this.PracticeSubject.SelectedIndex;
                if (iCurrentIndex < 0) return;
                DataRow dr = subject_table.Rows[iCurrentIndex];
                subjectID = int.Parse(dr[0].ToString());
                subjectName= dr["subject_name"].ToString();
                Single startanswer = new Single(subjectID);
            startanswer.Owner = this;
            startanswer.ShowDialog();
            //this.Close();

            }
            else
            {
                System.Windows.MessageBox.Show("请先上传自己本人的照片才可进行考试");
            }
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

   

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认退出系统?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                //Login_normal login_Normal = new Login_normal();
                //login_Normal.Show();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void selectpicture(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "jpg|*.jpg|jpeg|*.png";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string fileName = openFileDialog.FileName;
       
            byte[] face = File.ReadAllBytes(fileName);
            if (face.Length> 102400)
            {
                System.Windows.MessageBox.Show("上传的图片大小不能大于100KB");
            }
            else
            {

            string isface = baiduAI.face_identify(face);
            if (isface.Equals("识别不出你是谁")||isface.Equals(BaiduAI.userid))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(fileName);
                bi.EndInit();
                image1.Source = bi;
               
                string result1 = baiduAI.face_useradd(BaiduAI.userid, BaiduAI.username, face);
                String sql = "update student set stu_image=@filecontent where stu_id=@userid";
                    mySqlParameter = new MySqlParameter[] {
                         new MySqlParameter("@filecontent",face),
                    new MySqlParameter("@userid",BaiduAI.userid)
                };
                   
                    db_connect.AddNonQuery(sql,mySqlParameter );
           
            }
            else if (isface.Equals("未识别到人脸"))
            {
                System.Windows.MessageBox.Show("未识别到人脸");
            }
            else
            {
                System.Windows.MessageBox.Show("你的人脸已被注册，请联系老师");
            }
            }
        }

     

        private void CollectionPractice_Click(object sender, RoutedEventArgs e)
        {
            int iCurrentIndex = this.CollectionSubject.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr = subject_table.Rows[iCurrentIndex];
            subjectID = int.Parse(dr[0].ToString());
            subjectName = dr["subject_name"].ToString();

            String sql_single = "Select * from single_question Inner join single_collection on single_collection.stu_id= '" + BaiduAI.userid+ "' and single_collection.ques_id=single_question.ques_id and single_question.ques_subject= "+subjectID;
            String sql_bank = "Select * from bank_question Inner join bank_collection on bank_collection.stu_id= '" + BaiduAI.userid + "' and bank_collection.ques_id=bank_question.bank_id and bank_question.ques_subject= " + subjectID;
            Practice practice = new Practice(subjectID, sql_single, sql_bank);
            practice.Show();
        }

        private void StartPractice_Click(object sender, RoutedEventArgs e)
        {
          
            int iCurrentIndex = this.PracticeSubject.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr = subject_table.Rows[iCurrentIndex];
            subjectID = int.Parse(dr[0].ToString());
            subjectName = dr["subject_name"].ToString();

            String sql_single = "Select * from single_question where ques_subject= " + subjectID;
            String sql_bank = "Select * from bank_question  where ques_subject= " + subjectID;
            Practice practice = new Practice(subjectID, sql_single, sql_bank);
            practice.Show();

        }
    }
}
