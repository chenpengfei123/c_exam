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

        int i=0;
        int j = 0;
        int count_single;
        int count_bank;
        int single_question_id = 0;
        int bank_question_id = 0;
        //DataTable dataSet;
        char[] answer_single;
        String[] answer_bank;
        DataSet dataSet;
        public Single()
        {
            InitializeComponent();
            dataSet = new DataSet();
            //dataSet = new DataTable();
            MySqlConnection mycon = db_connect.Mysql_con();
            String sql_single = "Select * from single_question ";
            String sql_bank = "Select * from bank_question ";
         
      
            mycon.Open();
            //MySqlCommand mycmd = new MySqlCommand(sql_single, mycon);
            MySqlDataAdapter adapter_single = new MySqlDataAdapter(sql_single,mycon);
            MySqlDataAdapter adapter_bank = new MySqlDataAdapter(sql_bank, mycon);
            adapter_single.Fill(dataSet,"single");
            adapter_bank.Fill(dataSet, "bank");
            //MySqlDataReader reader = mycmd.ExecuteReader();
            //dt.Load(reader);


            //if (reader!=null&&!reader.IsClosed)
            //{
            //    reader.Close();
            //}
            Set_SingleQuestion(i);
            Set_BankQuestion(j);
             //count = singles.Count;
             count_single = dataSet.Tables["single"].Rows.Count;
            count_bank = dataSet.Tables["bank"].Rows.Count;
            answer_single = new char[count_single];
            answer_bank = new string [count_bank];
            countdown countdown = new countdown(endtime,this);
            progressbar_single.Maximum = count_single;//设置最大长度值
            progress_bank.Maximum = count_bank;
            progressbar_single.Value = 0;//设置当前值
            progress_bank.Value = 0;
            finish_single.Content = "已完成0/" + count_single + "题";
            finish_bank.Content = "已完成0/" + count_bank + "题";
        }
        private void Set_BankQuestion(int j) {
            bank_question.Text = dataSet.Tables["bank"].Rows[j]["bank_id"] + " 、 " + dataSet.Tables["bank"].Rows[i]["ques_name"];
            bank_question_id = (int)dataSet.Tables["bank"].Rows[i]["bank_id"];
            if (answer_bank!=null)
            {
                bank_answer.Text = answer_bank[j];
            }
        }
        private void Set_SingleQuestion(int i)
        {

            
            single_name.Text = dataSet.Tables["single"].Rows[i]["ques_id"] +" 、 "+ dataSet.Tables["single"].Rows[i]["ques_name"];
            single_answerA.Content = dataSet.Tables["single"].Rows[i]["ques_answerA"];
            single_answerB.Content = dataSet.Tables["single"].Rows[i]["ques_answerB"];
            single_answerC.Content = dataSet.Tables["single"].Rows[i]["ques_answerC"];
            single_answerD.Content = dataSet.Tables["single"].Rows[i]["ques_answerD"];
            single_question_id = (int)dataSet.Tables["single"].Rows[i]["ques_id"];
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
        private void save_BankAnswer() {
            if (!bank_answer.Text.Trim( ).Equals(""))
            {
                
                answer_bank[j] = bank_answer.Text.Trim();
                SetAnswer(1, bank_answer.Text.Trim());
                finishBank();
            }
            return;
        }
        private void single_answerA_Checked(object sender, RoutedEventArgs e)
        {
         
            answer_single[i] = 'A';
            SetAnswer(0,"A");
            FinishSingle();
        }
        private void single_answerB_Checked(object sender, RoutedEventArgs e)
        {
           
            answer_single[i] = 'B';
            SetAnswer(0,"B");
            FinishSingle();
        }
        private void single_answerC_Checked(object sender, RoutedEventArgs e)
        {
    
            answer_single[i] = 'C';
            SetAnswer(0,"C");
            FinishSingle();
        }
        private void single_answerD_Checked(object sender, RoutedEventArgs e)
        {
       
            answer_single[i] = 'D';
            SetAnswer(0,"D");
            FinishSingle();
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

        private void SetAnswer(int i, string answer)
        {
            String sql;
            //if (i<count_single)
            //{
            if (i==1)
            {
             sql = "replace into bank_answer_stu(ques_id,stu_name,stu_answer,time) values(" + bank_question_id + ",'" + BaiduAI.userid + "','" + answer + "',now()) ";

            }
            else
            {
             sql = "replace into single_answer_stu(ques_id,stu_name,stu_answer,time) values(" + single_question_id + ",'" + BaiduAI.userid + "','"+answer+"',now()) ";

            }
            MySqlConnection mycon = db_connect.Mysql_con();
            mycon.Open();
            MySqlCommand mycmd = new MySqlCommand(sql, mycon);
            mycmd.ExecuteNonQuery();
            if (mycon != null && mycon.State == ConnectionState.Open)
            {
                mycon.Close();
            }
            //}

            return;



        }

        private void FinishSingle()
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
        private void finishBank() {
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
                DialogResult r1 = System.Windows.Forms.MessageBox.Show("退出系统", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r1.ToString() == "OK")

                { this.Close(); }
                //System.Windows.MessageBox.Show("已经是最后一题了");
                j = count_bank - 1;
            }
        }

        //public static  void TimeEnd()
        //{
        //    DialogResult r1 = System.Windows.Forms.MessageBox.Show("考试时间到了，点击提交?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        //    if (r1.ToString() == "OK")

        //    {
        //        Single.close();

        //    }
               
        //}

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult r1 = System.Windows.Forms.MessageBox.Show("确认提交?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r1.ToString() == "OK")

            {
                System.Windows.MessageBox.Show("提交成功");



                MySqlConnection mycon = db_connect.Mysql_con();
                String sql_single = "Select count(*) from single_question,single_answer_stu where single_question.ques_id=single_answer_stu.ques_id and single_question.ques_answer=single_answer_stu.stu_answer and single_answer_stu.stu_name='"+LoginWindow.stu_name+"'";
                String sql_bank = "Select count(*) from bank_question,bank_answer_stu where bank_question.bank_id=bank_answer_stu.ques_id and bank_question.ques_answer=bank_answer_stu.stu_answer and bank_answer_stu.stu_name='" + LoginWindow.stu_name + "'";

                mycon.Open();

                MySqlCommand mycmd = new MySqlCommand(sql_single, mycon);
                MySqlCommand mycmd1 = new MySqlCommand(sql_bank, mycon);
                //int g = int.Parse(mycmd.ExecuteScalar().ToString());

                System.Windows.MessageBox.Show("选择题你答对了"+ mycmd.ExecuteScalar().ToString()+ "题,\n填空题你答对了"+ mycmd1.ExecuteScalar().ToString() +"题。");

                String sql = "replace into score(stu_name,subject,score_single,score_bank) values('" + LoginWindow. stu_name + "',1 ,"+mycmd.ExecuteScalar().ToString()+","+mycmd1.ExecuteScalar().ToString()+")" ;

                MySqlCommand mycmd2 = new MySqlCommand(sql_bank, mycon);
                mycmd2.ExecuteNonQuery();
                if (mycon != null && mycon.State == ConnectionState.Open)
                {
                    mycon.Close();
                }
                this.Close();
            }
        }
    }
}
