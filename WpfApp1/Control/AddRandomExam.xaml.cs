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
    public partial class AddRandomExam : UserControl
    {
        DataTable subject_table;
        MySqlParameter[] mySqlParameter;
        int singlenum;
        int banknum;
        public AddRandomExam()
        {
            InitializeComponent();
          string   sql_subject = "select  * from subject";
             subject_table = db_connect.GetTables(sql_subject);
            ExamSubject.ItemsSource = subject_table.DefaultView;
            ExamSubject.DisplayMemberPath = "subject_name";
            ExamSubject.SelectedIndex = 0;
            for (int i = 0; i < 24; i++)
            {
           
                ExamHour.Items.Add(i.ToString().PadLeft(2,'0'));         

                
            }
            for (int i = 0; i < 60; i++)
            {
            
                ExamMinute.Items.Add(i.ToString().PadLeft(2, '0'));

            }
        }

        private void AddEaxm_Click(object sender, RoutedEventArgs e)
        {
            try
            {

          
            string examName =  ExamName.Text.Trim();
            if (String.IsNullOrEmpty(examName))
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
            if (String.IsNullOrEmpty(ExamDate.Text))
            {
                MessageBox.Show("请选择允许考试开始日期");
                return;
            }
            string  examDate =  Convert.ToDateTime(ExamDate.Text).ToString("yyyy/MM/dd");
            string examHour = ExamHour.Text;
            if (String.IsNullOrEmpty(examHour))
            {
                MessageBox.Show("请选择允许考试开始小时");
                return;
            }
            string examMinute = ExamMinute.Text;
            if (String.IsNullOrEmpty(examMinute))
            {
                MessageBox.Show("请输入允许考试开始分钟");
                return;
            }
            string examLater = this.ExamLater.Text;
   
            if (String.IsNullOrEmpty(examLater))
            {
                MessageBox.Show("请输入延时开始时间");
                return;
            }
            string examTime = ExamTime.Text;
            if (String.IsNullOrEmpty(examTime))
            {
                MessageBox.Show("请输入考试时间");
                return;
            }
            string singleNum = SingleNum.Text;
            if (String.IsNullOrEmpty(singleNum))
            {
                MessageBox.Show("请输入选择题数量");
                return;
            }
            string singleScore = SingleScore.Text;
            if (String.IsNullOrEmpty(singleScore))
            {
                MessageBox.Show("请输入选择题分数");
                return;
            }

            string bankNum = BankNum.Text;
            if (String.IsNullOrEmpty(bankNum))
            {
                MessageBox.Show("请输入填空题数量");
                return;
            }
            string bankScore = BankScore.Text;
            if (String.IsNullOrEmpty(bankScore))
            {
                MessageBox.Show("请输入填空题分数");
                return;
            }
            string totalScore = TotalScore.Text;
            if (String.IsNullOrEmpty(totalScore))
            {
                MessageBox.Show("请输入总分");
                return;
            }
      
          


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
                            new MySqlParameter("@bank_score",bankScore),
                              new MySqlParameter("@score",totalScore),
                                new MySqlParameter("@is_random",1)
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
        catch (Exception)
        {

                MessageBox.Show("确认输入的为整数");
        }
        }

        private void SimulationSubject_Change(object sender, SelectionChangedEventArgs e)
        {
            int examsubject = (int)subject_table.Rows[ExamSubject.SelectedIndex]["subject_id"];
            string sql = "select count(*)from single_question where ques_subject=" + examsubject;
            singlenum = db_connect.getcount(sql);
            SingleMaxNum.Content = "共有" + singlenum + "题";
            sql = "select count(*)from bank_question where ques_subject=" + examsubject;
            banknum = db_connect.getcount(sql);
            BankMaxNum.Content = "共有" + banknum + "题";
        }

        private void GetTotalScore_Click(object sender, RoutedEventArgs e)
        {
            try
            {

        
            if (String.IsNullOrEmpty(SingleNum.Text))
            {
                MessageBox.Show("请输入选择题数量");
                return;
            }
        
            if (String.IsNullOrEmpty(SingleScore.Text))
            {
                MessageBox.Show("请输入选择题分数");
                return;
            }

            if (String.IsNullOrEmpty(BankNum.Text))
            {
                MessageBox.Show("请输入填空题数量");
                return;
            }
            if (String.IsNullOrEmpty(BankScore.Text))
            {
                MessageBox.Show("请输入填空题分数");
                return;
            }
            int singleNum = int.Parse(SingleNum.Text);
            int singleScore = int.Parse(SingleScore.Text);
            int bankNum = int.Parse(BankNum.Text);
            int bankScore = int.Parse(BankScore.Text);
            if (singleNum>singlenum)
            {
                MessageBox.Show("输入选择题数量不能大于总共数量");
                return;
            }
            if (bankNum > banknum)
            {
                MessageBox.Show("输入填空题数量不能大于总共数量");
                return;
            }
            int totalscore = singleNum * singleScore + bankNum * bankScore;
            TotalScore.Text = totalscore.ToString();
       
              }
            catch (Exception)
            {

                MessageBox.Show("确认输入的为整数");
            }
        }
    }
}
