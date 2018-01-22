using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1
{
    class countdown
    {
        Label count_time;
        Window single;
        private static DateTime fiveM = new DateTime();
        private static Timer aTimer = new Timer();
        public countdown(Label countdown, Window w)
        {
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



        private delegate void UpdateUiTextDelegate(Label countdown);
        private delegate void SingleDelegate(Window w);
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (fiveM != Convert.ToDateTime("00:00:00"))
            {
                fiveM = fiveM.AddSeconds(-1);
                if (!count_time.Dispatcher.CheckAccess())
                {
                    count_time.Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate((Label countdown) =>
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
                //if (!single.Dispatcher.CheckAccess())
                //{
                //    single.Dispatcher.Invoke(DispatcherPriority.Send, new SingleDelegate((Window single) =>
                //    {
                   
                      
                       //Single.TimeEnd();
                      
                //    }), single);
                  
                //}
               


            }

        }


    }
}