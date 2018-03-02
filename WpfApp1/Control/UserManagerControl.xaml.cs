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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Control
{
    /// <summary>
    /// UserManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagerControl : UserControl
    {
        byte[] image;
        MySqlParameter[] mySqlParameter;
        DataTable student_table;
        string sql_student;
        public UserManagerControl()
        {
            InitializeComponent();
            ShowStudent();
        }

        private void ShowStudent()
        {
            sql_student = "select stu_id,stu_name from student";
            student_table = db_connect.GetTables(sql_student);
            student_table.Columns[0].ColumnName = "学号";
            student_table.Columns[1].ColumnName = "姓名";
            stu_manager.ItemsSource = student_table.DefaultView;
            UserId.ItemsSource = student_table.DefaultView;
            UserId.DisplayMemberPath = "学号";
        }
        private void RefreshStudent_Click(object sender, RoutedEventArgs e)
        {
            ShowStudent();
        }
        private void ShowPicture_Click(object sender, RoutedEventArgs e)
        {
            if (UserId.SelectedIndex<0)
            {
                System.Windows.MessageBox.Show("请选择学号");
            }
            else
            {
                string sql = "select stu_image from student where stu_id=@userid";

                mySqlParameter = new MySqlParameter[] {

                    new MySqlParameter("@userid",UserId.Text)
                };
                image = db_connect.getpictures(sql, mySqlParameter);
                if (image != null)
                {
                    MemoryStream imageStream = new MemoryStream(image);
                    BitmapImage bit = new BitmapImage();
                    bit.BeginInit();
                    bit.StreamSource = imageStream;
                    bit.EndInit();
                    image2.Source = bit;
                }

            }

        }
    }
}
