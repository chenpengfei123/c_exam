using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
//using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    class db_connect
    {

        private static String SERVER = "server=115.159.148.59";
        private static String USER_ID = "User Id=abcabc1130";
        private static String PASSWORD = "password=abcabc123";
        private static String DATABASE = "Database=c_exam";

        public static MySqlConnection Mysql_con()
        {
            MySqlConnection mycon = null;
            try
            {
                string constr = SERVER + ";" + USER_ID + ";" + PASSWORD + ";" + DATABASE;
                mycon = new MySqlConnection(constr);
            }
            catch (Exception)
            {
                throw;
            }
            return mycon;
        }




        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }
        /// <summary>
        /// 读取客户设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetSettingString(string settingName)
        {
            try
            {
                string settingString = ConfigurationManager.AppSettings[settingName].ToString();
                return settingString;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="valueName"></param>
        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, valueName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        public static void AddNonQuery(string sql)
        {

            MySqlConnection mycon = db_connect.Mysql_con();
            mycon.Open();
            MySqlCommand mycmd = new MySqlCommand(sql, mycon);
            mycmd.ExecuteNonQuery();
            if (mycon != null && mycon.State == ConnectionState.Open)
            {
                mycon.Close();
            }
            //MessageBox.Show("添加成功");

            return;



        }

        public static int getcount(string sql)
        {

            MySqlConnection mycon = db_connect.Mysql_con();
            mycon.Open();
            MySqlCommand mycmd = new MySqlCommand(sql, mycon);
            if (mycon != null && mycon.State == ConnectionState.Open)
            {
                mycon.Close();
            }
            return Convert.ToInt32( mycmd.ExecuteScalar());
         
            //MessageBox.Show("添加成功");

          



        }


        public static byte[] getpictures(string sql)
        {

            MySqlConnection mycon = db_connect.Mysql_con();
            mycon.Open();
            MySqlCommand mycmd = new MySqlCommand(sql, mycon);
            MySqlDataReader reader = mycmd.ExecuteReader();
            if (reader.Read())
            {
                byte[] image = (byte[])reader["stu_image"];
                return image;
            }
            if (mycon != null && mycon.State == ConnectionState.Open)
            {
                reader.Close();
                mycon.Close();
            }
            return null;

        }
        public static DataTable GetScores()
        {
            string sql = "select * from score";
            DataTable scores = new DataTable();
            MySqlConnection mycon = db_connect.Mysql_con();
            mycon.Open();
            MySqlDataAdapter adapter_single = new MySqlDataAdapter(sql, mycon);
            adapter_single.Fill(scores);
   
            if (mycon != null && mycon.State == ConnectionState.Open)
            {
           
                mycon.Close();
            }
            return scores;
        }
    }
}
