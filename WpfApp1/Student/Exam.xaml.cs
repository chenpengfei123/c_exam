using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
//using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Exam : Window
    {
       static  MySqlParameter[] mySqlParameter;
        public   static   byte[] face1;
          public  static    byte[] face2;
        CountDown countdown;
        int single_id=0;
        int blank_id = 0;
        int count_single;
        int count_blank;
        int single_question_id = 0;
        int blank_question_id = 0;
       static int single_score;
       static  int blank_score;
        public  static  int ID; 
       public static  DataSet dataSet;
        public static  string IsExam;
        public   static   DataTable single_answer;
        public static DataTable blank_answer1;
        public Exam(int  ID,string Name, string sql_single, string sql_blank,int time,int single_score,int blank_score,string IsExam)
        {
            InitializeComponent();
            Exam.single_score = single_score;
            Exam.blank_score = blank_score;
            Exam.IsExam = IsExam;
            Exam.ID = ID;
            if (!CameraHelper.CameraInit(player))
            {
                this.Close();
                return;
            }
            Init(sql_single,sql_blank);
            countdown = new CountDown(endtime, this, user_message, time, single_score, blank_score);
            SubjectName.Content = Name;
        }


        private void Init(string sql_single, string sql_blank)
        {
            single_answer = new DataTable();
            blank_answer1 = new DataTable();
            single_answer.Columns.Add("ques_id");
            single_answer.Columns.Add("stu_id");
            single_answer.Columns.Add("stu_answer");
            single_answer.Columns.Add("ID");
            single_answer.Columns.Add("time");
            single_answer.PrimaryKey = new DataColumn[] { single_answer.Columns["ques_id"] };
            blank_answer1.Columns.Add("ques_id");
            blank_answer1.Columns.Add("stu_id");
            blank_answer1.Columns.Add("stu_answer");
            blank_answer1.Columns.Add("ID");
            blank_answer1.Columns.Add("time");
            blank_answer1.PrimaryKey = new DataColumn[] { blank_answer1.Columns["ques_id"] };


            dataSet = new DataSet();
           

            DataTable single_question = db_connect.GetTables(sql_single);
            DataTable blank_question = db_connect.GetTables(sql_blank);
            single_question.TableName = "single";
            single_question.PrimaryKey = new DataColumn[] { single_question.Columns["ques_id"] };
            blank_question.TableName = "blank";
            blank_question.PrimaryKey = new DataColumn[] { blank_question.Columns["ques_id"] };

            dataSet.Tables.Add(single_question);
            dataSet.Tables.Add(blank_question);

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
            }
            if (count_blank > 0)
            {
                BlankPaper.SelectedIndex = blank_id;
            }
            else
            {
                Blank_Next.IsEnabled = false;
                Blank_Back.IsEnabled = false;
                this.blank_question.Text = "没有填空题";
            }
            user_message.Text = "欢迎你，" + BaiduAI.username;
            progressbar_single.Maximum = count_single;//设置最大长度值
            progress_blank.Maximum = count_blank;
            progressbar_single.Value = 0;//设置当前值
            progress_blank.Value = 0;
            finish_single.Content = "已完成0/" + count_single + "题";
            finish_blank.Content = "已完成0/" + count_blank + "题";
           

        }

        private void Set_SingleQuestion(int i)
        {
            if (i== count_single-1)
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

            single_name.Text = i+1 +" 、 "+ dataSet.Tables["single"].Rows[i]["ques_name"];
            single_answerA.Content = dataSet.Tables["single"].Rows[i]["ques_answerA"];
            single_answerB.Content = dataSet.Tables["single"].Rows[i]["ques_answerB"];
            single_answerC.Content = dataSet.Tables["single"].Rows[i]["ques_answerC"];
            single_answerD.Content = dataSet.Tables["single"].Rows[i]["ques_answerD"];
            single_question_id = (int)dataSet.Tables["single"].Rows[i]["ques_id"];
      
            if (single_answer.Rows.Contains(single_question_id))
            {
                DataRow dr = single_answer.Rows.Find(single_question_id);

                switch (dr["stu_answer"])
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
                single_answerA.IsChecked = false;
                single_answerB.IsChecked = false;
                single_answerC.IsChecked = false;
                single_answerD.IsChecked = false;
            }
        


        }

        private void SaveSingleAnswer(string answer)
        {
            DataRow dataRow = single_answer.NewRow();

            dataRow["ques_id"] = single_question_id;
            dataRow["stu_id"] = BaiduAI.userid;
            dataRow["stu_answer"] = answer;
            dataRow["ID"] = ID;
            dataRow["time"] = DateTime.Now.ToString();
            if (single_answer.Rows.Contains(single_question_id))
            {
                DataRow DeleteRow = single_answer.Rows.Find(single_question_id);
                single_answer.Rows.Remove(DeleteRow);

            }
            single_answer.Rows.InsertAt(dataRow, single_id);

            ListBoxItem listBoxItem = (ListBoxItem)SinglePaper.ItemContainerGenerator.ContainerFromIndex(single_id);
            listBoxItem.Background = Brushes.LightGreen;
            ProgressSingle();
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
      
        private void Set_BlankQuestion(int j) {
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
            blank_question.Text = j+1 + " 、 " + dataSet.Tables["blank"].Rows[j]["ques_name"];
            blank_question_id = (int)dataSet.Tables["blank"].Rows[j]["ques_id"];
            if (blank_answer1.Rows.Contains(blank_question_id))
            {
                DataRow dr = blank_answer1.Rows.Find(blank_question_id);

                blank_answer.Text = (string)dr["stu_answer"];
                SaveBlankAnswer.Content = "已保存";
            }
            else
            {
                blank_answer.Text = "";
            SaveBlankAnswer.Content = "保存";
            }
        }

        private void save_BlankAnswer() {
            if (!String.IsNullOrEmpty(blank_answer.Text.Trim( ))&count_blank>0)
            {
                DataRow dataRow = blank_answer1.NewRow();
                dataRow["ques_id"] = blank_question_id;
                dataRow["stu_id"] = BaiduAI.userid;
                dataRow["stu_answer"] = blank_answer.Text.Trim();
                dataRow["ID"] = ID;
                dataRow["time"] = DateTime.Now.ToString();
                if (blank_answer1.Rows.Contains(blank_question_id))
                {
                    DataRow DeleteRow = blank_answer1.Rows.Find(blank_question_id);
                    blank_answer1.Rows.Remove(DeleteRow);
                }
                blank_answer1.Rows.InsertAt(dataRow, blank_id);

                ListBoxItem listBoxItem = (ListBoxItem)BlankPaper.ItemContainerGenerator.ContainerFromIndex(blank_id);
                listBoxItem.Background = Brushes.LightGreen;
                SaveBlankAnswer.Content = "保存成功";
                ProgressBlank();
            }
            return;
        }

  
        private void ProgressSingle()
        {
            int j = single_answer.Rows.Count;
            progressbar_single.Value = j;
            finish_single.Content = "已完成" + j + "/" + count_single + "题";
        }

        private void ProgressBlank() {
            int j =blank_answer1.Rows.Count;       
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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认提交?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                face2 = CameraHelper.CaptureImage();
                SubmitAnswer();
                countdown.GetExamScores();

                this.Close();

            }
        }

        public static void SubmitAnswer()
        {
            string addsingleanswer;
            string addblankanswer;
            if (IsExam.Equals("exam"))
            {
                 addsingleanswer = "replace into exam_single_answer(ques_id, stu_id, stu_answer, exam_id, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
                 addblankanswer = "replace into exam_blank_answer(ques_id, stu_id, stu_answer, exam_id, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
            }
            else
            {
                addsingleanswer = "replace into practice_single_answer(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
                addblankanswer = "replace into practice_blank_answer(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
            }
            mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@ques_id", MySqlDbType.Int32, 25, "ques_id"),
                     new MySqlParameter("@stu_id", MySqlDbType.VarChar, 25, "stu_id"),
                      new MySqlParameter("@stu_answer", MySqlDbType.VarChar, 25, "stu_answer"),
                     new MySqlParameter("@ID", MySqlDbType.VarChar, 25, "ID"),
                     new MySqlParameter("@time", MySqlDbType.DateTime, 255, "time"),
                };

            db_connect.AddAnswer(addsingleanswer, single_answer, mySqlParameter);
            db_connect.AddAnswer(addblankanswer, blank_answer1, mySqlParameter);
           
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
 
                countdown.destroyCountdown();
                CameraHelper.CloseDevice();
                e.Cancel = false;
     
          
           
            
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


        public static  void GetScores()
        {
            if (IsExam.Equals("simulation"))
            {
                int single_count = 0; 
                int blank_count = 0; 

                for (int i = 0; i < dataSet.Tables["single"].Rows.Count; i++)
                {
                   string answer= dataSet.Tables["single"].Rows[i]["ques_answer"].ToString();
                    string id = dataSet.Tables["single"].Rows[i]["ques_id"].ToString();
                    if (single_answer.Rows.Contains(id))
                    {
                        DataRow Row = single_answer.Rows.Find(id);
                        if (Row["stu_answer"].Equals(answer))
                        {
                            single_count++;
                        }
                    }
                    
                }
                for (int i = 0; i < dataSet.Tables["blank"].Rows.Count; i++)
                {
                    string answer = dataSet.Tables["blank"].Rows[i]["ques_answer"].ToString();
                    string id = dataSet.Tables["blank"].Rows[i]["ques_id"].ToString();
                    if (blank_answer1.Rows.Contains(id))
                    {
                        DataRow Row = blank_answer1.Rows.Find(id);
                        if (Row["stu_answer"].Equals(answer))
                        {
                            blank_count++;
                        }
                    }
                }
                System.Windows.MessageBox.Show("选择题你答对了" + single_count * single_score + "分。\n填空题你答对了" + blank_count * blank_score + "分。"  );

             
            }
        }



    }
}
