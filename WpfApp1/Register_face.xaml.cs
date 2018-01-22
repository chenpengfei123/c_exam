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
    /// Register_face.xaml 的交互逻辑
    /// </summary>
    public partial class Register_face : Window
    {
       
       
        public Register_face()
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
            string uid = userid.Text;
            string user_name = username.Text;
            byte[] face = CameraHelper.CaptureImage();
            BaiduAI baiduAi = new BaiduAI();
            string result = baiduAi.face_useradd(uid, user_name, face);
            MessageBox.Show(result);
        }

        private void Windows_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CameraHelper.CloseDevice();
        }
    }
}
