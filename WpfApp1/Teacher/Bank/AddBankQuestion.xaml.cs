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
using System.Windows.Shapes;

namespace WpfApp1.Blank
{
    /// <summary>
    /// AddBlankQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class AddBlankQuestion : Window
    {
        MySqlParameter[] mySqlParameter;
        int subject;
        string sql_subject;
        DataTable subject_table;
        public AddBlankQuestion(int subject)
        {
            InitializeComponent();
            this.subject = subject;
            sql_subject = "select  * from subject";
            subject_table = db_connect.GetTables(sql_subject);
            subject_table.PrimaryKey = new DataColumn[] { subject_table.Columns["subject_id"] };
            QuestionSubject.ItemsSource = subject_table.DefaultView;
            QuestionSubject.DisplayMemberPath = "subject_name";
            QuestionSubject.SelectedIndex = subject_table.Rows.IndexOf(subject_table.Rows.Find(subject));

        }

        private void add_blank_Click(object sender, RoutedEventArgs e)
        {
            subject = (int)subject_table.Rows[QuestionSubject.SelectedIndex]["subject_id"];
            if ( string.IsNullOrEmpty(blank_name.Text) | string.IsNullOrEmpty(blank_answer.Text))
            {
                System.Windows.MessageBox.Show("请确认输入了所有信息");
                return;
            }
            String sql = "insert into blank_question(ques_name,ques_answer,ques_subject,ques_explain) values(@ques_name,@ques_answer,@ques_subject,@ques_explain)";

            mySqlParameter = new MySqlParameter[] {
                       new MySqlParameter("@ques_name",blank_name.Text),
                    new MySqlParameter("@ques_answer",blank_answer.Text ),    
                           new MySqlParameter("@ques_subject",subject ),
                            new MySqlParameter("@ques_explain",Explain.Text )
                };
            int i = db_connect.AddNonQuery(sql, mySqlParameter);
            if (i > 0)
            {
                MessageBox.Show("添加成功");
            }
            else
            {
                MessageBox.Show("添加失败");
            }
        }

        private void Reset_blank_Click(object sender, RoutedEventArgs e)
        {
            blank_name.Text = "";
            blank_answer.Text = "";
            Explain.Text = "";
        }
    }
}
