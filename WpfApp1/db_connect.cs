﻿using System;
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
       private static  MySqlConnection mycon;
        private static MySqlDataReader reader;
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
                MessageBox.Show("连接数据库失败");
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
            try
            {
                 mycon = db_connect.Mysql_con();
                mycon.Open();
                MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                mycmd.ExecuteNonQuery();
               

            }
            catch (Exception)
            {

                MessageBox.Show("执行操作失败");
            }
            finally {
                if (mycon != null && mycon.State == ConnectionState.Open)
                {
                    mycon.Close();
                }
            }
           


        }

        public static void register_Face(string sql,byte[] face )
        {
            try
            {
                mycon = db_connect.Mysql_con();
                mycon.Open();
                MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                mycmd.Parameters.Add("@filecontent", MySql.Data.MySqlClient.MySqlDbType.Blob);
                mycmd.Parameters[0].Value = face;
                mycmd.ExecuteNonQuery();
                mycon.Close();
          


            }
            catch (Exception)
            {

                MessageBox.Show("执行操作失败");
            }
            finally
            {
                if (mycon != null && mycon.State == ConnectionState.Open)
                {
                    mycon.Close();
                }
            }



        }

        public static int getcount(string sql)
        {
            try
            {
                mycon = db_connect.Mysql_con();
                mycon.Open();
                MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                int count = Convert.ToInt32(mycmd.ExecuteScalar());

                return count;
            }
            catch (Exception)
            {
                MessageBox.Show("执行任务失败");
                return 0;
            }
            finally {
                if (mycon != null && mycon.State == ConnectionState.Open)
                {
                    mycon.Close();
                }
            }
           
            

        }

        public static string  getstring(string sql)
        {
            try
            {
                mycon = db_connect.Mysql_con();
                mycon.Open();
                MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                string  respond = Convert.ToString(mycmd.ExecuteScalar());

                return respond;
            }
            catch (Exception)
            {
                MessageBox.Show("获取姓名失败");
                return null ;
            }
            finally
            {
                if (mycon != null && mycon.State == ConnectionState.Open)
                {
                    mycon.Close();
                }
            }



        }


        public static byte[] getpictures(string sql)
        {
            try
            {
                mycon = db_connect.Mysql_con();
                mycon.Open();
                MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                reader = mycmd.ExecuteReader();
                if (reader.Read())
                {
                    byte[] image = (byte[])reader["stu_image"];
                    return image;
                }
                else
                {
                return null;

                }
            }
            catch (Exception)
            {

                MessageBox.Show("执行任务失败");
                return null;
            }
            finally {
                if (mycon != null && mycon.State == ConnectionState.Open)
                {
                    reader.Close();
                    mycon.Close();
                }
            }
          
        

        }
        public static DataTable GetScores()
        {

            try
            {
                string sql = "select * from score";
                DataTable scores = new DataTable();
                mycon = db_connect.Mysql_con();
                mycon.Open();
                MySqlDataAdapter adapter_single = new MySqlDataAdapter(sql, mycon);
                adapter_single.Fill(scores);
                return scores;
            }
            catch (Exception)
            {
                MessageBox.Show("执行操作失败");
                return null;
            }

            finally {
                if (mycon != null && mycon.State == ConnectionState.Open)
                {

                    mycon.Close();
                }
            }
            
          
        }
    }
}
