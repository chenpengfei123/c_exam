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
        public static String username;
        public static String userid;
        public static double score;
        string groupId = "group1";
        static string APP_ID = "10510765";
        static string API_KEY = "UGhdtobh5w5Q1AS3pEZEK9NI";
        static string SECRET_KEY = "U5UnWbTLDMEfLa8pAhkaE6F3UNyTpgzu";
        Face client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);


        //private bool GetPicZoomSize(ref int picWidth, ref int picHeight, int specifiedWidth, int specifiedHeight)
        //{
        //    int sW = 0, sH = 0;
        //    Boolean isZoomSize = false;
        //    //按比例缩放
        //    System.Drawing.Size tem_size = new System.Drawing.Size(picWidth, picHeight);
        //    if (tem_size.Width > specifiedWidth || tem_size.Height > specifiedHeight) //将**改成c#中的或者操作符号
        //    {
        //        if ((tem_size.Width * specifiedHeight) > (tem_size.Height * specifiedWidth))
        //        {
        //            sW = specifiedWidth;
        //            sH = (specifiedWidth * tem_size.Height) / tem_size.Width;
        //        }
        //        else
        //        {
        //            sH = specifiedHeight;
        //            sW = (tem_size.Width * specifiedHeight) / tem_size.Height;
        //        }
        //        isZoomSize = true;
        //    }
        //    else
        //    {
        //        sW = tem_size.Width;
        //        sH = tem_size.Height;
        //    }
        //    picHeight = sH;
        //    picWidth = sW;
        //    return isZoomSize;
        //}
        //public bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        //{
        //    System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
        //    ImageFormat tFormat = iSource.RawFormat;
        //    int sW = iSource.Width, sH = iSource.Height;

        //    GetPicZoomSize(ref sW, ref sH, dWidth, dHeight);

        //    Bitmap ob = new Bitmap(dWidth, dHeight);
        //    Graphics g = Graphics.FromImage(ob);
        //    g.Clear(Color.WhiteSmoke);
        //    g.CompositingQuality = CompositingQuality.HighQuality;
        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
        //    g.Dispose();
        //    //以下代码为保存图片时，设置压缩质量
        //    EncoderParameters ep = new EncoderParameters();
        //    long[] qy = new long[1];
        //    qy[0] = flag;//设置压缩的比例1-100
        //    EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
        //    ep.Param[0] = eParam;
        //    try
        //    {
        //        ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

        //        ImageCodecInfo jpegICIinfo = null;

        //        for (int x = 0; x < arrayICI.Length; x++)
        //        {
        //            if (arrayICI[x].FormatDescription.Equals("JPEG"))
        //            {
        //                jpegICIinfo = arrayICI[x];
        //                break;
        //            }
        //        }
        //        if (jpegICIinfo != null)
        //        {
        //            ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
        //        }
        //        else
        //        {
        //            ob.Save(dFile, tFormat);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        iSource.Dispose();
        //        ob.Dispose();

        //    }
        //}





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
                  string  userid = result1["result"][i]["uid"].ToString();
                   string  username = result1["result"][i]["user_info"].ToString();
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
        public string face_verify(byte[] face) {
            var result1 = client.Verify(userid, groupId, face);
            if (result1["error_code"] == null)
            {

                score = Convert.ToDouble(result1["result"][0].ToString());
                if (score >= 80)
                {

                    return "欢迎你，"+username;

                }
                return "对不起，你是谁";
            }

            else
            {             
                return "未识别到人脸";
            }

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
