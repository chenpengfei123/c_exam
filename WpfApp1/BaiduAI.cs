using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Baidu.Aip.Face;

namespace WpfApp1
{
    class BaiduAI
    {
        public static String username;
        public static String userid;
        public static double score;
        string groupId = "group1";
        static string APP_ID = "10510765";
        static string API_KEY = "UGhdtobh5w5Q1AS3pEZEK9NI";
        static string SECRET_KEY = "U5UnWbTLDMEfLa8pAhkaE6F3UNyTpgzu";
        Face client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);


        public string face_useradd(String uid, string user_name, byte[] face) {
            var options = new Dictionary<string, object>{
        {"action_type", "replace"}
    };
            var result1 = client.UserAdd(uid, user_name, groupId, face,options);
            if (result1["error_code"] == null)
            {
                return "注册成功";
            }
            else
            {
                return "注册失败";
            }
        }

        public DataTable face_getuser() {
            DataTable dataTable =new DataTable();
            dataTable.Columns.Add("学号", typeof(string));
            dataTable.Columns.Add("姓名", typeof(string));
            var result1 = client.GroupGetusers(groupId);
            int  stu_num =Convert.ToInt32( result1["result_num"]);
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

        public string face_deleteuser(String userid)
        {
            var result1 = client.UserDelete(userid);
            string result = Convert.ToString(result1);
            return result;
        }

        public string face_identify(byte[] face)
        {

            var result1 = client.Identify(groupId, face);
              if (result1["error_code"] == null)
            {
         
               score =Convert.ToDouble(result1["result"][0]["scores"][0].ToString()) ;
                if (score>=80)
                {
                    userid = result1["result"][0]["uid"].ToString();
                    username = result1["result"][0]["user_info"].ToString();
                    return "学号：" + userid + "\n姓名：" + username+Convert.ToString(result1);

                }
                return "识别不出你是谁";
            }

            else
            {
                //MessageBox.Show("登录失败");
                return "未识别到人脸";
            }

        }
    }
}
