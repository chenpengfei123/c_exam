using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using WpfApp1.Bank;

namespace WpfApp1
{
    /// <summary>
    /// Teacher_Main.xaml 的交互逻辑
    /// </summary>
    public partial class Teacher_Main : Window
    {     
        public Teacher_Main()
        {
            InitializeComponent();              
            welecome.Content = "欢迎您，" + BaiduAI.username + "老师";
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

        private void Window_Closing(object sender,System.ComponentModel.CancelEventArgs e)
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

       
 
      
    }
    }

