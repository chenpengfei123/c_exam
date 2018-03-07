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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Control
{
    /// <summary>
    /// ScoreManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreManagerControl : UserControl
    {
        string sql_score;
        DataTable score_table;
        public ScoreManagerControl()
        {
            InitializeComponent();
            ShowScore();
        }

        private void ShowScore()
        {
            sql_score = "select * from exam_score";
            score_table = db_connect.GetTables(sql_score);
            score_table.Columns[0].ColumnName = "学号";
            score_table.Columns[1].ColumnName = "姓名";
            score_table.Columns[2].ColumnName = "试卷名称";
            score_table.Columns[3].ColumnName = "选择题得分";
            score_table.Columns[4].ColumnName = "填空题得分";
            score_table.Columns[5].ColumnName = "总分";
            getscores.ItemsSource = score_table.DefaultView;
        }
        private void RefreshScore_Click(object sender, RoutedEventArgs e)
        {
            ShowScore();
        }
        private void AnswerManager_Click(object sender, RoutedEventArgs e)
        {
            Answer_Manager answer = new Answer_Manager();
            answer.Owner = Window.GetWindow(this);
            answer.ShowDialog();
        }

    }
}
