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
    /// SimulationControl.xaml 的交互逻辑
    /// </summary>
    public partial class SimulationControl : UserControl
    {
        int singlenum;
        int banknum;
        int subjectID;
        public string subjectName;
        string sql_subject;
        DataTable subject_table;
        public SimulationControl()
        {
            InitializeComponent();
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            SimulationSubject.ItemsSource =subject_table.DefaultView;
            SimulationSubject.DisplayMemberPath = "subject_name";
            SimulationSubject.SelectedIndex = 0;

        }

        private void SimulationSubject_Change(object sender, SelectionChangedEventArgs e)
        {
            int iCurrentIndex = this.SimulationSubject.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr =subject_table.Rows[iCurrentIndex];
            subjectID = int.Parse(dr[0].ToString());
            subjectName = dr["subject_name"].ToString();
            string sql = "select count(*)from single_question where ques_subject=" + subjectID;
            singlenum = db_connect.getcount(sql);
            SingleMaxNum.Content = "共有" + singlenum + "题";
            sql = "select count(*)from bank_question where ques_subject=" + subjectID;
            banknum = db_connect.getcount(sql);
            BankMaxNum.Content = "共有" + banknum + "题";

        }
        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            try
            {

          
            if (student_main.image != null)
            {
                string single_num = SingleNum.Text;
                if (single_num.Equals(""))
                {
                    System.Windows.MessageBox.Show("请输入选择题数量");
                    return;
                }
                int singleNum = int.Parse(single_num);
                if (singleNum > singlenum)
                {
                    MessageBox.Show("输入选择题数量不能大于总共数量");
                    return;
                }
                string bank_num = BankNum.Text;
                if (bank_num.Equals(""))
                {
                    System.Windows.MessageBox.Show("请输入填空题数量");
                    return; 
                }
                int bankNum = int.Parse(bank_num);
                if (bankNum > banknum)
                {
                    MessageBox.Show("输入填空题数量不能大于总共数量");
                    return;
                }
                if (SimulationTime.Text.Equals(""))
                {
                    System.Windows.MessageBox.Show("请输入考试时间");
                    return;
                }
                int exam_time = int.Parse(SimulationTime.Text);

                String sql_single = "Select * from single_question where ques_subject= " + subjectID + " order by rand() limit " + single_num;
                String sql_bank = "Select * from bank_question  where ques_subject= " + subjectID + " order by rand() limit " + bank_num;

                Exam startanswer = new Exam(subjectID, subjectName, sql_single, sql_bank, exam_time, 1, 1,"simulation");
                startanswer.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先上传你的完整照片");
            }
            }
            catch (Exception)
            {

                MessageBox.Show("确认输入的为整数");
            }
        }
        
           
            }
    }

