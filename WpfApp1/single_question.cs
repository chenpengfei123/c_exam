using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class single_question
    {
        private int ques_id;
        private String ques_name;
        private String ques_answerA;
        private String ques_answerB;
        private String ques_answerC;
        private String ques_answerD;

        public single_question(int ques_id, string ques_name, string ques_answerA, string ques_answerB, string ques_answerC, string ques_answerD)
        {
            this.ques_id = ques_id;
            this.ques_name = ques_name;
            this.ques_answerA = ques_answerA;
            this.ques_answerB = ques_answerB;
            this.ques_answerC = ques_answerC;
            this.ques_answerD = ques_answerD;
        }

        public int Ques_id { get => ques_id; set => ques_id = value; }
        public string Ques_name { get => ques_name; set => ques_name = value; }
        public string Ques_answerA { get => ques_answerA; set => ques_answerA = value; }
        public string Ques_answerB { get => ques_answerB; set => ques_answerB = value; }
        public string Ques_answerC { get => ques_answerC; set => ques_answerC = value; }
        public string Ques_answerD { get => ques_answerD; set => ques_answerD = value; }
    }
}
