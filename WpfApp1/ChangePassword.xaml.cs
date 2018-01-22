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
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// ChangePassword.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePassword : Window
    {
        String userpwdOld;
        String userpwdNew;
        String userpwdAgain;
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void Sure_Click(object sender, RoutedEventArgs e)
        {
            userpwdOld = passwordOld.Password;
            userpwdNew = passwordNew.Password;
            userpwdAgain = passwordNew1.Password;
            if (userpwdOld.Equals("") | userpwdNew.Equals(""))
            {
                MessageBox.Show("请输入密码");
                return;
            }
            if (!userpwdNew.Equals(userpwdAgain))
            {
                MessageBox.Show("新密码与确认密码不一样");
                return;
            }
            MySqlConnection mycon = db_connect.Mysql_con();
            String s = "select count(*) from student where stu_name = " + "'" + LoginWindow.stu_name + "' and stu_pwd='"+db_connect.GetMD5(userpwdOld)+"'";

            try
            {
                mycon.Open();
                MySqlCommand mycmd1 = new MySqlCommand(s, mycon);
                int g = int.Parse(mycmd1.ExecuteScalar().ToString());
                if (g != 0)
                {
                    String sql = "update student  set  stu_pwd='"+ db_connect.GetMD5(userpwdNew) +"' where stu_name='" + LoginWindow.stu_name +" '";


                    MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                    mycmd.ExecuteNonQuery();
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show("旧密码错误");
                }
            }
            catch
            {
                MessageBox.Show("请检查网络顺畅");
            }
            finally
            {
                mycon.Close();
            }



        }

        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    }

