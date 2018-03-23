using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Baidu.Aip.Face;

namespace WpfApp1
{
    class BaiduAI
    {
        public static String usergroup;
        public static String username;
        public static String userid;
        public static double score;
        string groupId = "group1";
        //static string APP_ID = "10510765";
        static string API_KEY = "UGhdtobh5w5Q1AS3pEZEK9NI";
        static string SECRET_KEY = "U5UnWbTLDMEfLa8pAhkaE6F3UNyTpgzu";
        Face client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);

        public string face_useradd(String uid, string user_name, byte[] face) {
            try
            {
                var options = new Dictionary<string, object>{
        {"action_type", "replace"}
    };
                var result1 = client.UserAdd(uid, user_name, groupId, face, options);

                if (result1["error_code"] == null)
                {
                    return "success";
                }
                else
                {
                    return "fail";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("网络连接失败");

                return "fail";
            }
        }

        public DataTable face_getuser() {
            DataTable dataTable = new DataTable();
            try
            {
              
                dataTable.Columns.Add("学号", typeof(string));
                dataTable.Columns.Add("姓名", typeof(string));
                var result1 = client.GroupGetusers(groupId);
                int stu_num = Convert.ToInt32(result1["result_num"]);
                if (stu_num != 0)
                {
                    for (int i = 0; i < stu_num; i++)
                    {
                         userid = result1["result"][i]["uid"].ToString();
                         username = result1["result"][i]["user_info"].ToString();
                        dataTable.Rows.Add(userid, username);
                    }
                }

                return dataTable;
            }
            catch (Exception)
            {
                MessageBox.Show("网络连接失败");
                return dataTable;
            }
           
        }

        public string face_deleteuser(String userid)
        {
            var result1 = client.UserDelete(userid);
            string result = Convert.ToString(result1);
            return result;
        }

        public string face_verify(byte[] face) {
            try
            {
                var options = new Dictionary<string, object>{{"top_num", 1},{"ext_fields", "faceliveness"}};
                var result1 = client.Verify(userid, groupId, face,options);


                if (result1["error_code"] == null)
                {
                    double faceliveness = Convert.ToDouble(result1["ext_info"]["faceliveness"].ToString());
                  
                        score = Convert.ToDouble(result1["result"][0].ToString());
                        if (score >= 80)
                        {

                            return "欢迎你，" + username;

                        }
                        else
                        {
                            return "unknown_face";
                        }
                  
                 

                }

                else
                {
                    return "no_face";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("网络连接失败");

                return "fail";
            }
          

        }

        public string face_identify(byte[] face)
        {
            try
            {
                var options = new Dictionary<string, object>{
        {"ext_fields", "faceliveness"},
        {"user_top_num", 1}
    };
                var result1 = client.Identify(groupId, face,options);
                //MessageBox.Show(result1.ToString());

                if ((result1["error_code"] == null))
                {
                    double faceliveness = Convert.ToDouble(result1["ext_info"]["faceliveness"].ToString());
                    if (faceliveness > 0.393241)
                    {
                        score = Convert.ToDouble(result1["result"][0]["scores"][0].ToString());
                        if (score >= 80)
                        {
                            if (String.IsNullOrEmpty(userid))
                            {
                                userid = result1["result"][0]["uid"].ToString();
                                username = result1["result"][0]["user_info"].ToString();
                                return "success";
                            }
                            else
                            {
                                string Uid = result1["result"][0]["uid"].ToString();
                                if (Uid.Equals(userid))
                                {
                                return Uid;

                                }
                                else
                                {
                                return "success";

                                }
                            }
                          
                        }
                        else
                        {
                                return "unknown_face";
                        }
                    }
                    else
                    {
     
                        return "live_fail";

                    }
                }
              else  if (result1["error_code"].ToString().Equals("216402"))
                {
                    return "no_face";
                }
                else
                {
                    return "fail";
                }
            
            }
            catch (Exception)
            {

                MessageBox.Show("网络连接失败");

                return "fail";
            }
           

        }




     

    }
}
