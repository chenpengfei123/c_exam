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
    /// ExamManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ExamManagerControl : UserControl
    {
        string sql;
        DataTable dataTable;
        public ExamManagerControl()
        {
            InitializeComponent();

            GetExam();
        }

        private void GetExam()
        {
            sql = "select exam_id, exam_name, subject_name, exam_starttime, exam_endtime, exam_time, single_num, single_score, blank_num, blank_score, score from exam,subject where now()>exam_endtime and subject_id=exam_subject";
            dataTable = GetExamInfo(sql);
            PastExamManager.ItemsSource = dataTable.DefaultView;

            sql = "select exam_id, exam_name, subject_name, exam_starttime, exam_endtime, exam_time, single_num, single_score, blank_num, blank_score, score  from exam,subject where now()<exam_endtime and subject_id=exam_subject";
            dataTable = GetExamInfo(sql);
            NowExamManager.ItemsSource = dataTable.DefaultView;
        }

        private static DataTable GetExamInfo(string sql)
        {
            DataTable dataTable = db_connect.GetTables(sql);
            dataTable.Columns[0].ColumnName = "考试编号";
            dataTable.Columns[1].ColumnName = "考试名称";
            dataTable.Columns[2].ColumnName = "考试章节";
            dataTable.Columns[3].ColumnName = "考试最早开始时间";
            dataTable.Columns[4].ColumnName = "考试最晚开始时间";
            dataTable.Columns[5].ColumnName = "考试时间";
            dataTable.Columns[6].ColumnName = "选择题数量";
            dataTable.Columns[7].ColumnName = "选择题分数";
            dataTable.Columns[8].ColumnName = "填空题数量";
            dataTable.Columns[9].ColumnName = "填空题分数";
            dataTable.Columns[10].ColumnName = "总分";
  
            return dataTable;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            GetExam();
        }
    }
}
