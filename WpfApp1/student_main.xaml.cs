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
        string sql;
        BaiduAI baiduAI;
        public student_main()
        {
            InitializeComponent();
            baiduAI = new BaiduAI();
          
            double screen = SystemParameters.FullPrimaryScreenHeight;
            double width = SystemParameters.FullPrimaryScreenWidth;
            welecome.Content = "欢迎您，" + BaiduAI.username+"同学";

            sql = "select stu_image from student where stu_name='"+BaiduAI.userid+"'";
                  byte[] image = db_connect.getpictures(sql);
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


        private void StartAnswer_Click(object sender, RoutedEventArgs e)
        {
            Single startanswer = new Single();
            startanswer.Owner = this;
            startanswer.ShowDialog();
            //this.Close();
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
