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
    public partial class LoginWindow : Window
    {
        String userid;
        String userpwd;
       public static String stu_name;
        public LoginWindow()
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
            //Register();
            //Register register = new Register();
            //register.ShowDialog();
            Register_face register_Face = new Register_face();
            register_Face.Owner = this;
            register_Face.ShowDialog();
            
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
            MySqlConnection mycon = db_connect.Mysql_con();
            userid = userId.Text;
             userpwd = userPwd.Password;
            if (userid.Equals("") || userpwd.Equals(""))
            {
                MessageBox.Show("账户或密码不能为空");
                return;
            }
            String sql = null;
            if (bu)
            {
                sql = "Select count(*) from student where stu_name='" + userid + "'"+"and stu_pwd='"+db_connect.GetMD5(userpwd)+"'";
            }
            else
            {
                sql = "Select count(*) from teacher where tea_name='" + userid + "'"+"and tea_pwd='"+db_connect.GetMD5(userpwd)+"'";
            }
            MySqlCommand mycmd = new MySqlCommand(sql, mycon);

            try
            {
                mycon.Open();
                int g = int.Parse(mycmd.ExecuteScalar().ToString());
                if (g != 0)
                {
                    //MessageBox.Show(" 登录成功");
                    if ((bool)is_Remind.IsChecked)
                    {
                        db_connect.UpdateSettingString("userName", userid);
                        db_connect.UpdateSettingString("password", userpwd);
                        db_connect.UpdateSettingString("isRemind", "true");
                    }
                    else {
                        db_connect.UpdateSettingString("userName", "");
                        db_connect.UpdateSettingString("password", "");
                        db_connect.UpdateSettingString("isRemind", "false");
                    }
                    db_connect.UpdateSettingString("isStudent", (bool)is_stu.IsChecked?"true":"false");
                    db_connect.UpdateSettingString("isTeacher", (bool)is_tea.IsChecked ? "true" : "false");
                    stu_name = userid;
                    exam  exam = new exam();
                    exam.Show();
                    Close();
                }
             
                else
                {
                    MessageBox.Show("登录失败");
                }
            }
            catch (Exception){
                MessageBox.Show("请检查网络顺畅");
            }
            finally
            {
               
                mycon.Close();
            }
        }

        //private void Register()
        //{
        //    bool bu = (bool)is_stu.IsChecked;
        //     userpwd = userPwd.Password;
        //     userid = userId.Text;
        //    if (userid.Equals("") | userpwd.Equals(""))
        //    {
        //        MessageBox.Show("账户或密码不能为空");
        //        return;
        //    }
        //    MySqlConnection mycon = db_connect.Mysql_con();
        //    String s;
        //    if (bu)
        //    {
        //        s = "select count(*) from student where stu_name = " + "'" + userid + "'";
        //    }
        //    else
        //    {
        //        s = "select count(*) from teacher where tea_name = " + "'" + userid + "'";
        //    }
        //    try
        //    {
        //           mycon.Open();
        //        MySqlCommand mycmd1 = new MySqlCommand(s, mycon);
        //        int g = int.Parse(mycmd1.ExecuteScalar().ToString());
        //        if (g != 0)
        //        {
        //            MessageBox.Show(" 用户名已被使用");

        //        }
        //        else
        //        {
        //            String sql;
        //            if (bu)
        //            {
        //                sql = "insert into student(stu_name,stu_pwd) values('" + userid + "','" + db_connect.GetMD5(userpwd) + "')";

        //            }
        //            else
        //            {
        //                sql = "insert into teacher(tea_name,tea_pwd) values('" + userid + "','" + db_connect.GetMD5(userpwd) + "')";
        //            }
        //            MySqlCommand mycmd = new MySqlCommand(sql, mycon);
        //            mycmd.ExecuteNonQuery();
        //            MessageBox.Show("注册成功");
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("请检查网络顺畅");
        //    }
        //    finally
        //    {
        //        mycon.Close();
        //    }


           
        //}

   

        private void forgetPassword(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("忘记密码");
        }

        private void changeColor(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("注册");
        }
    }

}
