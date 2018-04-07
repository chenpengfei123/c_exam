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
        int blank_score;
        System.Windows.Controls.Label count_time;
        Window Exam;
        TextBlock userMessage;
        private static DateTime fiveM = new DateTime();
        private System.Timers.Timer aTimer;
        public CountDown(System.Windows.Controls.Label countdown, Window w,TextBlock textBlock,int time, int single_score, int blank_score)
        {
      
            baiduAI = new BaiduAI();
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            aTimer.Interval = 1000;
            aTimer.AutoReset = true;//执行一次 false，一直执行true  
            //是否执行System.Timers.Timer.Elapsed事件  
            aTimer.Enabled = true;
            this.count_time = countdown;
            this.Exam = w;
            this.userMessage = textBlock;
            this.blank_score = blank_score;
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


            if (WpfApp1.Exam.IsExam.Equals("exam"))
            {
                String sql = "insert into exam_score(stu_id,stu_name,exam_id,start_time)  values(@userid, @username,@exam_id,now() )";

                mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@userid",BaiduAI.userid),
                    new MySqlParameter("@username",BaiduAI.username),
                     new MySqlParameter("@exam_id",WpfApp1.Exam.ID)

                };
                db_connect.AddNonQuery(sql, mySqlParameter);
            }


        }

        public  void destroyCountdown() {
            aTimer.Dispose();
        }

        private delegate void UpdateUiTextDelegate(System.Windows.Controls.Label countdown);
        private delegate void SingleDelegate(Window w);
     
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
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
                    WpfApp1.Exam.face1 = CameraHelper.CaptureImage();
                }
                if (i % 10 == 0)
                {
                   
                    

                    byte[] face = CameraHelper.CaptureImage();
                    
                    string logininfo = baiduAI.face_verify(face);
                    if (!logininfo.Equals("欢迎你，" + BaiduAI.username))
                    {
                        if (logininfo.Equals("unknown_face"))
                        {
                            userMessage.Dispatcher.Invoke(new Action(() => userMessage.Text = "你是谁？？？请确认为本人考试"));
                        }
                        else if (logininfo.Equals("no_face"))
                        {
                            userMessage.Dispatcher.Invoke(new Action(() => userMessage.Text = "未识别到人脸"));
                        }
                        else
                        {
                            userMessage.Dispatcher.Invoke(new Action(() => userMessage.Text = "网络故障"));
                        }

                        Exam.Dispatcher.Invoke(new Action(() =>
                        {
                     System.Windows.MessageBox.Show("请确保是你本人考试，并让摄像头能拍到你的全脸", "提示");  
                        }
                          ));
                    }
                    else
                    {
                        userMessage.Dispatcher.Invoke(new Action(() => userMessage.Text=logininfo ));

                    }
                    
                    

                    
                   
                }
            }
            else
            {
                WpfApp1.Exam.face2 = CameraHelper.CaptureImage();
                aTimer.Stop();
                aTimer.Dispose();
           
                Exam.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("时间到了，请交卷", "提示", MessageBoxButton.OK);
                    if (messageBoxResult.ToString() == "OK")
                    {
                        WpfApp1.Exam.SubmitAnswer();                   
                            GetExamScores();   
                    }

                    Exam.Close();
                }

               ));
                

            }

            }
            catch (Exception)
            {

                System.Windows.MessageBox.Show("定时遇到问题");
            }
        }



        public  void GetExamScores( )
        {
            String sql_single;
            String sql_blank;
            if (WpfApp1.Exam.IsExam.Equals("exam"))
            {

                sql_single = "Select count(*) from single_question,exam_single_answer where single_question.ques_id=exam_single_answer.ques_id and single_question.ques_answer=exam_single_answer.stu_answer and exam_single_answer.stu_id=@userid  and   exam_single_answer.exam_id=@exam_id";
                sql_blank = "Select count(*) from blank_question,exam_blank_answer where blank_question.ques_id=exam_blank_answer.ques_id and blank_question.ques_answer=exam_blank_answer.stu_answer and exam_blank_answer.stu_id=@userid   and exam_blank_answer.exam_id=@exam_id";


                mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@userid",BaiduAI.userid),
                     new MySqlParameter("@exam_id",WpfApp1.Exam.ID)
                };

                int single_count = db_connect.getcount(sql_single, mySqlParameter);
                int blank_count = db_connect.getcount(sql_blank, mySqlParameter);
                int score = single_count * single_score + blank_count * blank_score;

                System.Windows.MessageBox.Show("选择题你答对了" + single_count * single_score + "分。\n填空题你答对了" + blank_count * blank_score + "分。\n得分：" + score+"分");

                String sql = "update exam_score  set score_single=@singlescore,score_blank=@blankscore,score=@score,end_time=now() where stu_id=@userid and exam_id=@exam_id";

                mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@userid",BaiduAI.userid),
                     new MySqlParameter("@exam_id",WpfApp1.Exam.ID),
                      new MySqlParameter("@singlescore",single_count*single_score),
                      new MySqlParameter("@blankscore",blank_count* blank_score),
                         new MySqlParameter("@score",score)
                };
                db_connect.AddNonQuery(sql, mySqlParameter);

                sql = "replace into exam_picture(stu_id,stu_name,exam_id,picture1,picture2,time) values(@userid,@username,@exam_id,@picture1,@picture2,now())";

                mySqlParameter = new MySqlParameter[] {
                     new MySqlParameter("@userid",BaiduAI.userid),
                    new MySqlParameter("@username",BaiduAI.username),
                     new MySqlParameter("@exam_id",WpfApp1.Exam.ID),
                      new MySqlParameter("@picture1",WpfApp1.Exam.face1),
                      new MySqlParameter("@picture2",WpfApp1.Exam.face2)
                };
                db_connect.AddNonQuery(sql, mySqlParameter);
            }
          
           else if (WpfApp1.Exam.IsExam.Equals("simulation"))
            {
                int single_count = 0;
                int blank_count = 0;

                for (int i = 0; i < WpfApp1.Exam.dataSet.Tables["single"].Rows.Count; i++)
                {
                    string answer = WpfApp1.Exam.dataSet.Tables["single"].Rows[i]["ques_answer"].ToString();
                    string id = WpfApp1.Exam.dataSet.Tables["single"].Rows[i]["ques_id"].ToString();
                    if (WpfApp1.Exam.single_answer.Rows.Contains(id))
                    {
                        DataRow Row = WpfApp1.Exam.single_answer.Rows.Find(id);
                        if (Row["stu_answer"].Equals(answer))
                        {
                            single_count++;
                        }
                    }

                }
                for (int i = 0; i < WpfApp1.Exam.dataSet.Tables["blank"].Rows.Count; i++)
                {
                    string answer = WpfApp1.Exam.dataSet.Tables["blank"].Rows[i]["ques_answer"].ToString();
                    string id = WpfApp1.Exam.dataSet.Tables["blank"].Rows[i]["ques_id"].ToString();
                    if (WpfApp1.Exam.blank_answer1.Rows.Contains(id))
                    {
                        DataRow Row = WpfApp1.Exam.blank_answer1.Rows.Find(id);
                        if (Row["stu_answer"].Equals(answer))
                        {
                            blank_count++;
                        }
                    }
                }
                System.Windows.MessageBox.Show("选择题你答对了" + single_count  + "题。\n填空题你答对了" + blank_count+ "题。");

                ShowAnswer showAnswer = new ShowAnswer();
                showAnswer.Show();
            }
        }
       
    }
}