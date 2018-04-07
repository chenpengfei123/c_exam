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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Answer_Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Answer_Manager : Window
    {
        int examID;
        MySqlParameter[] mySqlParameter;
        string sql_single;
        string sql_blank;
        DataTable dataTable_single;
        DataTable dataTable_blank;
        DataTable dataTable_UserId;
        DataTable dataTable_ExamId;
        public Answer_Manager()
        {
            InitializeComponent();
            string sql = "select stu_id  from student";
            dataTable_UserId = db_connect.GetTables(sql);
            dataTable_UserId.Rows.Add("全部");
            StuId.ItemsSource = dataTable_UserId.DefaultView;
            StuId.DisplayMemberPath = "stu_id";  
            StuId.SelectedIndex = dataTable_UserId.Rows.Count-1;

                sql = "select exam_id,exam_name from exam";
            dataTable_ExamId = db_connect.GetTables(sql);
            dataTable_ExamId.Rows.Add(0,"全部");
            ExamName.ItemsSource = dataTable_ExamId.DefaultView;
            ExamName.DisplayMemberPath = "exam_name";
            ExamName.SelectedIndex = dataTable_ExamId.Rows.Count-1;

            sql_single = "select  ques_id,stu_id,stu_answer,exam_name,time from exam_single_answer,exam where exam.exam_id=exam_single_answer.exam_id";
    
            ShowSingleAnswer();

            sql_blank = "select ques_id,stu_id,stu_answer,exam_name,time from exam_blank_answer,exam where exam.exam_id=exam_blank_answer.exam_id";
          
            ShowBlankAnswer();
        }
        private void ShowSingleAnswer(params MySqlParameter[] commandParameters)
        {
            dataTable_single = db_connect.GetTables(sql_single,commandParameters );
            dataTable_single.Columns["ques_id"].ColumnName = "题目编号";
            dataTable_single.Columns["stu_id"].ColumnName = "学号";
            dataTable_single.Columns["stu_answer"].ColumnName = "答案";
            dataTable_single.Columns["exam_name"].ColumnName = "考试名称";
            dataTable_single.Columns["time"].ColumnName = "作答时间";
            Answer_Single.ItemsSource = dataTable_single.DefaultView;
        }


        private void ShowBlankAnswer(params MySqlParameter[] commandParameters)
        {
            dataTable_blank = db_connect.GetTables(sql_blank, commandParameters);
            dataTable_blank.Columns["ques_id"].ColumnName = "题目编号";
            dataTable_blank.Columns["stu_id"].ColumnName = "学号";
            dataTable_blank.Columns["stu_answer"].ColumnName = "答案";
            dataTable_blank.Columns["exam_name"].ColumnName = "考试名称";
            dataTable_blank.Columns["time"].ColumnName = "作答时间";
            Answer_Blank.ItemsSource = dataTable_blank.DefaultView;
        }

        private void Sure_Click(object sender, RoutedEventArgs e)
        {
           

            if (!StuId.Text.Trim().Equals("全部")& ExamName.Text.Trim().Equals("全部"))
            {
               
                sql_single = "select  ques_id,stu_id,stu_answer,exam_name,time from exam_single_answer,exam where exam.exam_id=exam_single_answer.exam_id and stu_id=@userid";
                sql_blank = "select ques_id,stu_id,stu_answer,exam_name,time from exam_blank_answer,exam where exam.exam_id=exam_blank_answer.exam_id and stu_id=@userid";

                mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",StuId.Text.Trim())
                };

                ShowSingleAnswer(mySqlParameter );
        
                ShowBlankAnswer(mySqlParameter );
            }
            else if (StuId.Text.Trim().Equals("全部") & !ExamName.Text.Trim().Equals("全部"))
            {
                sql_single = "select  ques_id,stu_id,stu_answer,exam_name,time from exam_single_answer,exam where exam.exam_id=exam_single_answer.exam_id and exam.exam_id=@subject";
                sql_blank = "select ques_id,stu_id,stu_answer,exam_name,time from exam_blank_answer,exam where exam.exam_id=exam_blank_answer.exam_id and exam.exam_id=@subject";

                mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@subject",examID)
                };
                ShowSingleAnswer(mySqlParameter );
                ShowBlankAnswer(mySqlParameter );
            }
            else if(!StuId.Text.Trim().Equals("全部") & !ExamName.Text.Trim().Equals("全部"))
            {
                sql_single = "select  ques_id,stu_id,stu_answer,exam_name,time from exam_single_answer,exam where exam.exam_id=exam_single_answer.exam_id and exam.exam_id=@subject and stu_id=@userid";

                sql_blank = "select ques_id,stu_id,stu_answer,exam_name,time from exam_blank_answer,exam where exam.exam_id=exam_blank_answer.exam_id and exam.exam_id=@subject and stu_id=@userid";
                mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@userid",StuId.Text.Trim()),
                    new MySqlParameter("@subject",examID)
                };
                ShowSingleAnswer(mySqlParameter );
                ShowBlankAnswer(mySqlParameter );
            }
            else
            {
                sql_single = "select  ques_id,stu_id,stu_answer,exam_name,time from exam_single_answer,exam where exam.exam_id=exam_single_answer.exam_id";

                ShowSingleAnswer();

                sql_blank = "select ques_id,stu_id,stu_answer,exam_name,time from exam_blank_answer,exam where exam.exam_id=exam_blank_answer.exam_id";

                ShowBlankAnswer();
            }
        }

        private void AnswerPicture_Click(object sender, RoutedEventArgs e)
        {
            StuPicture.Source = null;
            ExamPicture1.Source = null;
            ExamPicture2.Source = null;
            string sql = "select picture1 from exam_picture where stu_id=@userid and exam_id=@exam_id";

            mySqlParameter = new MySqlParameter[] {

                    new MySqlParameter("@userid",StuId.Text),
                      new MySqlParameter("@exam_id",examID)
                };
          byte[] image = db_connect.getpictures(sql, mySqlParameter);
            if (image != null)
            {
                MemoryStream imageStream = new MemoryStream(image);
                BitmapImage bit = new BitmapImage();
                bit.BeginInit();
                bit.StreamSource = imageStream;
                bit.EndInit();
                ExamPicture1.Source = bit;
            }
             sql = "select picture2 from exam_picture where stu_id=@userid and exam_id=@exam_id";
        
            if (image != null)
            {
                MemoryStream imageStream = new MemoryStream(image);
                BitmapImage bit = new BitmapImage();
                bit.BeginInit();
                bit.StreamSource = imageStream;
                bit.EndInit();
                ExamPicture2.Source = bit;
            }
            sql = "select stu_image from student where stu_id='" + StuId.Text+"'";
            image = db_connect.getpictures(sql);
            if (image != null)
            {
                MemoryStream imageStream = new MemoryStream(image);
                BitmapImage bit = new BitmapImage();
                bit.BeginInit();
                bit.StreamSource = imageStream;
                bit.EndInit();
                StuPicture.Source = bit;
            }

        }

        private void ExamName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            examID = (int)dataTable_ExamId.Rows[ExamName.SelectedIndex]["exam_id"];
        }
    }
}
