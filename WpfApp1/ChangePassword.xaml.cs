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
            if (BaiduAI.usergroup.Equals("学生"))
            {
                String s = "select count(*) from student where stu_id = " + "'" + BaiduAI.userid + "' and stu_pwd='" + db_connect.GetMD5(userpwdOld) + "'";
                if (db_connect.getcount(s) != 0)
                {
                    String sql = "update student  set  stu_pwd='"+ db_connect.GetMD5(userpwdNew) +"' where stu_id='" + BaiduAI.userid + " '";
                    db_connect.AddNonQuery(sql);

                }
                else
                {
                    MessageBox.Show("旧密码错误");
                }
            }
            else
            {
                String s = "select count(*) from teacher where tea_id = " + "'" + BaiduAI.userid + "' and tea_pwd='" + db_connect.GetMD5(userpwdOld) + "'";
                if (db_connect.getcount(s) != 0)
                {
                    String sql = "update teacher  set  tea_pwd='" + db_connect.GetMD5(userpwdNew) + "' where tea_id='" + BaiduAI.userid + " '";
                    db_connect.AddNonQuery(sql);
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

