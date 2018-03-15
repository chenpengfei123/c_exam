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
using WpfApp1.Bank;

namespace WpfApp1.Control
{
    /// <summary>
    /// QuestionManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class QuestionManagerControl : UserControl
    {
        int subject;
        string sql_single;
        string sql_bank;
        DataTable dataTable;
        string sql_subject;
        DataTable subject_table;
        public QuestionManagerControl()
        {
            InitializeComponent();
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            myComboxBox.ItemsSource = subject_table.DefaultView;
            myComboxBox.DisplayMemberPath = "subject_name";
            myComboxBox.SelectedIndex = 0;
        }
        private void myComboxBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int iCurrentIndex = this.myComboxBox.SelectedIndex;
            if (iCurrentIndex < 0) return;
            DataRow dr = subject_table.Rows[iCurrentIndex];
            subject = int.Parse(dr[0].ToString());
            sql_single = "select  ques_id,ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer,ques_explain from single_question where ques_subject=" + subject;
            ShowSingleQuestion();

            sql_bank = "select bank_id,ques_name,ques_answer,ques_explain from bank_question where ques_subject=" + subject;
            ShowBankQuestion();
        }
        private void ShowSingleQuestion()
        {
            dataTable = db_connect.GetTables(sql_single);
            dataTable.Columns[0].ColumnName = "题目编号";
            dataTable.Columns[1].ColumnName = "题目名称";
            dataTable.Columns[2].ColumnName = "选项A";
            dataTable.Columns[3].ColumnName = "选项B";
            dataTable.Columns[4].ColumnName = "选项C";
            dataTable.Columns[5].ColumnName = "选项D";
            dataTable.Columns[6].ColumnName = "正确选项";
            dataTable.Columns[7].ColumnName = "答案解析";
            single_manager.ItemsSource = dataTable.DefaultView;
        }

        private void ShowBankQuestion()
        {
            dataTable = db_connect.GetTables(sql_bank);
            dataTable.Columns[0].ColumnName = "题目编号";
            dataTable.Columns[1].ColumnName = "题目名称";
            dataTable.Columns[2].ColumnName = "正确答案";
            dataTable.Columns[3].ColumnName = "答案解析";
            bank_manager.ItemsSource = dataTable.DefaultView;
        }

        private void AddSingle_Click(object sender, RoutedEventArgs e)
        {

            AddSingleQuestion addSingleQuestion = new AddSingleQuestion(subject);        
           addSingleQuestion.Owner = Window.GetWindow(this) ;
            addSingleQuestion.Show();
        }

        private void RefreshSingle_Click(object sender, RoutedEventArgs e)
        {
            ShowSingleQuestion();
        }

        private void ChangeSingle_Click(object sender, RoutedEventArgs e)
        {
            ChangeSingleQuestion changeSingleQuestion = new ChangeSingleQuestion();
            changeSingleQuestion.Owner = Window.GetWindow(this);
            changeSingleQuestion.Show();


        }

        private void DeleteSingle_Click(object sender, RoutedEventArgs e)
        {
            DeleteSingleQuestion deleteSingleQuestion = new DeleteSingleQuestion();
            deleteSingleQuestion.Owner = Window.GetWindow(this);
            deleteSingleQuestion.Show();
        }

        private void AddBank_Click(object sender, RoutedEventArgs e)
        {

            AddBankQuestion addBankQuestion = new AddBankQuestion(subject);
            addBankQuestion.Owner = Window.GetWindow(this);
            addBankQuestion.Show();
        }

        private void ChangeBank_Click(object sender, RoutedEventArgs e)
        {
            ChangeBankQuestion changeBankQuestion = new ChangeBankQuestion();
            changeBankQuestion.Owner = Window.GetWindow(this);
            changeBankQuestion.Show();
        }

        private void DeleteBank_Click(object sender, RoutedEventArgs e)
        {
            DeleteBankQuestion deleteBankQuestion = new DeleteBankQuestion();
            deleteBankQuestion.Owner = Window.GetWindow(this);
            deleteBankQuestion.Show();
        }

        private void RefreshBank_Click(object sender, RoutedEventArgs e)
        {
            ShowBankQuestion();
        }


    }
}
