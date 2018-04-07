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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Practice.xaml 的交互逻辑
    /// </summary>
    public partial class Practice : Window
    {
        static MySqlParameter[] mySqlParameter;
        int single_iscollection;
        int blank_iscollection;
        int single_id = 0;
        int blank_id = 0;
        int count_single;
        int count_blank;
        int single_question_id = 0;
        int blank_question_id = 0;
        public int subject;
        DataSet dataSet;
        public DataTable single_answer;
        public DataTable blank_answer1;
        public Practice(int subjectID, string subjectname, string sql_single, string sql_blank)
        {

            subject = subjectID;
            InitializeComponent();



            InitDataTable(sql_single, sql_blank);

            count_single = dataSet.Tables["single"].Rows.Count;
            count_blank = dataSet.Tables["blank"].Rows.Count;

            for (int i = 1; i <= count_single; i++)
            {
                SinglePaper.Items.Add(i);

            }
            for (int i = 1; i <= count_blank; i++)
            {
                BlankPaper.Items.Add(i);
            }

            if (count_single > 0)
            {
                SinglePaper.SelectedIndex = single_id;
            }
            else
            {
                single_name.Text = "没有选择题";
                single_answerA.IsEnabled = false;
                single_answerB.IsEnabled = false;
                single_answerC.IsEnabled = false;
                single_answerD.IsEnabled = false;
                Single_next.IsEnabled = false;
                Single_back.IsEnabled = false;
                AddSingleCollection.IsEnabled = false;
                ShowSingleAnswer.IsEnabled = false;
            }
            if (count_blank > 0)
            {
                BlankPaper.SelectedIndex = blank_id;
            }
            else
            {
                ShowBlankAnswer.IsEnabled = false;
                AddBlankCollection.IsEnabled = false;
                SaveBlankAnswer.IsEnabled = false;
                Blank_Next.IsEnabled = false;
                Blank_Back.IsEnabled = false;
                this.blank_question.Text = "没有填空题";
            }
            user_message.Content = "欢迎你，" + BaiduAI.username;

            progressbar_single.Maximum = count_single;//设置最大长度值
            progress_blank.Maximum = count_blank;
            progressbar_single.Value = 0;//设置当前值
            progress_blank.Value = 0;
            finish_single.Content = "已完成0/" + count_single + "题";
            finish_blank.Content = "已完成0/" + count_blank + "题";
            SubjectName.Content = subjectname;
        }

        private void InitDataTable(string sql_single, string sql_blank)
        {
            single_answer = new DataTable();
            blank_answer1 = new DataTable();
            single_answer.Columns.Add("question_id");
            single_answer.Columns.Add("userid");
            single_answer.Columns.Add("answer");
            single_answer.Columns.Add("subject");
            single_answer.Columns.Add("time");
            single_answer.PrimaryKey = new DataColumn[] { single_answer.Columns["question_id"] };
            blank_answer1.Columns.Add("question_id");
            blank_answer1.Columns.Add("userid");
            blank_answer1.Columns.Add("answer");
            blank_answer1.Columns.Add("subject");
            blank_answer1.Columns.Add("time");
            blank_answer1.PrimaryKey = new DataColumn[] { blank_answer1.Columns["question_id"] };


            dataSet = new DataSet();


            DataTable single_question = db_connect.GetTables(sql_single);
            DataTable blank_question = db_connect.GetTables(sql_blank);
            single_question.TableName = "single";
            single_question.PrimaryKey = new DataColumn[] { single_question.Columns["ques_id"] };
            blank_question.TableName = "blank";
            blank_question.PrimaryKey = new DataColumn[] { blank_question.Columns["ques_id"] };

            dataSet.Tables.Add(single_question);
            dataSet.Tables.Add(blank_question);
        }

        private void Set_SingleQuestion(int i)
        {
            if (i == count_single - 1)
            {
                Single_next.IsEnabled = false;
            }
            else
            {
                Single_next.IsEnabled = true;
            }
            if (i == 0)
            {
                Single_back.IsEnabled = false;
            }
            else
            {
                Single_back.IsEnabled = true;
            }

            single_name.Text = i + 1 + " 、 " + dataSet.Tables["single"].Rows[i]["ques_name"];
            single_answerA.Content = dataSet.Tables["single"].Rows[i]["ques_answerA"];
            single_answerB.Content = dataSet.Tables["single"].Rows[i]["ques_answerB"];
            single_answerC.Content = dataSet.Tables["single"].Rows[i]["ques_answerC"];
            single_answerD.Content = dataSet.Tables["single"].Rows[i]["ques_answerD"];
            single_question_id = (int)dataSet.Tables["single"].Rows[i]["ques_id"];

            if (single_answer.Rows.Contains(single_question_id))
            {

                DataRow dr = single_answer.Rows.Find(single_question_id);

                switch (dr["answer"])
                {
                    case "A":

                        single_answerA.IsChecked = true;
                        break;

                    case "B":
                        single_answerB.IsChecked = true;
                        break;
                    case "C":
                        single_answerC.IsChecked = true;
                        break;
                    case "D":
                        single_answerD.IsChecked = true;
                        break;
                    default:
                        single_answerA.IsChecked = false;
                        single_answerB.IsChecked = false;
                        single_answerC.IsChecked = false;
                        single_answerD.IsChecked = false;
                        return;
                }
            }
            else
            {
                SingleAnswer.Text = "";
                single_answerA.IsChecked = false;
                single_answerB.IsChecked = false;
                single_answerC.IsChecked = false;
                single_answerD.IsChecked = false;
            }
            string sql_addcollection = "select count(*) from single_collection where stu_id =@stu_id and ques_id=@ques_id";
            mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@ques_id",single_question_id),
                    new MySqlParameter("@stu_id", BaiduAI.userid)

            };
            single_iscollection = db_connect.getcount(sql_addcollection, mySqlParameter);
            if (single_iscollection != 0)
            {
                AddSingleCollection.Content = "取消收藏";
            }
            else
            {
                AddSingleCollection.Content = "添加收藏";
            }


        }

        private void SaveSingleAnswer(string answer)
        {
            DataRow dataRow = single_answer.NewRow();

            dataRow["question_id"] = single_question_id;
            dataRow["userid"] = BaiduAI.userid;
            dataRow["answer"] = answer;
            dataRow["subject"] = subject;
            dataRow["time"] = DateTime.Now.ToString();
            if (single_answer.Rows.Contains(single_question_id))
            {
                DataRow DeleteRow = single_answer.Rows.Find(single_question_id);
                single_answer.Rows.Remove(DeleteRow);

            }
            single_answer.Rows.InsertAt(dataRow, single_id);



            if (dataSet.Tables["single"].Rows[single_id]["ques_answer"].Equals(answer))
            {
                ListBoxItem listBoxItem = (ListBoxItem)SinglePaper.ItemContainerGenerator.ContainerFromIndex(single_id);
                listBoxItem.Background = Brushes.LightGreen;
            }
            else
            {
                ListBoxItem listBoxItem = (ListBoxItem)SinglePaper.ItemContainerGenerator.ContainerFromIndex(single_id);
                listBoxItem.Background = Brushes.LightPink;

            }



            ProgressSingle();
            if ((bool)AutoSingleAnswer.IsChecked)
            {

                showSingleAnswer();
            }

        }

        private void single_answerA_Checked(object sender, RoutedEventArgs e)
        {
            SaveSingleAnswer("A");
        }

        private void single_answerB_Checked(object sender, RoutedEventArgs e)
        {

            SaveSingleAnswer("B");
        }
        private void single_answerC_Checked(object sender, RoutedEventArgs e)

        {

            SaveSingleAnswer("C");
        }
        private void single_answerD_Checked(object sender, RoutedEventArgs e)
        {

            SaveSingleAnswer("D");
        }

        private void Set_BlankQuestion(int j)
        {
            if (j == count_blank - 1)
            {
                Blank_Next.IsEnabled = false;
            }
            else
            {
                Blank_Next.IsEnabled = true;
            }
            if (j == 0)
            {
                Blank_Back.IsEnabled = false;
            }
            else
            {
                Blank_Back.IsEnabled = true;
            }
            blank_question.Text = j + 1 + " 、 " + dataSet.Tables["blank"].Rows[j]["ques_name"];
            blank_question_id = (int)dataSet.Tables["blank"].Rows[j]["ques_id"];
            if (blank_answer1.Rows.Contains(blank_question_id))
            {
                DataRow dr = blank_answer1.Rows.Find(blank_question_id);

                blank_answer.Text = (string)dr["answer"];
                SaveBlankAnswer.Content = "再次保存";
            }
            else
            {
                BlankAnswer.Text = "";
                blank_answer.Text = "";
                SaveBlankAnswer.Content = "保存";
            }

            string sql_addcollection = "select count(*) from blank_collection where stu_id =@stu_id and ques_id=@ques_id";
            mySqlParameter = new MySqlParameter[] {
                    new MySqlParameter("@ques_id",blank_question_id),
                    new MySqlParameter("@stu_id", BaiduAI.userid)

            };
            blank_iscollection = db_connect.getcount(sql_addcollection, mySqlParameter);
            if (blank_iscollection != 0)
            {
                AddBlankCollection.Content = "取消收藏";
            }
            else
            {
                AddBlankCollection.Content = "添加收藏";
            }



        }

        private void save_BlankAnswer()
        {
            if (!String.IsNullOrEmpty(blank_answer.Text.Trim()) & count_blank > 0)
            {
                DataRow dataRow = blank_answer1.NewRow();
                dataRow["question_id"] = blank_question_id;
                dataRow["userid"] = BaiduAI.userid;
                dataRow["answer"] = blank_answer.Text.Trim();
                dataRow["subject"] = subject;
                dataRow["time"] = DateTime.Now.ToString();
                if (blank_answer1.Rows.Contains(blank_question_id))
                {
                    DataRow DeleteRow = blank_answer1.Rows.Find(blank_question_id);
                    blank_answer1.Rows.Remove(DeleteRow);
                }
                blank_answer1.Rows.InsertAt(dataRow, blank_id);


                if (dataSet.Tables["blank"].Rows[blank_id]["ques_answer"].Equals(blank_answer.Text.Trim()))
                {
                    ListBoxItem listBoxItem = (ListBoxItem)BlankPaper.ItemContainerGenerator.ContainerFromIndex(blank_id);
                    listBoxItem.Background = Brushes.LightGreen;
                }
                else
                {
                    ListBoxItem listBoxItem = (ListBoxItem)BlankPaper.ItemContainerGenerator.ContainerFromIndex(blank_id);
                    listBoxItem.Background = Brushes.LightPink;

                }

                if ((bool)AutoBlankAnswer.IsChecked)
                {
                    showBlankAnswer();
                }
                SaveBlankAnswer.Content = "保存成功";
                ProgressBlank();
            }

        }

        private void ProgressSingle()
        {
            int j = single_answer.Rows.Count;
            progressbar_single.Value = j;
            finish_single.Content = "已完成" + j + "/" + count_single + "题";
        }

        private void ProgressBlank()
        {
            int j = blank_answer1.Rows.Count;
            progress_blank.Value = j;
            finish_blank.Content = "已完成" + j + "/" + count_blank + "题";
        }

        private void Single_next_Click(object sender, RoutedEventArgs e)
        {

            SinglePaper.SelectedIndex = ++single_id;
        }

        private void Single_back_Click(object sender, RoutedEventArgs e)
        {

            SinglePaper.SelectedIndex = --single_id;
        }

        private void Blank_back_Click(object sender, RoutedEventArgs e)
        {
            BlankPaper.SelectedIndex = --blank_id;


        }

        private void Blank_next_Click(object sender, RoutedEventArgs e)
        {


            BlankPaper.SelectedIndex = ++blank_id;

        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认退出练习?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                System.Windows.MessageBox.Show("退出成功");
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }



        }

        private void SinglePaper_Changed(object sender, SelectionChangedEventArgs e)
        {
            single_id = this.SinglePaper.SelectedIndex;
            Set_SingleQuestion(single_id);

        }

        private void BlankPaper_Changed(object sender, SelectionChangedEventArgs e)
        {
            blank_id = this.BlankPaper.SelectedIndex;
            Set_BlankQuestion(blank_id);

        }

        private void SaveBlankAnswer_Click(object sender, RoutedEventArgs e)
        {
            save_BlankAnswer();
        }

        private void ShowSingleAnswer_Click(object sender, RoutedEventArgs e)
        {
            showSingleAnswer();

        }

        private void showSingleAnswer()
        {
            DataRow dataRow = dataSet.Tables["single"].Rows.Find(single_question_id);
            string answer = dataRow["ques_answer"].ToString();
            string explain = !dataRow.IsNull("ques_explain") ? dataRow["ques_explain"].ToString() : "暂无解析";
            SingleAnswer.Text = "答案：" + answer + "\n解析：" + explain;
        }

        private void ShowBlankAnswer_Click(object sender, RoutedEventArgs e)
        {
            showBlankAnswer();

        }

        private void showBlankAnswer()
        {
            DataRow dataRow = dataSet.Tables["blank"].Rows.Find(blank_question_id);
            string answer = dataRow["ques_answer"].ToString();
            string explain = !dataRow.IsNull("ques_explain") ? dataRow["ques_explain"].ToString() : "暂无解析";
            BlankAnswer.Text = "答案：" + answer + "\n解析：" + explain;
        }

        private void AddSingleCollection_Click(object sender, RoutedEventArgs e)
        {


            if (single_iscollection == 0)
            {
                string sql_addcollection = "insert into  single_collection values(@stu_id,@ques_id)";
                db_connect.AddNonQuery(sql_addcollection, mySqlParameter);
                AddSingleCollection.Content = "取消收藏";
                single_iscollection = 1;
            }
            else
            {
                string sql_deletecollection = "delete from single_collection where stu_id =@stu_id and ques_id=@ques_id";
                db_connect.AddNonQuery(sql_deletecollection, mySqlParameter);
                AddSingleCollection.Content = "添加收藏";
                single_iscollection = 0;
            }
        }

        private void AddBlankCollection_Click(object sender, RoutedEventArgs e)
        {

            if (blank_iscollection == 0)
            {
                string sql_addcollection = "insert into  blank_collection values(@stu_id,@ques_id)";
                db_connect.AddNonQuery(sql_addcollection, mySqlParameter);
                AddBlankCollection.Content = "取消收藏";
                blank_iscollection = 1;
            }
            else
            {
                string sql_deletecollection = "delete from blank_collection where stu_id =@stu_id and ques_id=@ques_id";
                db_connect.AddNonQuery(sql_deletecollection, mySqlParameter);
                AddBlankCollection.Content = "添加收藏";
                blank_iscollection = 0;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }















    }
}
