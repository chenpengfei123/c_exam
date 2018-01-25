﻿using System;
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

namespace WpfApp1
{
    /// <summary>
    /// Login_face.xaml 的交互逻辑
    /// </summary>
    public partial class Login_face : Window
    {
        public static byte[] face;
        public Login_face()
        {

            InitializeComponent();
            CameraHelper.CameraInit(player);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //byte[] face = File.ReadAllBytes("D:/weizhong.jpg");
             face = CameraHelper.CaptureImage();
            BaiduAI baiduAI = new BaiduAI();
          string logininfo=  baiduAI.face_identify(face);

            if (logininfo.Equals("未识别到人脸"))
            {
                MessageBox.Show("对不起，未识别到人脸");
                return;
            }
            else if (logininfo.Equals("识别不出你是谁"))
            {
                MessageBox.Show("对不起，识别不出你是谁");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CameraHelper.CloseDevice();
            Register_face register_Face = new Register_face();
            register_Face.Owner = this;
            register_Face.ShowDialog();
            CameraHelper.CameraInit(player);
        }
    }
    }
