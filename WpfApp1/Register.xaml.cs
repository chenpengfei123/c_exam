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
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        String userid;
        String userpwd;
        String userpwdAgain;

        public Register()
        {
            InitializeComponent();
          
        

        }

        private void Sure_Click(object sender, RoutedEventArgs e)
        {
            
             
                userpwd = passwordFirst.Password;
                userid = username.Text;
            userpwdAgain = passwordAgain.Password;
                if (userid.Equals("") | userpwd.Equals(""))
                {
                    MessageBox.Show("账户或密码不能为空");
                    return;
                }
            if (!userpwd.Equals(userpwdAgain))
            {
                MessageBox.Show("两次密码不一样");
                return;
            }
                MySqlConnection mycon = db_connect.Mysql_con();
                String s = "select count(*) from student where stu_name = " + "'" + userid + "'";
               
                try
                {
                    mycon.Open();
                    MySqlCommand mycmd1 = new MySqlCommand(s, mycon);
                    int g = int.Parse(mycmd1.ExecuteScalar().ToString());
                    if (g != 0)
                    {
                        MessageBox.Show(" 用户名已被使用");

                    }
                    else
                    {
                        String sql = "insert into student(stu_name,stu_pwd) values('" + userid + "','" + db_connect.GetMD5(userpwd) + "')";

                       
                        MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                        mycmd.ExecuteNonQuery();
                        MessageBox.Show("注册成功");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
