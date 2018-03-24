using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Login_normal : Window
    {
        MySqlParameter[] mySqlParameter;
        String sql;
        String userid;
        String userpwd;
        public Login_normal()
        {
            InitializeComponent();
            userId.Text = db_connect.GetSettingString("userName");
            userPwd.Password = db_connect.GetSettingString("password");
            is_Remind.IsChecked = db_connect.GetSettingString("isRemind").Equals("true") ? true:false ;
            is_stu.IsChecked = db_connect.GetSettingString("isStudent").Equals("true") ? true : false;
            is_tea.IsChecked = db_connect.GetSettingString("isTeacher").Equals("true") ? true : false;
        }

        private void Regiser_click(object sender, RoutedEventArgs e)
        {
          
            Register register = new Register();
            register.Owner = this;
            register.ShowDialog();
            
        }   

        private void Login_click(object sender, RoutedEventArgs e)
        {
            Login();

        }

        private void Pwd_up(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Login();  
            }
        }

        private void Login()
        {
  
            bool bu = (bool)is_stu.IsChecked;
            userid = userId.Text;
             userpwd = userPwd.Password;
            if (String.IsNullOrEmpty(userid) || (String.IsNullOrEmpty(userpwd)))
            {
                MessageBox.Show("账户或密码不能为空");
                return;
            }
         
            if (bu)
            {
                sql = "Select count(*) from student where stu_id=@userid and stu_pwd=@password";

                 mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",userid),
                     new MySqlParameter("@password", db_connect.GetMD5(userpwd)),
                };
               
                int g = db_connect.getcount(sql,mySqlParameter);
                if (g != 0)
                {
                    is_remember();
                    sql = "select stu_name from student where stu_id=@userid";
                    mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",userid)                  
                };
                 
       
                    BaiduAI.userid = userid;
                    BaiduAI.username = db_connect.getstring(sql,mySqlParameter);
                    BaiduAI.usergroup = "学生";
                    student_main exam = new student_main();
                    exam.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("账户或密码错误，请检查");
                   
                }
            }
            else
            {
                sql = "Select count(*) from teacher where tea_id=@userid and tea_pwd=@password";

                mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",userid),
                     new MySqlParameter("@password", db_connect.GetMD5(userpwd)),
                };
                int g = db_connect.getcount(sql,mySqlParameter);
                if (g != 0)
                {
                    is_remember();
                    sql = "select tea_name from teacher where tea_id=@userid";
                    mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",userid)
                };
                    BaiduAI.userid = userid;
                    BaiduAI.username = db_connect.getstring(sql,mySqlParameter);
                    BaiduAI.usergroup="老师";
                    Teacher_Main teacher = new Teacher_Main();      
                    teacher.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("账户或密码错误，请检查");
                   
                }
            }

                
               
        }

        private void is_remember()
        {
            if ((bool)is_Remind.IsChecked)
            {
                db_connect.UpdateSettingString("userName", userid);
                db_connect.UpdateSettingString("password", userpwd);
                db_connect.UpdateSettingString("isRemind", "true");
            }
            else
            {
                db_connect.UpdateSettingString("userName", "");
                db_connect.UpdateSettingString("password", "");
                db_connect.UpdateSettingString("isRemind", "false");
            }
            db_connect.UpdateSettingString("isStudent", (bool)is_stu.IsChecked ? "true" : "false");
            db_connect.UpdateSettingString("isTeacher", (bool)is_tea.IsChecked ? "true" : "false");
        }







        private void login_face(object sender, RoutedEventArgs e)
        {
            Login_face login_Face = new Login_face();
            login_Face.Show();
            Close();
        }
    }

}
