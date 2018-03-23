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
        string sql;
        BaiduAI baiduAI;
      public  static   byte[] image;
        DataTable score_table;
        public student_main()
        {
            InitializeComponent();
            baiduAI = new BaiduAI();
            

            ShowScore();

            welecome.Content = "欢迎您，" + BaiduAI.username+"同学";
   
            sql = "select stu_image from student where stu_id='"+BaiduAI.userid+"'";
        
            image = db_connect.getpictures(sql);
            if (image!=null)
            {
                MemoryStream imageStream = new MemoryStream(image);
                BitmapImage bit = new BitmapImage();
                bit.BeginInit();
                bit.StreamSource = imageStream;
                bit.EndInit();
                image1.Source = bit;
            }

          
          



        }
        private void ShowScore( )
        {
            string sql_scores = "select stu_id,stu_name,exam_name,score_single,score_bank,exam_score.score, start_time,end_time from exam_score,exam where stu_id='" + BaiduAI.userid + "' and exam_score.exam_id=exam.exam_id";
            score_table = db_connect.GetTables(sql_scores);
            score_table.Columns[0].ColumnName = "学号";
            score_table.Columns[1].ColumnName = "姓名";
            score_table.Columns[2].ColumnName = "试卷名称";
            score_table.Columns[3].ColumnName = "选择题得分";
            score_table.Columns[4].ColumnName = "填空题得分";
            score_table.Columns[5].ColumnName = "总分";
            score_table.Columns[6].ColumnName = "开始考试时间";
            score_table.Columns[7].ColumnName = "结束考试时间";
            getscores.ItemsSource = score_table.DefaultView;
        }

     
     
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
          
            Login_normal login_Normal = new Login_normal();
            login_Normal.Show();
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

            BaiduAI.userid = "";
            BaiduAI.username = "";
            e.Cancel = false;
     
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

            image = File.ReadAllBytes(fileName);
            if (image.Length> 102400)
            {
                System.Windows.MessageBox.Show("上传的图片大小不能大于100KB");
            }
            else
            {

            string isface = baiduAI.face_identify(image);
            if (isface.Equals("unknown_face") ||isface.Equals(BaiduAI.userid))
                {
                    MemoryStream imageStream = new MemoryStream(image);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = imageStream;
                    bi.EndInit();
                    image1.Source = bi;
               
                baiduAI.face_useradd(BaiduAI.userid, BaiduAI.username, image);
                String sql = "update student set stu_image=@filecontent where stu_id=@userid";
                    mySqlParameter = new MySqlParameter[] {
                         new MySqlParameter("@filecontent",image),
                    new MySqlParameter("@userid",BaiduAI.userid)
                };
                   
                  
                    int i = db_connect.AddNonQuery(sql, mySqlParameter);
                    if (i > 0)
                    {
                        System.Windows.MessageBox.Show("注册成功");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("注册失败");
                    }
                }
            else if (isface.Equals("no_face"))
            {
                System.Windows.MessageBox.Show("未识别到人脸");
            }
                else if (isface.Equals("success"))
                {
                System.Windows.MessageBox.Show("你的人脸已被注册");
            }
                else
                {
                    System.Windows.MessageBox.Show("人脸识别失败");
                }

            }
        }

     

        

        private void RefreshScore_Click(object sender, RoutedEventArgs e)
        {
            ShowScore();
        }

        private void TakePhoto_Click(object sender, MouseButtonEventArgs e)
        {
            TakePhoto takephoto = new TakePhoto(image1);
            takephoto.Owner = this;
            takephoto.ShowDialog();

        }
    }
}
