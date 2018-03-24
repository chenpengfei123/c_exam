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
        MySqlParameter[] mySqlParameter;
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
            if (String.IsNullOrEmpty(userpwdOld)| String.IsNullOrEmpty(userpwdNew))
            {
                MessageBox.Show("请输入密码");
                return;
            }
            if (!userpwdNew.Equals(userpwdAgain))
            {
                MessageBox.Show("新密码与确认密码不一样");
                return;
            }
            if (BaiduAI.usergroup.Equals("学生"))
            {
                String sql = "select count(*) from student where stu_id = @userid  and stu_pwd=@password";

                mySqlParameter = new MySqlParameter[] {
                         new MySqlParameter("@password", db_connect.GetMD5(userpwdOld)),
                    new MySqlParameter("@userid",BaiduAI.userid)
                };
                if (db_connect.getcount(sql,mySqlParameter ) != 0)
                {
                     sql = "update student  set  stu_pwd=@password where stu_id=@userid";

                    mySqlParameter = new MySqlParameter[] {
                         new MySqlParameter("@password", db_connect.GetMD5(userpwdNew)),
                    new MySqlParameter("@userid",BaiduAI.userid)
                    };
                    int i = db_connect.AddNonQuery(sql, mySqlParameter);
                    if (i > 0)
                    {
                        MessageBox.Show("修改成功");
                    }
                    else
                    {
                        MessageBox.Show("修改失败");
                    }

                }
                else
                {
                    MessageBox.Show("旧密码错误");
                }
            }
            else
            {
                String sql = "select count(*) from teacher where tea_id =@userid  and tea_pwd=@password";
                mySqlParameter = new MySqlParameter[] {
                         new MySqlParameter("@password", db_connect.GetMD5(userpwdOld)),
                    new MySqlParameter("@userid",BaiduAI.userid)
                };
                if (db_connect.getcount(sql,mySqlParameter ) != 0)
                {
                    sql = "update teacher  set  tea_pwd=@password where tea_id=@userid";
                    mySqlParameter = new MySqlParameter[] {
                         new MySqlParameter("@password", db_connect.GetMD5(userpwdNew)),
                     new MySqlParameter("@userid",BaiduAI.userid)
                    };
                    int i = db_connect.AddNonQuery(sql, mySqlParameter);
                    if (i > 0)
                    {
                        MessageBox.Show("修改成功");
                    }
                    else
                    {
                        MessageBox.Show("修改失败");
                    }
                }
                else
                {
                    MessageBox.Show("旧密码错误");
                }
            }
         

   
            }
         



        

        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    }

