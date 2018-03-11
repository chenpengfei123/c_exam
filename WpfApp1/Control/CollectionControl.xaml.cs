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
    /// CollectionControl.xaml 的交互逻辑
    /// </summary>
    public partial class CollectionControl : UserControl
    {
        int singlenum;
        int banknum;
        int subjectID;
        public static string subjectName;
        string sql_subject;
        DataTable subject_table;
        public CollectionControl()
        {
            InitializeComponent();
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            CollectionSubject.ItemsSource = subject_table.DefaultView;
            CollectionSubject.DisplayMemberPath = "subject_name";
            CollectionSubject.SelectedIndex = 0;
        }

     
        private void CollectionPractice_Click(object sender, RoutedEventArgs e)
        {
            try
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

          

                bool isOrder = (bool)IsOrder.IsChecked;

          

                String sql_single = "Select * from single_question Inner join single_collection on single_collection.stu_id= '" + BaiduAI.userid + "' and single_collection.ques_id=single_question.ques_id and single_question.ques_subject= " + subjectID + (isOrder ? " order by rand()" : " ") + " limit " + single_num;
                String sql_bank = "Select * from bank_question Inner join bank_collection on bank_collection.stu_id= '" + BaiduAI.userid + "' and bank_collection.ques_id=bank_question.bank_id and bank_question.ques_subject= " + subjectID + (isOrder ? " order by rand()" : " ") + " limit " + bank_num;

                Practice practice = new Practice(subjectID, subjectName, sql_single, sql_bank);
                practice.Show();


        }
        catch (Exception)
        {

            MessageBox.Show("确认输入的为整数");
        }
        }

        private void CollectionSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int iCurrentIndex = this.CollectionSubject.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr = subject_table.Rows[iCurrentIndex];
            subjectID = int.Parse(dr[0].ToString());
            subjectName = dr["subject_name"].ToString();
            String sql_single = "Select count(*) from single_question Inner join single_collection on single_collection.stu_id= '" + BaiduAI.userid + "' and single_collection.ques_id=single_question.ques_id and single_question.ques_subject= " + subjectID;
            singlenum = db_connect.getcount(sql_single);
            SingleMaxNum.Content = "共有" + singlenum + "题";
            String sql_bank = "Select count(*) from bank_question Inner join bank_collection on bank_collection.stu_id= '" + BaiduAI.userid + "' and bank_collection.ques_id=bank_question.bank_id and bank_question.ques_subject= " + subjectID;
            banknum = db_connect.getcount(sql_bank);
            BankMaxNum.Content = "共有" + banknum + "题";
        }
    }
}
