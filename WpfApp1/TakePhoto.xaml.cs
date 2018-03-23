using MySql.Data.MySqlClient;
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

namespace WpfApp1
{
    /// <summary>
    /// TakePhoto.xaml 的交互逻辑
    /// </summary>
    public partial class TakePhoto : Window
    {
        MySqlParameter[] mySqlParameter;
      
        BaiduAI baiduAi;
        Image image;
        public TakePhoto(Image image)
        {
            InitializeComponent();
            CameraHelper.CameraInit(player);
            this.image = image;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] face = CameraHelper.CaptureImage();
            baiduAi = new BaiduAI();
            string isface = baiduAi.face_identify(face);
            if (isface.Equals("unknown_face") || isface.Equals(BaiduAI.userid))
            {
                MemoryStream imageStream = new MemoryStream(face);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = imageStream;
                bi.EndInit();
                image.Source = bi;

                baiduAi.face_useradd(BaiduAI.userid, BaiduAI.username, face);
                String sql = "update student set stu_image=@filecontent where stu_id=@userid";
                mySqlParameter = new MySqlParameter[] {
                         new MySqlParameter("@filecontent",face),
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Windows_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CameraHelper.CloseDevice();
        }
    }
}
