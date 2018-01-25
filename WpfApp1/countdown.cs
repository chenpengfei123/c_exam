using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    class CountDown
    {
        System.Windows.Controls.Label count_time;
        Window single;
        private static DateTime fiveM = new DateTime();
        private System.Timers.Timer aTimer;
        public CountDown(System.Windows.Controls.Label countdown, Window w)
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            aTimer.Interval = 1000;
            aTimer.AutoReset = true;//执行一次 false，一直执行true  
            //是否执行System.Timers.Timer.Elapsed事件  
            aTimer.Enabled = true;
            this.count_time = countdown;
            this.single = w;
            fiveM = DateTime.Parse("00:00:10");
            aTimer.Start();
        }

        public  void destroyCountdown() {
            aTimer.Dispose();
        }

        private delegate void UpdateUiTextDelegate(System.Windows.Controls.Label countdown);
        private delegate void SingleDelegate(Window w);
     
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (fiveM != Convert.ToDateTime("00:00:00"))
            {
                fiveM = fiveM.AddSeconds(-1);
                if (!count_time.Dispatcher.CheckAccess())
                {
                    count_time.Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate((System.Windows.Controls.Label countdown) =>
                    {
                        
                        countdown.Content = "剩余时间：" + fiveM.Hour.ToString("00") + ":" + fiveM.Minute.ToString("00") + ":" + fiveM.Second.ToString("00");
                    }), count_time);

                }
                else
                {
                    count_time.Content = "剩余时间：" + fiveM.Hour.ToString("00") + ":" + fiveM.Minute.ToString("00") + ":" + fiveM.Second.ToString("00");
                }

            }
            else
            {
                aTimer.Stop();
                aTimer.Dispose();
                //MessageBoxResult messageBoxResult= System.Windows.MessageBox.Show("时间到了","提示",MessageBoxButton.OK);
                // if (messageBoxResult.ToString()=="OK")
                // {

                //}

                //single.Dispatcher.Invoke(DispatcherPriority.Send, new SingleDelegate((Window w) =>
                //    {
                //        single.Close();
                //    }), single);
                single.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("时间到了，请交卷", "提示", MessageBoxButton.OK);
                    if (messageBoxResult.ToString() == "OK")
                    {
                        System.Windows.MessageBox.Show("提交成功");

                    }

                    submitAnswer();
                    single.Close();
                }

               ));
                

            }

        }

        public  void submitAnswer()
        {
            String sql_single = "Select count(*) from single_question,single_answer_stu where single_question.ques_id=single_answer_stu.ques_id and single_question.ques_answer=single_answer_stu.stu_answer and single_answer_stu.stu_name='" + BaiduAI.userid + "'";
            String sql_bank = "Select count(*) from bank_question,bank_answer_stu where bank_question.bank_id=bank_answer_stu.ques_id and bank_question.ques_answer=bank_answer_stu.stu_answer and bank_answer_stu.stu_name='" + BaiduAI.userid + "'";
            int single_score = db_connect.getcount(sql_single);
            int single_bank = db_connect.getcount(sql_bank);


            System.Windows.MessageBox.Show("选择题你答对了" + single_score + "题,\n填空题你答对了" + single_bank + "题。");

            String sql = "replace into score(stu_name,subject,score_single,score_bank) values('" + BaiduAI.userid + "',1 ," + single_score + "," + single_bank + ")";

            db_connect.AddNonQuery(sql);
        }
    }
}