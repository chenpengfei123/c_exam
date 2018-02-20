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
   public   static   byte[] face1;
    public   static   byte[] face2;
        CountDown countdown;
        int i=0;
        int j = 0;
        int count_single;
        int count_bank;
        int single_question_id = 0;
        int bank_question_id = 0;
     public static   string subject; 
        char[] answer_single;
        String[] answer_bank;
        DataSet dataSet;
     public   static   DataTable single_answer;
        public Single()
        {
            InitializeComponent();
            CameraHelper.CameraInit(player);
            dataSet = new DataSet();
             single_answer = new DataTable();
            String sql_single = "Select * from single_question ";
            String sql_bank = "Select * from bank_question ";
            DataTable single_question= db_connect.GetTables(sql_single);
            DataTable single_bank = db_connect.GetTables(sql_bank);

            single_answer.Columns.Add("question_id");
            single_answer.Columns.Add("userid");
            single_answer.Columns.Add("answer");
            single_answer.Columns.Add("subject");
            single_answer.Columns.Add("time");
     
            single_question.TableName = "single";
            single_bank.TableName = "bank";
            dataSet.Tables.Add(single_question);
            dataSet.Tables.Add(single_bank);
            Set_SingleQuestion(i);
            Set_BankQuestion(j);
             count_single = dataSet.Tables["single"].Rows.Count;
            count_bank = dataSet.Tables["bank"].Rows.Count;
            answer_single = new char[count_single];
            answer_bank = new string [count_bank];
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
            single_answerA.Content = dataSet.Tables["single"].Rows[i]["ques_answerA"];
            single_answerB.Content = dataSet.Tables["single"].Rows[i]["ques_answerB"];
            single_answerC.Content = dataSet.Tables["single"].Rows[i]["ques_answerC"];
            single_answerD.Content = dataSet.Tables["single"].Rows[i]["ques_answerD"];
            single_question_id = (int)dataSet.Tables["single"].Rows[i]["ques_id"];
            subject =(string) dataSet.Tables["single"].Rows[i]["ques_subject"];
            if (answer_single != null)
            {
                switch (answer_single[i])
                {
                    case 'A':

                        single_answerA.IsChecked = true;
                        break;

                    case 'B':
                        single_answerB.IsChecked = true;
                        break;
                    case 'C':
                        single_answerC.IsChecked = true;
                        break;
                    case 'D':
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
            


         
        }

        private void Set_BankQuestion(int j) {
            bank_question.Text = dataSet.Tables["bank"].Rows[j]["bank_id"] + " 、 " + dataSet.Tables["bank"].Rows[i]["ques_name"];
            bank_question_id = (int)dataSet.Tables["bank"].Rows[i]["bank_id"];
            if (answer_bank!=null)
            {
                bank_answer.Text = answer_bank[j];
            }
        }

        private void save_BankAnswer() {
            if (!bank_answer.Text.Trim( ).Equals(""))
            {
                
                answer_bank[j] = bank_answer.Text.Trim();
                //SetAnswer(1, bank_answer.Text.Trim());
                ProgressBank();
            }
            return;
        }

        private void single_answerA_Checked(object sender, RoutedEventArgs e)
        {
            DataRow dataRow = single_answer.NewRow();
            dataRow["question_id"] = single_question_id;
            dataRow["userid"] = BaiduAI.userid;
            dataRow["answer"] = 'A';
            dataRow["subject"] = subject;
            dataRow["time"] = DateTime.Now.ToString();
            single_answer.Rows.InsertAt(dataRow, i);

            answer_single[i] = 'A';
     
            ProgressSingle();
        }
        private void single_answerB_Checked(object sender, RoutedEventArgs e)
        {
            DataRow dataRow = single_answer.NewRow();
            dataRow["question_id"] = single_question_id;
            dataRow["userid"] = BaiduAI.userid;
            dataRow["answer"] = 'B';
            dataRow["subject"] = subject;
            dataRow["time"] = DateTime.Now.ToString();
            single_answer.Rows.InsertAt(dataRow, i);

            answer_single[i] = 'B';
         
            ProgressSingle();
        }
        private void single_answerC_Checked(object sender, RoutedEventArgs e)

        {

            DataRow dataRow = single_answer.NewRow();
            dataRow["question_id"] = single_question_id;
            dataRow["userid"] = BaiduAI.userid;
            dataRow["answer"] = 'C';
            dataRow["subject"] = subject;
            dataRow["time"] = DateTime.Now.ToString();
            single_answer.Rows.InsertAt(dataRow, i);

            answer_single[i] = 'C';
     
            ProgressSingle();
        }
        private void single_answerD_Checked(object sender, RoutedEventArgs e)
        {

            DataRow dataRow = single_answer.NewRow();
            dataRow["question_id"] = single_question_id;
            dataRow["userid"] = BaiduAI.userid;
            dataRow["answer"] = 'D';
            dataRow["subject"] = subject;
            dataRow["time"] = DateTime.Now.ToString();
            single_answer.Rows.InsertAt(dataRow, i);

            answer_single[i] = 'D';
        
            ProgressSingle();
        }
        private void save_SingleAnswer()
        {
            if ((bool)single_answerA.IsChecked)
            {
              
            }
            else if ((bool)single_answerB.IsChecked)
            {
             
            }
            else if ((bool)single_answerC.IsChecked)
            {
              
            }
            else if ((bool)single_answerD.IsChecked)
            {
               
            }
            return;
        }

        //private void SetAnswer(int i, string answer)
        //{
        //    String sql;
           
        //    if (i==1)
        //    {
        //     sql = "replace into bank_answer_stu(ques_id,stu_name,stu_answer,time) values(" + bank_question_id + ",'" + BaiduAI.userid + "','" + answer + "',now()) ";

        //    }
        //    else
        //    {
        //     sql = "replace into single_answer_stu(ques_id,stu_name,stu_answer,time) values(" + single_question_id + ",'" + BaiduAI.userid + "','"+answer+"',now()) ";

        //    }
        //    db_connect.AddNonQuery(sql);
       

        //    return;



        //}

        private void ProgressSingle()
        {
            int j = 0;
            for (int i = 0; i < answer_single.Length; i++)
            {
                if (answer_single[i] == 'A' || answer_single[i] == 'B' || answer_single[i] == 'C' || answer_single[i] == 'D')
                {
                    j++;
                }
            }
            progressbar_single.Value = j;
            finish_single.Content = "已完成" + j + "/" + count_single + "题";
        }
        private void ProgressBank() {
            int j = 0;
            for (int i = 0; i < answer_bank.Length; i++)
            {
                if (!answer_bank[i].Equals(""))
                {
                    j++;
                }
            }
            progress_bank.Value = j;
            finish_bank.Content = "已完成" + j + "/" + count_bank + "题";
        }


        private void Single_next_Click(object sender, RoutedEventArgs e)
        {
            //save_SingleAnswer();
          
            if (++i < count_single)
            {
                Set_SingleQuestion(i);
            }
            else
            {
                System.Windows.MessageBox.Show("已经是最后一题了");
                i = count_single-1;
            }
          
            
        }

        private void Single_back_Click(object sender, RoutedEventArgs e)
        {
            //save_SingleAnswer();
            if (--i >= 0)
            {
                Set_SingleQuestion(i);
            }
            else {
                System.Windows.MessageBox.Show("已经是第一题了");
                i = 0;
                }
       
        }

     

        private void Bank_back_Click(object sender, RoutedEventArgs e)
        {
            save_BankAnswer();
            if (--j >= 0)
            {
                Set_BankQuestion(j);
            }
            else
            {
                System.Windows.MessageBox.Show("已经是第一题了");
                j = 0;
            }
        }

        private void Bank_next_Click(object sender, RoutedEventArgs e)
        {
            save_BankAnswer();
            if (++j < count_bank)
            {
                Set_BankQuestion(j);
            }
            else
            {
                //DialogResult r1 = System.Windows.Forms.MessageBox.Show("退出系统", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //if (r1.ToString() == "OK")

                //{ this.Close(); }
                System.Windows.MessageBox.Show("已经是最后一题了");
                j = count_bank - 1;
            }
        }


           
        

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认提交?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                face2 = CameraHelper.CaptureImage();
                db_connect.AddDatatable( single_answer);
                countdown.submitAnswer( );
                this.Close();
                
            }
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
