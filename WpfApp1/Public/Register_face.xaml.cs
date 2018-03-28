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
        MySqlParameter[] mySqlParameter;
        string sql;
        BaiduAI baiduAi;
        public Register_face()
        {
            InitializeComponent();
            if (!CameraHelper.CameraInit(player))
            {
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string user_id = userid.Text;
            string user_name = username.Text;
            if (String.IsNullOrEmpty(user_id) | String.IsNullOrEmpty(user_name))
            {
                MessageBox.Show("请输入所有信息");
                return;
            }
            sql = "select count(*) from student where stu_id =@userid";
            mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",user_id)
                };
            int g = db_connect.getcount(sql,mySqlParameter );
            if (g != 0)
            {
                MessageBox.Show(" 学号已被注册，请检查是否填写正确");

            }
            else
            {
                byte[] face = CameraHelper.CaptureImage();
                baiduAi = new BaiduAI();
                string isface = baiduAi.face_identify(face);


                if (isface.Equals("unknown_face"))
                {
                     baiduAi.face_useradd(user_id, user_name, face);
                    String sql = "insert into student(stu_id,stu_name,stu_image) values(@userid, @username, @filecontent)";
                    mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",user_id),
                        new MySqlParameter("@username",user_name),
                            new MySqlParameter("@filecontent",face)
                };
                    int i = db_connect.AddNonQuery(sql, mySqlParameter);
                    if (i>0)
                    {
                        MessageBox.Show("注册成功");
                    }
                    else
                    {
                        MessageBox.Show("注册失败");
                    }
                  
                    
                }
                else if(isface.Equals("success"))
                {
                    MessageBox.Show("你的人脸已被注册，请直接登录");
                }
                else if (isface.Equals("no_face"))
                {
                    MessageBox.Show("未检测到人脸，请重试");
                }
                else 
                {
                    MessageBox.Show("人脸识别失败");
                }
              
            }
           
        }

        private void Windows_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CameraHelper.CloseDevice();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
