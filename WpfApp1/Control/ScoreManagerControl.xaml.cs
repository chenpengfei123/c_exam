using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// ScoreManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreManagerControl : UserControl
    {
        MySqlParameter[] mySqlParameter;
        DataTable dataTable_UserId;
        DataTable dataTable_ExamId;
        string sql_score;
        DataTable score_table;
        public ScoreManagerControl()
        {
            InitializeComponent();

            sql_score = "select stu_id  from student";
            dataTable_UserId = db_connect.GetTables(sql_score);
            dataTable_UserId.Rows.Add("全部");
            StuID.ItemsSource = dataTable_UserId.DefaultView;
            StuID.DisplayMemberPath = "stu_id";
            StuID.SelectedIndex = dataTable_UserId.Rows.Count - 1;

            sql_score = "select exam_id,exam_name from exam";
            dataTable_ExamId = db_connect.GetTables(sql_score);
            dataTable_ExamId.Rows.Add(0, "全部");
            ExamName.ItemsSource = dataTable_ExamId.DefaultView;
            ExamName.DisplayMemberPath = "exam_name";
            ExamName.SelectedIndex = dataTable_ExamId.Rows.Count - 1;
            sql_score = "select stu_id,stu_name,exam_name,score_single,score_blank,exam_score.score,start_time,end_time from exam_score,exam where exam.exam_id=exam_score.exam_id";
            ShowScore();
        }

        private void ShowScore(params MySqlParameter[] commandParameters)
        {
           
            score_table = db_connect.GetTables(sql_score,commandParameters );
            score_table.Columns[0].ColumnName = "学号";
            score_table.Columns[1].ColumnName = "姓名";
            score_table.Columns[2].ColumnName = "试卷名称";
            score_table.Columns[3].ColumnName = "选择题得分";
            score_table.Columns[4].ColumnName = "填空题得分";
            score_table.Columns[5].ColumnName = "总分";
            score_table.Columns[6].ColumnName = "开始考试时间";
            score_table.Columns[7].ColumnName = "结束考试时间";
            getscores.ItemsSource = score_table.DefaultView;
        }
        private void RefreshScore_Click(object sender, RoutedEventArgs e)
        {
            if (mySqlParameter !=null)
            {
            ShowScore(mySqlParameter );

            }
            else
            {
                ShowScore();
            }
        }
        private void AnswerManager_Click(object sender, RoutedEventArgs e)
        {
            Answer_Manager answer = new Answer_Manager();
            answer.Owner = Window.GetWindow(this);
            answer.ShowDialog();
        }

        private void Sure_Click(object sender, RoutedEventArgs e)
        {
            int examID = (int)dataTable_ExamId.Rows[ExamName.SelectedIndex]["exam_id"];

            if (!StuID.Text.Trim().Equals("全部") & ExamName.Text.Trim().Equals("全部"))
            {

                sql_score = "select stu_id,stu_name,exam_name,score_single,score_blank,exam_score.score,start_time,end_time from exam_score,exam where exam.exam_id=exam_score.exam_id and stu_id=@userid";


                mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@userid",StuID.Text.Trim())
                };

                ShowScore(mySqlParameter);

           
            }
            else if (StuID.Text.Trim().Equals("全部") & !ExamName.Text.Trim().Equals("全部"))
            {
                sql_score = "select stu_id,stu_name,exam_name,score_single,score_blank,exam_score.score,start_time,end_time from exam_score,exam where exam.exam_id=exam_score.exam_id and exam.exam_id=@exam_id";
          

                mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@exam_id",examID)
                };
                ShowScore(mySqlParameter);
       
            }
            else if (!StuID.Text.Trim().Equals("全部") & !ExamName.Text.Trim().Equals("全部"))
            {
                sql_score = "select stu_id,stu_name,exam_name,score_single,score_blank,exam_score.score,start_time,end_time from exam_score,exam where exam.exam_id=exam_score.exam_id and exam.exam_id=@exam_id and stu_id=@userid";

                mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@userid",StuID.Text.Trim()),
                    new MySqlParameter("@exam_id",examID)
                };
                ShowScore(mySqlParameter);
 
            }
            else
            {
                sql_score = "select stu_id,stu_name,exam_name,score_single,score_blank,exam_score.score,start_time,end_time from exam_score,exam where exam.exam_id=exam_score.exam_id";

                ShowScore();


                
            }
        }

      
    }
}
