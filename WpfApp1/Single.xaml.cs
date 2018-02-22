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
            dataSet = new DataSet();
             single_answer = new DataTable();
            bank_answer1 = new DataTable();
            String sql_single = "Select * from single_question where ques_subject= "+subject;
            String sql_bank = "Select * from bank_question  where ques_subject= " + subject ;
            DataTable single_question= db_connect.GetTables(sql_single);
            DataTable single_bank = db_connect.GetTables(sql_bank);

            single_answer.Columns.Add("question_id");
            single_answer.Columns.Add("userid");
            single_answer.Columns.Add("answer");
            single_answer.Columns.Add("subject");
            single_answer.Columns.Add("time");
            single_answer.PrimaryKey =new DataColumn[] { single_answer.Columns["question_id"] };
            bank_answer1.Columns.Add("question_id");
            bank_answer1.Columns.Add("userid");
            bank_answer1.Columns.Add("answer");
            bank_answer1.Columns.Add("subject");
            bank_answer1.Columns.Add("time");
            bank_answer1.PrimaryKey = new DataColumn[] { bank_answer1.Columns["question_id"] };
            single_question.TableName = "single";
            single_bank.TableName = "bank";
            dataSet.Tables.Add(single_question);
            dataSet.Tables.Add(single_bank);
             count_single = dataSet.Tables["single"].Rows.Count;
            count_bank = dataSet.Tables["bank"].Rows.Count;
            if (count_single>0)
            {
            Set_SingleQuestion(single_id);
            }
            else
            {
                single_name.Text = "没有选择题";
                single_answerA.IsEnabled = false;
                single_answerB.IsEnabled = false;
                single_answerC.IsEnabled = false;
                single_answerD.IsEnabled = false;
            }
            if (count_bank>0)
            {
            Set_BankQuestion(bank_id);
            }
            else
            {
                bank_question.Text = "没有填空题";
            }
            user_message.Text = "欢迎你，" + BaiduAI.username;
          countdown = new CountDown(endtime,this,user_message);
            progressbar_single.Maximum = count_single;//设置最大长度值
            progress_bank.Maximum = count_bank;
            progressbar_single.Value = 0;//设置当前值
            progress_bank.Value = 0;
            finish_single.Content = "已完成0/" + count_single + "题";
            finish_bank.Content = "已完成0/" + count_bank + "题";
        }


        private void Set_SingleQuestion(int i)
        {

            
            single_name.Text = i+1 +" 、 "+ dataSet.Tables["single"].Rows[i]["ques_name"];
            single_answerA.Content = "A、"+dataSet.Tables["single"].Rows[i]["ques_answerA"];
            single_answerB.Content = "B、" + dataSet.Tables["single"].Rows[i]["ques_answerB"];
            single_answerC.Content = "C、" + dataSet.Tables["single"].Rows[i]["ques_answerC"];
            single_answerD.Content = "D、" + dataSet.Tables["single"].Rows[i]["ques_answerD"];
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
            bank_question.Text = j+1 + " 、 " + dataSet.Tables["bank"].Rows[single_id]["ques_name"];
            bank_question_id = (int)dataSet.Tables["bank"].Rows[j]["bank_id"];
            if (bank_answer1.Rows.Contains(bank_question_id))
            {
                DataRow dr = bank_answer1.Rows.Find(bank_question_id);

                bank_answer.Text = (string)dr["answer"];
            }
            else
            {
                bank_answer.Text = "";
            }
        }

        private void save_BankAnswer() {
            if (!bank_answer.Text.Trim( ).Equals(""))
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
            
          
            if (++single_id < count_single)
            {
                Set_SingleQuestion(single_id);
            }
            else
            {
                System.Windows.MessageBox.Show("已经是最后一题了");
                single_id = count_single-1;
            }
          
            
        }

        private void Single_back_Click(object sender, RoutedEventArgs e)
        {
         
            if (--single_id >= 0)
            {
                Set_SingleQuestion(single_id);
            }
            else {
                System.Windows.MessageBox.Show("已经是第一题了");
                single_id = 0;
                }
       
        }
   
        private void Bank_back_Click(object sender, RoutedEventArgs e)
        {
            save_BankAnswer();
            if (--bank_id >= 0)
            {
                Set_BankQuestion(bank_id);
            }
            else
            {
                System.Windows.MessageBox.Show("已经是第一题了");
                bank_id = 0;
            }
        }

        private void Bank_next_Click(object sender, RoutedEventArgs e)
        {
            save_BankAnswer();
            if (++bank_id < count_bank)
            {
                Set_BankQuestion(bank_id);
            }
            else
            {
               
                System.Windows.MessageBox.Show("已经是最后一题了");
                bank_id = count_bank - 1;
            }
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
            string addsingleanswer = "replace into single_answer_stu(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_name, @stu_answer, @subject, @time)";
            string addbankanswer = "replace into bank_answer_stu(ques_id, stu_id, stu_answer, subject, time) VALUES(@ques_id, @stu_name, @stu_answer, @subject, @time)";
            mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@ques_id", MySqlDbType.Int32, 25, "question_id"),
                     new MySqlParameter("@stu_name", MySqlDbType.VarChar, 25, "userid"),
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

    }
}
