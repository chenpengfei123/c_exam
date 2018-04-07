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
using WpfApp1.Blank;

namespace WpfApp1.Control
{
    /// <summary>
    /// QuestionManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class QuestionManagerControl : UserControl
    {
        int subject;
        string sql_single;
        string sql_blank;
        DataTable dataTable_single;
        DataTable dataTable_blank;
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

            sql_blank = "select ques_id,ques_name,ques_answer,ques_explain from blank_question where ques_subject=" + subject;
            ShowBlankQuestion();
        }
        private void ShowSingleQuestion()
        {
            dataTable_single = db_connect.GetTables(sql_single);
            dataTable_single.Columns[0].ColumnName = "题目编号";
            dataTable_single.Columns[1].ColumnName = "题目名称";
            dataTable_single.Columns[2].ColumnName = "选项A";
            dataTable_single.Columns[3].ColumnName = "选项B";
            dataTable_single.Columns[4].ColumnName = "选项C";
            dataTable_single.Columns[5].ColumnName = "选项D";
            dataTable_single.Columns[6].ColumnName = "正确选项";
            dataTable_single.Columns[7].ColumnName = "答案解析";
            single_manager.ItemsSource = dataTable_single.DefaultView;
        }

        private void ShowBlankQuestion()
        {
            dataTable_blank = db_connect.GetTables(sql_blank);
            dataTable_blank.Columns[0].ColumnName = "题目编号";
            dataTable_blank.Columns[1].ColumnName = "题目名称";
            dataTable_blank.Columns[2].ColumnName = "正确答案";
            dataTable_blank.Columns[3].ColumnName = "答案解析";
            blank_manager.ItemsSource = dataTable_blank.DefaultView;
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
            int id = single_manager.SelectedIndex;
            if (id>=0)
            {
            DataRow dataRow = dataTable_single.Rows[id];
            string singleId = dataRow["题目编号"].ToString();
            ChangeSingleQuestion changeSingleQuestion = new ChangeSingleQuestion(singleId);
            changeSingleQuestion.Owner = Window.GetWindow(this);
            changeSingleQuestion.Show();

            }
            else
            {
                ChangeSingleQuestion changeSingleQuestion = new ChangeSingleQuestion();
                changeSingleQuestion.Owner = Window.GetWindow(this);
                changeSingleQuestion.Show();
            }

        }

        private void DeleteSingle_Click(object sender, RoutedEventArgs e)
        {
            int id = single_manager.SelectedIndex;
            if (id > 0)
            {
                DataRow dataRow = dataTable_single.Rows[id];
                string singleId = dataRow["题目编号"].ToString();
                DeleteSingleQuestion deleteSingleQuestion = new DeleteSingleQuestion(singleId);
                deleteSingleQuestion.Owner = Window.GetWindow(this);
                deleteSingleQuestion.Show();

            }
            else
            {
                DeleteSingleQuestion deleteSingleQuestion = new DeleteSingleQuestion();
                deleteSingleQuestion.Owner = Window.GetWindow(this);
                deleteSingleQuestion.Show();
            }
        }

        private void AddBlank_Click(object sender, RoutedEventArgs e)
        {

            AddBlankQuestion addBlankQuestion = new AddBlankQuestion(subject);
            addBlankQuestion.Owner = Window.GetWindow(this);
            addBlankQuestion.Show();
        }

        private void ChangeBlank_Click(object sender, RoutedEventArgs e)
        {

            int id = blank_manager.SelectedIndex;
            if (id >= 0)
            {
                DataRow dataRow = dataTable_blank.Rows[id];
                string blankId = dataRow["题目编号"].ToString();
                ChangeBlankQuestion changeBlankQuestion = new ChangeBlankQuestion(blankId);
                changeBlankQuestion.Owner = Window.GetWindow(this);
                changeBlankQuestion.Show();

            }
            else
            {
                ChangeBlankQuestion changeBlankQuestion = new ChangeBlankQuestion();
                changeBlankQuestion.Owner = Window.GetWindow(this);
                changeBlankQuestion.Show();
            }




          
        }

        private void DeleteBlank_Click(object sender, RoutedEventArgs e)
        {

            int id = blank_manager.SelectedIndex;
            if (id >= 0)
            {
                DataRow dataRow = dataTable_blank.Rows[id];
                string blankId = dataRow["题目编号"].ToString();
                DeleteBlankQuestion deleteBlankQuestion = new DeleteBlankQuestion(blankId);
                deleteBlankQuestion.Owner = Window.GetWindow(this);
                deleteBlankQuestion.Show();

            }
            else
            {
                DeleteBlankQuestion deleteBlankQuestion = new DeleteBlankQuestion();
                deleteBlankQuestion.Owner = Window.GetWindow(this);
                deleteBlankQuestion.Show();
            }


       
        }

        private void RefreshBlank_Click(object sender, RoutedEventArgs e)
        {
            ShowBlankQuestion();
        }


    }
}
