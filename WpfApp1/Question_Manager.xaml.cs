﻿using System;
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

namespace WpfApp1
{
    /// <summary>
    /// Question_Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Question_Manager : Window
    {
        string sql_single;
        string sql_bank;
        DataTable dataTable;
        public Question_Manager()
        {

            InitializeComponent();
            sql_single = "select  ques_id,ques_name,ques_answerA,ques_answerB,ques_answerC,ques_answerD,ques_answer from single_question";
            ShowSingleQuestion();

            sql_bank = "select * from bank_question";
            bank_manager.ItemsSource = db_connect.GetTables(sql_bank).DefaultView;
        }

        private void ShowSingleQuestion()
        {
            dataTable = db_connect.GetTables(sql_single);
            dataTable.Columns[0].ColumnName = "题目编号";
            dataTable.Columns[1].ColumnName = "题目名称";
            dataTable.Columns[2].ColumnName = "选项A";
            dataTable.Columns[3].ColumnName = "选项B";
            dataTable.Columns[4].ColumnName = "选项C";
            dataTable.Columns[5].ColumnName = "选项D";
            dataTable.Columns[6].ColumnName = "正确选项";
            single_manager.ItemsSource = dataTable.DefaultView;
        }

        private void AddSingle_Click(object sender, RoutedEventArgs e)
        {
            AddSingleQuestion addSingleQuestion = new AddSingleQuestion();
            addSingleQuestion.Owner = this;
            addSingleQuestion.ShowDialog();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ShowSingleQuestion();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            ChangeSingleQuestion changeSingleQuestion = new ChangeSingleQuestion();
            changeSingleQuestion.Owner = this;
            changeSingleQuestion.ShowDialog();


        }

       

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSingleQuestion deleteSingleQuestion = new DeleteSingleQuestion();
            deleteSingleQuestion.Owner = this;
            deleteSingleQuestion.ShowDialog();
        }
    }
}
