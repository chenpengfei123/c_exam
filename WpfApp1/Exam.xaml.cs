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
        int bank_id = 0;
        int count_single;
        int count_bank;
        int single_question_id = 0;
        int bank_question_id = 0;
       static int single_score;
       static  int bank_score;
        public  static  int ID; 
       public static  DataSet dataSet;
        public static  string IsExam;
        public   static   DataTable single_answer;
        public static DataTable bank_answer1;
        public Exam(int  ID,string Name, string sql_single, string sql_bank,int time,int single_score,int bank_score,string IsExam)
        {
            InitializeComponent();
            Exam.single_score = single_score;
            Exam.bank_score = bank_score;
            Exam.IsExam = IsExam;
            Exam.ID = ID;
            CameraHelper.CameraInit(player);
            Init(sql_single,sql_bank);
            countdown = new CountDown(endtime, this, user_message, time, single_score, bank_score);
            SubjectName.Content = Name;
        }


        private void Init(string sql_single, string sql_bank)
        {
            single_answer = new DataTable();
            bank_answer1 = new DataTable();
            single_answer.Columns.Add("ques_id");
            single_answer.Columns.Add("stu_id");
            single_answer.Columns.Add("stu_answer");
            single_answer.Columns.Add("ID");
            single_answer.Columns.Add("time");
            single_answer.PrimaryKey = new DataColumn[] { single_answer.Columns["ques_id"] };
            bank_answer1.Columns.Add("ques_id");
            bank_answer1.Columns.Add("stu_id");
            bank_answer1.Columns.Add("stu_answer");
            bank_answer1.Columns.Add("ID");
            bank_answer1.Columns.Add("time");
            bank_answer1.PrimaryKey = new DataColumn[] { bank_answer1.Columns["ques_id"] };


            dataSet = new DataSet();
           

            DataTable single_question = db_connect.GetTables(sql_single);
            DataTable bank_question = db_connect.GetTables(sql_bank);
            single_question.TableName = "single";
            single_question.PrimaryKey = new DataColumn[] { single_question.Columns["ques_id"] };
            bank_question.TableName = "bank";
            bank_question.PrimaryKey = new DataColumn[] { bank_question.Columns["bank_id"] };

            dataSet.Tables.Add(single_question);
            dataSet.Tables.Add(bank_question);

            count_single = dataSet.Tables["single"].Rows.Count;
            count_bank = dataSet.Tables["bank"].Rows.Count;

            for (int i = 1; i <= count_single; i++)
            {
                SinglePaper.Items.Add(i);

            }
            for (int i = 1; i <= count_bank; i++)
            {
                BankPaper.Items.Add(i);
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
            if (count_bank > 0)
            {
                BankPaper.SelectedIndex = bank_id;
            }
            else
            {
                Bank_Next.IsEnabled = false;
                Bank_Back.IsEnabled = false;
                this.bank_question.Text = "没有填空题";
            }
            user_message.Text = "欢迎你，" + BaiduAI.username;
            progressbar_single.Maximum = count_single;//设置最大长度值
            progress_bank.Maximum = count_bank;
            progressbar_single.Value = 0;//设置当前值
            progress_bank.Value = 0;
            finish_single.Content = "已完成0/" + count_single + "题";
            finish_bank.Content = "已完成0/" + count_bank + "题";
           

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
      
        private void Set_BankQuestion(int j) {
            if (j == count_bank - 1)
            {
                Bank_Next.IsEnabled = false;
            }
            else
            {
                Bank_Next.IsEnabled = true;
            }
            if (j == 0)
            {
                Bank_Back.IsEnabled = false;
            }
            else
            {
                Bank_Back.IsEnabled = true;
            }
            bank_question.Text = j+1 + " 、 " + dataSet.Tables["bank"].Rows[single_id]["ques_name"];
            bank_question_id = (int)dataSet.Tables["bank"].Rows[j]["bank_id"];
            if (bank_answer1.Rows.Contains(bank_question_id))
            {
                DataRow dr = bank_answer1.Rows.Find(bank_question_id);

                bank_answer.Text = (string)dr["stu_answer"];
                SaveBankAnswer.Content = "已保存";
            }
            else
            {
                bank_answer.Text = "";
            SaveBankAnswer.Content = "保存";
            }
        }

        private void save_BankAnswer() {
            if (!String.IsNullOrEmpty(bank_answer.Text.Trim( ))&count_bank>0)
            {
                DataRow dataRow = bank_answer1.NewRow();
                dataRow["ques_id"] = bank_question_id;
                dataRow["stu_id"] = BaiduAI.userid;
                dataRow["stu_answer"] = bank_answer.Text.Trim();
                dataRow["ID"] = ID;
                dataRow["time"] = DateTime.Now.ToString();
                if (bank_answer1.Rows.Contains(bank_question_id))
                {
                    DataRow DeleteRow = bank_answer1.Rows.Find(bank_question_id);
                    bank_answer1.Rows.Remove(DeleteRow);
                }
                bank_answer1.Rows.InsertAt(dataRow, bank_id);

                ListBoxItem listBoxItem = (ListBoxItem)BankPaper.ItemContainerGenerator.ContainerFromIndex(bank_id);
                listBoxItem.Background = Brushes.LightGreen;
                SaveBankAnswer.Content = "保存成功";
                ProgressBank();
            }
            return;
        }

  
        private void ProgressSingle()
        {
            int j = single_answer.Rows.Count;
            progressbar_single.Value = j;
            finish_single.Content = "已完成" + j + "/" + count_single + "题";
        }

        private void ProgressBank() {
            int j =bank_answer1.Rows.Count;       
            progress_bank.Value = j;
            finish_bank.Content = "已完成" + j + "/" + count_bank + "题";
        }

        private void Single_next_Click(object sender, RoutedEventArgs e)
        {
       
            SinglePaper.SelectedIndex = ++single_id;
        }

        private void Single_back_Click(object sender, RoutedEventArgs e)
        {

            SinglePaper.SelectedIndex = --single_id;
        }
   
        private void Bank_back_Click(object sender, RoutedEventArgs e)
        {
           BankPaper.SelectedIndex = --bank_id;
        
         
        }

        private void Bank_next_Click(object sender, RoutedEventArgs e)
        {


            BankPaper.SelectedIndex = ++bank_id;

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认提交?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                face2 = CameraHelper.CaptureImage();
                SubmitAnswer();
                if (IsExam.Equals("exam"))
                {
                    countdown.GetScores();

                }
                else
                {
                    GetScores();
                    ShowAnswer answer = new ShowAnswer();
                    answer.Show();

                }
                this.Close();

            }
        }

        public static void SubmitAnswer()
        {
            string addsingleanswer;
            string addbankanswer;
            if (IsExam.Equals("exam"))
            {
                 addsingleanswer = "replace into exam_single_answer(ques_id, stu_id, stu_answer, exam_id, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
                 addbankanswer = "replace into exam_bank_answer(ques_id, stu_id, stu_answer, exam_id, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
            }
            else
            {
                addsingleanswer = "replace into practice_single_answer(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
                addbankanswer = "replace into practice_bank_answer(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_id, @stu_answer, @ID, @time)";
            }
            mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@ques_id", MySqlDbType.Int32, 25, "ques_id"),
                     new MySqlParameter("@stu_id", MySqlDbType.VarChar, 25, "stu_id"),
                      new MySqlParameter("@stu_answer", MySqlDbType.VarChar, 25, "stu_answer"),
                     new MySqlParameter("@ID", MySqlDbType.VarChar, 25, "ID"),
                     new MySqlParameter("@time", MySqlDbType.DateTime, 255, "time"),
                };

            db_connect.AddAnswer(addsingleanswer, single_answer, mySqlParameter);
            db_connect.AddAnswer(addbankanswer, bank_answer1, mySqlParameter);
           
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

        private void BankPaper_Changed(object sender, SelectionChangedEventArgs e)
        {
            bank_id = this.BankPaper.SelectedIndex;
            Set_BankQuestion(bank_id);

        }

        private void SaveBankAnswer_Click(object sender, RoutedEventArgs e)
        {
            save_BankAnswer();
        }


        public static  void GetScores()
        {
            if (IsExam.Equals("simulation"))
            {
                int single_count = 0; 
                int bank_count = 0; 

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
                for (int i = 0; i < dataSet.Tables["bank"].Rows.Count; i++)
                {
                    string answer = dataSet.Tables["bank"].Rows[i]["ques_answer"].ToString();
                    string id = dataSet.Tables["bank"].Rows[i]["bank_id"].ToString();
                    if (bank_answer1.Rows.Contains(id))
                    {
                        DataRow Row = bank_answer1.Rows.Find(id);
                        if (Row["stu_answer"].Equals(answer))
                        {
                            bank_count++;
                        }
                    }
                }
                System.Windows.MessageBox.Show("选择题你答对了" + single_count * single_score + "题。\n填空题你答对了" + bank_count * bank_score + "题。"  );

             
            }
        }



    }
}
