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
        int subjectID;
        public static string subjectName;
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
            SingleMaxNum.Content = "共有" + db_connect.getcount(sql) + "题";
            sql = "select count(*)from bank_question where ques_subject=" + subjectID;
            BankMaxNum.Content = "共有" + db_connect.getcount(sql) + "题";
        }

        private void StartPractice_Click(object sender, RoutedEventArgs e)
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

            bool isOrder = (bool)IsOrder.IsChecked;
            
            String sql_single = "Select * from single_question where ques_subject= " + subjectID + (isOrder? " order by rand()":" " )+ " limit " + single_num;
            String sql_bank = "Select * from bank_question  where ques_subject= " + subjectID + (isOrder ? " order by rand()" : " ")+" limit " + bank_num;
      
            Practice practice = new Practice(subjectID,subjectName, sql_single, sql_bank);
            practice.Show();
        }
    }
}
