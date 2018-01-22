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
using Baidu.Aip.Face;

namespace WpfApp1
{
    /// <summary>
    /// Login_face.xaml 的交互逻辑
    /// </summary>
    public partial class Login_face : Window
    {
     
        public Login_face()
        {

            InitializeComponent();
            CameraHelper.IsDisplay = true;
            CameraHelper.SourcePlayer = player;
            CameraHelper.UpdateCameraDevices();
            if (CameraHelper.CameraDevices.Count > 0)
            {
                CameraHelper.SetCameraDevice(0);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] face = CameraHelper.CaptureImage();
            BaiduAI baiduAI = new BaiduAI();
          string logininfo=  baiduAI.face_identify(face);

            if (logininfo.Equals("登录失败"))
            {
                MessageBox.Show("对不起，识别不出你的脸");
                return;
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(logininfo, "确认登录信息", MessageBoxButton.YesNo, MessageBoxImage.Question);

                //关闭窗口
                if (result == MessageBoxResult.Yes)
                {
                    exam exam = new exam();
                    exam.Show();
                    this.Close();

                }
                if (result == MessageBoxResult.No)
                {

                }
            }
               


            
           
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {


            CameraHelper.CloseDevice();
        }
    }
    }
