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
    /// AddExam.xaml 的交互逻辑
    /// </summary>
    public partial class AddExam : UserControl
    {
        DataTable subject_table;
        MySqlParameter[] mySqlParameter;
        public AddExam()
        {
            InitializeComponent();
          string   sql_subject = "select  * from subject";
             subject_table = db_connect.GetTables(sql_subject);
            ExamSubject.ItemsSource = subject_table.DefaultView;
            ExamSubject.DisplayMemberPath = "subject_name";
            ExamSubject.SelectedIndex = 0;
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    ExamHour.Items.Add("0" + i);
                }
                else
                {
                ExamHour.Items.Add(i);         

                }
            }
            for (int i = 0; i < 60; i++)
            {
                if (i<10)
                {
                    ExamMinute.Items.Add("0"+i);
                }
                else
                {
                ExamMinute.Items.Add(i);

                }
            }
        }

        private void AddEaxm_Click(object sender, RoutedEventArgs e)
        {
            string examName =  ExamName.Text.Trim();
            if (examName.Equals(""))
            {
                MessageBox.Show("请输入考试名称");
                return;
            }

            int examsubject =(int) subject_table.Rows[ExamSubject.SelectedIndex]["subject_id"];
            if (examsubject<0)
            {
                MessageBox.Show("请选择考试章节");
                return;
            }
            if (ExamDate.Text.Equals(""))
            {
                MessageBox.Show("请选择允许考试开始日期");
                return;
            }
            string  examDate =  Convert.ToDateTime(ExamDate.Text).ToString("yyyy/MM/dd");
            string examHour = ExamHour.Text;
            if (examHour.Equals(""))
            {
                MessageBox.Show("请选择允许考试开始小时");
                return;
            }
            string examMinute = ExamMinute.Text;
            if (examMinute.Equals(""))
            {
                MessageBox.Show("请输入允许考试开始分钟");
                return;
            }
            string examLater = this.ExamLater.Text;
   
            if (examLater.Equals(""))
            {
                MessageBox.Show("请输入延时开始时间");
                return;
            }
            string examTime = ExamTime.Text;
            if (examTime.Equals(""))
            {
                MessageBox.Show("请输入考试时间");
                return;
            }
            string singleNum = SingleNum.Text;
            if (singleNum.Equals(""))
            {
                MessageBox.Show("请输入选择题数量");
                return;
            }
            string singleScore = SingleScore.Text;
            if (singleScore.Equals(""))
            {
                MessageBox.Show("请输入选择题分数");
                return;
            }

            string bankNum = BankNum.Text;
            if (bankNum.Equals(""))
            {
                MessageBox.Show("请输入填空题数量");
                return;
            }
            string bankScore = BankScore.Text;
            if (bankScore.Equals(""))
            {
                MessageBox.Show("请输入填空题分数");
                return;
            }
            string totalScore = TotalScore.Text;
            if (totalScore.Equals(""))
            {
                MessageBox.Show("请输入总分");
                return;
            }
            bool isRandom =(bool) IsRandom.IsChecked;
          


            IFormatProvider iformat = new System.Globalization.CultureInfo("zh-CN");
            string format= "yyyy/MM/dd HH:mm:ss";
            string dateTime = examDate+" " + examHour +":"+ examMinute+":00";
            DateTime ExamStart = DateTime.ParseExact(dateTime, format,iformat);
            DateTime ExamLater=  ExamStart.AddMinutes(Double.Parse(examLater));

                string sql = "insert into exam(exam_name,exam_subject,exam_starttime,exam_endtime,exam_time,single_num,single_score,bank_num,bank_score,score,is_random) values(@examName,@examsubject,@exam_starttime,@exam_endtime,@exam_time,@single_num,@single_score,@bank_num,@bank_score,@score,@is_random)";
            mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@examName",examName),
                     new MySqlParameter("@examsubject",examsubject),
                      new MySqlParameter("@exam_starttime",ExamStart),
                       new MySqlParameter("@exam_endtime",ExamLater),
                        new MySqlParameter("@exam_time",examTime),
                         new MySqlParameter("@single_num",singleNum),
                          new MySqlParameter("@single_score",singleScore),
                           new MySqlParameter("@bank_num",bankNum),
                            new MySqlParameter("@bank_score",bankNum),
                              new MySqlParameter("@score",totalScore),
                                new MySqlParameter("@is_random",isRandom)
                };
           int i= db_connect.AddNonQuery(sql, mySqlParameter);
            if (i>0)
            {
                MessageBox.Show("添加考试成功");
            }
            else
            {
                MessageBox.Show("添加考试失败");
            }
        }
    }
}
