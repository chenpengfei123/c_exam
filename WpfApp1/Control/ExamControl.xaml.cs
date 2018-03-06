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
    /// ExamControl.xaml 的交互逻辑
    /// </summary>
    public partial class ExamControl : UserControl
    {
        string exam_subject;
        int subject_id;
        string single_num;
        string bank_num;
        DataTable datatable;
      public  int   exam_time;
        int single_score;
        int bank_score;
        public ExamControl()
        {
            InitializeComponent();

            Init();

        }

        private void Init()
        {
            string sql = "select * from exam,subject where now()>exam_starttime and now()<exam_endtime and subject_id=exam_subject";
            datatable = db_connect.GetTables(sql);
            if (datatable.Rows.Count == 0)
            {
                StartExam.IsEnabled = false;
                ExamInfo.Text = "当前暂无考试";
            }
            ExamSubject.ItemsSource = datatable.DefaultView;
            ExamSubject.DisplayMemberPath = "exam_name";
            ExamSubject.SelectedIndex = 0;
        }

        private void StartExam_Click(object sender, RoutedEventArgs e)
        {
            if (student_main.image!=null)
            {

            String sql_single = "Select * from single_question where ques_subject= " + subject_id +" order by rand() limit "+ single_num;
            String sql_bank = "Select * from bank_question  where ques_subject= " + subject_id + " order by rand() limit " + bank_num;
            Single startanswer = new Single(subject_id, ExamSubject.Text, sql_single, sql_bank,exam_time,single_score
                ,bank_score);
            startanswer.Owner = Window.GetWindow(this);
            startanswer.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先上传你的完整照片");
            }
        }

        private void ExamSubjectChange_Click(object sender, SelectionChangedEventArgs e)
        {
            int iCurrentIndex = this.ExamSubject.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr = datatable.Rows[iCurrentIndex];
            exam_subject = dr["subject_name"].ToString();
            subject_id = (int)dr["exam_subject"];
            string exam_starttime = dr["exam_starttime"].ToString();
            string exam_endtime = dr["exam_endtime"].ToString();
            exam_time = (int)( dr["exam_time"]);
             single_num = dr["single_num"].ToString();
             single_score = (int)dr["single_score"];
             bank_num = dr["bank_num"].ToString();
            bank_score = (int)dr["bank_score"];
            string score =dr["score"].ToString();
            string is_random =dr["is_random"].ToString();
            ExamInfo.Text = "考试章节：" + exam_subject + "\n考试开始时间: " + exam_starttime + " 到 " + exam_endtime + "\n考试时间：" + exam_time + "分钟\n选择题数量: " +single_num + "题  选择题分值: "+ single_score + "分\n填空题数量: "+bank_num + "题  填空题分值: " +bank_score + "分\n总分: " +score + "分\n是否随机组卷: "+(is_random.Equals("1")? "是":"否");
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {

            Init();
        }
    }
}
