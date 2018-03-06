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
            SingleMaxNum.Content = "共有" + db_connect.getcount(sql) + "题";
            sql = "select count(*)from bank_question where ques_subject=" + subjectID;
            BankMaxNum.Content = "共有" + db_connect.getcount(sql) + "题";

        }
        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {

            string single_num = SingleNum.Text;
            if (single_num.Equals(""))
            {
                System.Windows.MessageBox.Show("请输入选择题数量");
                return;
            }
            string bank_num = BankNum.Text;
            if (bank_num.Equals(""))
            {
                System.Windows.MessageBox.Show("请输入填空题数量");
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

            Single startanswer = new Single(subjectID,subjectName, sql_single, sql_bank, exam_time, 1, 1);
            startanswer.ShowDialog();
          
        }
        
           
            }
    }

