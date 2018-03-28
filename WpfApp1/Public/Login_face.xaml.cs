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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Baidu.Aip.Face;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Login_face.xaml 的交互逻辑
    /// </summary>
    public partial class Login_face : Window
    {

        BaiduAI baiduAI;
        public  byte[] face;
        public Login_face()
        {

            InitializeComponent();
            CameraHelper.CameraInit(player);
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
  
                 face = CameraHelper.CaptureImage();
                 baiduAI = new BaiduAI();
          string logininfo=  baiduAI.face_identify(face);

            if (logininfo.Equals("success"))
            {
    

                string s = "学号：" + BaiduAI.userid + "\n 姓名：" + BaiduAI.username;
                MessageBoxResult result = System.Windows.MessageBox.Show(s, "确认登录信息", MessageBoxButton.YesNo, MessageBoxImage.Question);

                //关闭窗口
                if (result == MessageBoxResult.Yes)
                {
                    student_main student_main = new student_main();
                    student_main.Show();
                    this.Close();

                }
            }
            else if (logininfo.Equals("unknown_face"))
            {
                MessageBox.Show("你的人脸还未注册");
            }
            else if (logininfo.Equals("no_face"))
            {
                MessageBox.Show("未检测到人脸，请重试");
            }
            else
            {
                MessageBox.Show("人脸识别失败");
            }




        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {


            CameraHelper.CloseDevice();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (CameraHelper.getcount())
            {
                CameraHelper.CloseDevice();
                Register_face register_Face = new Register_face();
                register_Face.Owner = this;
                register_Face.ShowDialog();
                CameraHelper.CameraInit(player);
            }
           
        }

        private void login_normal(object sender, RoutedEventArgs e)
        {
            Login_normal loginWindow = new Login_normal();
            loginWindow.Show();
            Close();
        }
    }
    }
