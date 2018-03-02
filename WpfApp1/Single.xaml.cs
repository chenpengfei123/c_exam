﻿using System;
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
    public partial class Single : Window
    {
       static  MySqlParameter[] mySqlParameter;
        public   static   byte[] face1;
          public   static   byte[] face2;
        CountDown countdown;
        int single_id=0;
        int bank_id = 0;
        int count_single;
        int count_bank;
        int single_question_id = 0;
        int bank_question_id = 0;
     public static   int subject; 
        //char[] answer_single;
        //String[] answer_bank;
        DataSet dataSet;
     public   static   DataTable single_answer;
        public static DataTable bank_answer1;
        public Single(int  subject1)
        {

            subject = subject1;
            InitializeComponent();

            CameraHelper.CameraInit(player);

            InitDataTable();

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
            countdown = new CountDown(endtime, this, user_message);
            progressbar_single.Maximum = count_single;//设置最大长度值
            progress_bank.Maximum = count_bank;
            progressbar_single.Value = 0;//设置当前值
            progress_bank.Value = 0;
            finish_single.Content = "已完成0/" + count_single + "题";
            finish_bank.Content = "已完成0/" + count_bank + "题";
            SubjectName.Content = student_main.subjectName;
        }

        private void InitDataTable()
        {
            single_answer = new DataTable();
            bank_answer1 = new DataTable();
            single_answer.Columns.Add("question_id");
            single_answer.Columns.Add("userid");
            single_answer.Columns.Add("answer");
            single_answer.Columns.Add("subject");
            single_answer.Columns.Add("time");
            single_answer.PrimaryKey = new DataColumn[] { single_answer.Columns["question_id"] };
            bank_answer1.Columns.Add("question_id");
            bank_answer1.Columns.Add("userid");
            bank_answer1.Columns.Add("answer");
            bank_answer1.Columns.Add("subject");
            bank_answer1.Columns.Add("time");
            bank_answer1.PrimaryKey = new DataColumn[] { bank_answer1.Columns["question_id"] };


            dataSet = new DataSet();
            String sql_single = "Select * from single_question where ques_subject= " + subject;
            String sql_bank = "Select * from bank_question  where ques_subject= " + subject;

            DataTable single_question = db_connect.GetTables(sql_single);
            DataTable bank_question = db_connect.GetTables(sql_bank);
            single_question.TableName = "single";
            single_question.PrimaryKey = new DataColumn[] { single_question.Columns["ques_id"] };
            bank_question.TableName = "bank";
            bank_question.PrimaryKey = new DataColumn[] { bank_question.Columns["bank_id"] };

            dataSet.Tables.Add(single_question);
            dataSet.Tables.Add(bank_question);
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
                single_answerA.IsChecked = false;
                single_answerB.IsChecked = false;
                single_answerC.IsChecked = false;
                single_answerD.IsChecked = false;
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
            DataRow dataRow = single_answer.NewRow();
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

                bank_answer.Text = (string)dr["answer"];
                SaveBankAnswer.Content = "已保存";
            }
            else
            {
                bank_answer.Text = "";
            SaveBankAnswer.Content = "保存";
            }
        }

        private void save_BankAnswer() {
            if (!bank_answer.Text.Trim( ).Equals("")&count_bank>0)
            {
                DataRow dataRow = bank_answer1.NewRow();
                dataRow["question_id"] = bank_question_id;
                dataRow["userid"] = BaiduAI.userid;
                dataRow["answer"] = bank_answer.Text.Trim();
                dataRow["subject"] = subject;
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
                countdown.GetScores();
                this.Close();

            }
        }

        public static void SubmitAnswer()
        {
            string addsingleanswer = "replace into single_answer_stu(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_id, @stu_answer, @subject, @time)";
            string addbankanswer = "replace into bank_answer_stu(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_id, @stu_answer, @subject, @time)";
            mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@ques_id", MySqlDbType.Int32, 25, "question_id"),
                     new MySqlParameter("@stu_id", MySqlDbType.VarChar, 25, "userid"),
                      new MySqlParameter("@stu_answer", MySqlDbType.VarChar, 25, "answer"),
                     new MySqlParameter("@subject", MySqlDbType.VarChar, 25, "subject"),
                     new MySqlParameter("@time", MySqlDbType.DateTime, 255, "time"),
                };

            db_connect.AddAnswer(addsingleanswer, single_answer, mySqlParameter);
            db_connect.AddAnswer(addbankanswer, bank_answer1, mySqlParameter);
           
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认退出考试?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                System.Windows.MessageBox.Show("退出成功");
                countdown.destroyCountdown();
                CameraHelper.CloseDevice();
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

        private void BankPaper_Changed(object sender, SelectionChangedEventArgs e)
        {
            bank_id = this.BankPaper.SelectedIndex;
            Set_BankQuestion(bank_id);

        }

        private void SaveBankAnswer_Click(object sender, RoutedEventArgs e)
        {
            save_BankAnswer();
        }

 

        private void ShowBankAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (count_bank>0)
            {
                DataRow dataRow = dataSet.Tables["bank"].Rows.Find(bank_question_id);
                string answer = dataRow["ques_answer"].ToString();
                string explain = !dataRow.IsNull("ques_explain") ? dataRow["ques_explain"].ToString() : "暂无解析";
                System.Windows.MessageBox.Show("答案：" + answer + "\n解析：" + explain);
            }
            
        }

     
    }
}
