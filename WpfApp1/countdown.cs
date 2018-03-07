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
using WpfApp1.Control;

namespace WpfApp1
{
    class CountDown
    {
        MySqlParameter[] mySqlParameter;
        BaiduAI baiduAI;
        int i = 0;
        int single_score;
        int bank_score;
        System.Windows.Controls.Label count_time;
        Window single;
        TextBlock userMessage;
        private static DateTime fiveM = new DateTime();
        private System.Timers.Timer aTimer;
        public CountDown(System.Windows.Controls.Label countdown, Window w,TextBlock textBlock,int time, int single_score, int bank_score)
        {
      
            baiduAI = new BaiduAI();
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            aTimer.Interval = 1000;
            aTimer.AutoReset = true;//执行一次 false，一直执行true  
            //是否执行System.Timers.Timer.Elapsed事件  
            aTimer.Enabled = true;
            this.count_time = countdown;
            this.single = w;
            this.userMessage = textBlock;
            this.bank_score = bank_score;
            this.single_score = single_score;

            if (time>59)
            {
                string time1 = (time / 60).ToString().PadLeft(2,'0') + ":" + (time% 60).ToString().PadLeft(2, '0') + ":00";
                fiveM = DateTime.Parse(time1);

            }
            else
            {
                string time1 = "00:" + time.ToString().PadLeft(2, '0') + ":00";
                fiveM = DateTime.Parse(time1);
            }
         
            aTimer.Start();
      
        }

        public  void destroyCountdown() {
            aTimer.Dispose();
        }

        private delegate void UpdateUiTextDelegate(System.Windows.Controls.Label countdown);
        private delegate void SingleDelegate(Window w);
     
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            i++; 
        
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
                if (i==10)
                {
                  Exam.face1 = CameraHelper.CaptureImage();
                }
                if (i % 10 == 0)
                {
                    byte[] face = CameraHelper.CaptureImage();
                    
                    string logininfo = baiduAI.face_verify(face);

                    
                        userMessage.Dispatcher.Invoke(new Action(() => userMessage.Text=logininfo ));

                    
                   
                }
            }
            else
            {
               Exam.face2 = CameraHelper.CaptureImage();
                aTimer.Stop();
                aTimer.Dispose();
           
                single.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("时间到了，请交卷", "提示", MessageBoxButton.OK);
                    if (messageBoxResult.ToString() == "OK")
                    {

                        Exam.SubmitAnswer();
                        GetScores();
                    }

                    single.Close();
                }

               ));
                

            }

        }



        public  void GetScores( )
        {
            String sql_single;
            String sql_bank;
            if (Exam.IsExam.Equals("exam"))
            {

                sql_single = "Select count(*) from single_question,exam_single_answer where single_question.ques_id=exam_single_answer.ques_id and single_question.ques_answer=exam_single_answer.stu_answer and exam_single_answer.stu_id=@userid  and   exam_single_answer.exam_id=@exam_id";
                sql_bank = "Select count(*) from bank_question,exam_bank_answer where bank_question.bank_id=exam_bank_answer.ques_id and bank_question.ques_answer=exam_bank_answer.stu_answer and exam_bank_answer.stu_id=@userid   and exam_bank_answer.exam_id=@exam_id";


                mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@userid",BaiduAI.userid),
                     new MySqlParameter("@exam_id",Exam.ID)
                };

                int single_count = db_connect.getcount(sql_single, mySqlParameter);
                int bank_count = db_connect.getcount(sql_bank, mySqlParameter);
                int score = single_count * single_score + bank_count * bank_score;

                System.Windows.MessageBox.Show("选择题你答对了" + single_count * single_score + "题。\n填空题你答对了" + bank_count * bank_score + "题。\n得分：" + score);

                String sql = "replace into exam_score  values(@userid, @username,@exam_id,@singlescore,@bankscore,@score )";

                mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@userid",BaiduAI.userid),
                    new MySqlParameter("@username",BaiduAI.username),
                     new MySqlParameter("@exam_id",Exam.ID),
                      new MySqlParameter("@singlescore",single_count),
                      new MySqlParameter("@bankscore",bank_count),
                         new MySqlParameter("@score",score)
                };
                db_connect.AddNonQuery(sql, mySqlParameter);

                sql = "replace into exam_picture(stu_id,stu_name,exam_id,picture1,picture2,time) values(@userid,@username,@exam_id,@picture1,@picture2,now())";

                mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@userid",BaiduAI.userid),
                    new MySqlParameter("@username",BaiduAI.username),
                     new MySqlParameter("@exam_id",Exam.ID),
                      new MySqlParameter("@picture1",Exam.face1),
                      new MySqlParameter("@picture2",Exam.face2)
                };
                db_connect.AddNonQuery(sql, mySqlParameter);
            }
          
        }
    }
}