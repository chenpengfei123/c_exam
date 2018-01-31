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
        String username;
        String sql;
        public Register()
        {
            InitializeComponent();
          
        

        }

        private void Sure_Click(object sender, RoutedEventArgs e)
        {
            
             
                userid = user_id.Text;
                 username = user_name.Text;
                userpwd = passwordFirst.Password;
                userpwdAgain = passwordAgain.Password;
                if (username.Equals("")| userid.Equals("") | userpwd.Equals(""))          
                {
                    MessageBox.Show("请输入所有数据");
                    return;
                }
            if (!userpwd.Equals(userpwdAgain))
            {
                MessageBox.Show("两次密码不一样");
                return;
            }
  
                 sql = "select count(*) from student where stu_id = " + "'" + userid + "'";
                   int g = db_connect.getcount(sql);
                    if (g != 0)
                    {
                        MessageBox.Show(" 学号已被注册");

                    }
                    else
                    {
                    sql = "insert into student(stu_id,stu_name,stu_pwd) values('" + userid + "','"+username+"','" + db_connect.GetMD5(userpwd) + "')";


                          db_connect.AddNonQuery(sql);
                        MessageBox.Show("注册成功");
                    }
                }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    }

