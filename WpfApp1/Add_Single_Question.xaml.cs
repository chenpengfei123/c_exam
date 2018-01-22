using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Add_Single_Question.xaml 的交互逻辑
    /// </summary>
    public partial class Add_Single_Question : Window
    {
        public Add_Single_Question()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (single_name.Text.Equals("") | single_A.Equals("") | single_B.Equals("") | single_C.Equals("") | single_D.Equals(""))
            {
                MessageBox.Show("请确认输入了所有信息");
                return;
            }
            if ((bool)answer_A.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、"+single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'A')";
                setAnswer(sql);
            }
            else if ((bool)answer_B.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'B')";
                setAnswer(sql);
            }
            else if ((bool)answer_C.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'C')";
                setAnswer(sql);
            }
            else if ((bool)answer_D.IsChecked)
            {
                String sql = "insert into single_question(ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer) values('" + single_name.Text + "','" + "A、" + single_A.Text + "','" + "B、" + single_B.Text + "','" + "C、" + single_C.Text + "','" + "D、" + single_D.Text + "', 'D')";
                setAnswer(sql);
            }
            else {
                MessageBox.Show("请选择正确答案");
                return;
            }
            
           
          
        }
        private void setAnswer(string sql)
        {
            
                MySqlConnection mycon = db_connect.Mysql_con();
                mycon.Open();
                MySqlCommand mycmd = new MySqlCommand(sql, mycon);
                mycmd.ExecuteNonQuery();
                if (mycon != null && mycon.State == ConnectionState.Open)
                {
                    mycon.Close();
                }
            MessageBox.Show("添加成功");

            return;



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window add_bank = new Add_Bank_Question();
            add_bank.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window add_single = new Add_Single_Question();
            add_single.Show();
        }

        private void single_name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}
