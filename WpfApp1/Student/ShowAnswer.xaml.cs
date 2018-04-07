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

namespace WpfApp1
{
    /// <summary>
    /// ShowAnswer.xaml 的交互逻辑
    /// </summary>
    public partial class ShowAnswer : Window
    {
        public ShowAnswer()
        {
            InitializeComponent();
            InitSingleAnswer();
            InitBlankAnswer();
        }
        private void InitBlankAnswer()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("是否正确", typeof(string));
            dataTable.Columns.Add("编号", typeof(Int32));
            dataTable.Columns.Add("标准答案", typeof(string));
            dataTable.Columns.Add("你的答案", typeof(string));
            for (int i = 0; i < Exam.dataSet.Tables["blank"].Rows.Count; i++)

            {
                string answer = Exam.dataSet.Tables["blank"].Rows[i]["ques_answer"].ToString();
                string id = Exam.dataSet.Tables["blank"].Rows[i]["ques_id"].ToString();
                if (Exam.blank_answer1.Rows.Contains(id))
                {
                    DataRow Row = Exam.blank_answer1.Rows.Find(id);
                    if (Row["stu_answer"].Equals(answer))
                    {
                        dataTable.Rows.Add("✔", i + 1, answer, Row["stu_answer"]);
                    }
                    else
                    {
                        dataTable.Rows.Add("✘", i + 1, answer, Row["stu_answer"]);
                    }
                }
                else
                {
                    dataTable.Rows.Add("✘", i + 1, answer, "");
                }
            }
            Blank_ID.ItemsSource = dataTable.DefaultView;

        }
        private void InitSingleAnswer()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("是否正确", typeof(string));
            dataTable.Columns.Add("编号", typeof(Int32));
            dataTable.Columns.Add("标准答案", typeof(string));
            dataTable.Columns.Add("你的答案", typeof(string));
            for (int i = 0; i < Exam.dataSet.Tables["single"].Rows.Count; i++)

            {
                string answer = Exam.dataSet.Tables["single"].Rows[i]["ques_answer"].ToString();
                string id = Exam.dataSet.Tables["single"].Rows[i]["ques_id"].ToString();
                if (Exam.single_answer.Rows.Contains(id))
                {
                    DataRow Row = Exam.single_answer.Rows.Find(id);
                    if (Row["stu_answer"].Equals(answer))
                    {
                        dataTable.Rows.Add("✔", i + 1, answer, Row["stu_answer"]);
                    }
                    else
                    {
                        dataTable.Rows.Add("✘", i + 1, answer, Row["stu_answer"]);
                    }
                }
                else
                {
                    dataTable.Rows.Add("✘", i + 1, answer, "");
                }
            }
            Single_ID.ItemsSource = dataTable.DefaultView;
        }

        private void SelectSingle_ID_Click(object sender, SelectionChangedEventArgs e)
        {
           
            int id = Single_ID.SelectedIndex;

           DataRow dataRow = Exam.dataSet.Tables["single"].Rows[id];
            Single_Name.Text = id+1+"、"+ dataRow["ques_name"].ToString()+"\n"+ dataRow["ques_answerA"].ToString()+ "\n" + dataRow["ques_answerB"].ToString() + "\n" + dataRow["ques_answerC"].ToString() + "\n" + dataRow["ques_answerD"].ToString();

        
            string explain = !dataRow.IsNull("ques_explain") ? dataRow["ques_explain"].ToString() : "暂无解析";
            Single_Answer.Text = "答案：" + dataRow["ques_answer"].ToString() + "\n解析：" + explain;
        }

        private void SelectBlank_ID_Click(object sender, SelectionChangedEventArgs e)
        {
            int id = Blank_ID.SelectedIndex;

            DataRow dataRow = Exam.dataSet.Tables["blank"].Rows[id];
           Blank_Name.Text = id + 1 + "、" + dataRow["ques_name"].ToString();


            string explain = !dataRow.IsNull("ques_explain") ? dataRow["ques_explain"].ToString() : "暂无解析";
           Blank_Answer.Text = "答案：" + dataRow["ques_answer"].ToString() + "\n解析：" + explain;
        }
    }
    }

