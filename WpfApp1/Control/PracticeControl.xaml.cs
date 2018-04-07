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
    /// PracticeControl.xaml 的交互逻辑
    /// </summary>
    public partial class PracticeControl : UserControl
    {
        int singlenum;
        int blanknum;
        int subjectID;
        public  string subjectName;
        string sql_subject;
        DataTable subject_table;
        public PracticeControl()
        {
            InitializeComponent();
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            PracticeSubject.ItemsSource = subject_table.DefaultView;
            PracticeSubject.DisplayMemberPath = "subject_name";
            PracticeSubject.SelectedIndex = 0;
        }

        private void PracticeSubject_Change(object sender, SelectionChangedEventArgs e)
        {
            int iCurrentIndex = this.PracticeSubject.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr = subject_table.Rows[iCurrentIndex];
            subjectID = int.Parse(dr[0].ToString());
            subjectName = dr["subject_name"].ToString();
            string sql = "select count(*)from single_question where ques_subject=" + subjectID;

            singlenum = db_connect.getcount(sql);

            SingleMaxNum.Content = "共有" + singlenum + "题";
            sql = "select count(*)from blank_question where ques_subject=" + subjectID;
            blanknum = db_connect.getcount(sql);
            BlankMaxNum.Content = "共有" + blanknum + "题";
        }

        private void StartPractice_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           
        
            string single_num = SingleNum.Text;
            if (String.IsNullOrEmpty(single_num))
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

            string blank_num = BlankNum.Text;
            if (String.IsNullOrEmpty(blank_num))
            {
                System.Windows.MessageBox.Show("请输入填空题数量");
                return;
            }
            int blankNum = int.Parse(blank_num);
            if (blankNum > blanknum)
            {
                MessageBox.Show("输入填空题数量不能大于总共数量");
                return;
            }
            bool isOrder = (bool)IsOrder.IsChecked;
            
            String sql_single = "Select * from single_question where ques_subject= " + subjectID + (isOrder? " order by rand()":" " )+ " limit " + single_num;
            String sql_blank = "Select * from blank_question  where ques_subject= " + subjectID + (isOrder ? " order by rand()" : " ")+" limit " + blank_num;
      
            Practice practice = new Practice(subjectID,subjectName, sql_single, sql_blank);
            practice.Show();

            }
            catch (Exception)
            {

                MessageBox.Show("确认输入的为整数");
            }
        }
    }
}
