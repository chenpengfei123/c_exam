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
            //byte[] face = File.ReadAllBytes("D:/weizhong.jpg");
            BaiduAI baiduAi = new BaiduAI();
            string isface = baiduAi.face_identify(face);
            if (isface.Equals("识别不出你是谁"))
            {
            string result = baiduAi.face_useradd(uid, user_name, face);
           
                String sql = "replace into student(stu_name,stu_image) values('" + uid + "'," +"@filecontent)";
           
            MySqlConnection mycon = db_connect.Mysql_con();
            mycon.Open();
            MySqlCommand mycmd = new MySqlCommand(sql, mycon);
        
            mycmd.Parameters.Add("@filecontent", MySql.Data.MySqlClient.MySqlDbType.Blob);
            mycmd.Parameters[0].Value = face;
            mycmd.ExecuteNonQuery();
            mycon.Close();
                MessageBox.Show(result);
            }
           else
            {
                MessageBox.Show("你已注册，请直接登录");
            }
        }

        private void Windows_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CameraHelper.CloseDevice();
        }

    }
}
