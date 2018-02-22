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
        MySqlParameter[] mySqlParameter;
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

            bool bu = (bool)is_student.IsChecked;
            userid = user_id.Text;
            username = user_name.Text;
            userpwd = passwordFirst.Password;
            userpwdAgain = passwordAgain.Password;
            if (username.Equals("") | userid.Equals("") | userpwd.Equals(""))
            {
                MessageBox.Show("请输入所有数据");
                return;
            }
            if (!userpwd.Equals(userpwdAgain))
            {
                MessageBox.Show("两次密码不一样");
                return;
            }

            if (bu)
            {
                sql = "select count(*) from student where stu_id =@userid";
                mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@userid",userid)   
                };
                int g = db_connect.getcount(sql,mySqlParameter );
                if (g != 0)
                {
                    MessageBox.Show(" 学生学号已被注册");

                }
                else
                {
                    sql = "insert into student(stu_id,stu_name,stu_pwd) values(@userid,@username,@password))";
                    mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@userid",userid),
                         new MySqlParameter("@username",username),
                          new MySqlParameter("@password",db_connect.GetMD5(userpwd))
                };

                    db_connect.AddNonQuery(sql,mySqlParameter );       
                }
            }
            else
            {
                sql = "select count(*) from teacher where tea_id =@userid";
                mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@userid",userid)
                };
                int g = db_connect.getcount(sql,mySqlParameter );
                if (g != 0)
                {
                    MessageBox.Show(" 老师学号已被注册");

                }
                else
                {
                    sql = "insert into teacher(tea_id,tea_name,tea_pwd) values(@userid,@username,@password))";
                    mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@userid",userid),
                         new MySqlParameter("@username",username),
                          new MySqlParameter("@password",db_connect.GetMD5(userpwd))
                    };
                    db_connect.AddNonQuery(sql,mySqlParameter );
                  
                }
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    }

