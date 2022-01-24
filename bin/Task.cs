using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoWantsToBeAMillionaire
{
    public class Task
    {
        public Task(string question, string ans0, string ans1, string ans2, string ans3, string correct)
        {
            this.question = question;
            this.ans0 = ans0;
            this.ans1 = ans1;
            this.ans2 = ans2;
            this.ans3 = ans3;
            this.correct = correct;
        }

        private string question;

        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        private string ans0;

        public string Ans0
        {
            get { return ans0; }
            set { ans0 = value; }
        }

        private string ans1;

        public string Ans1
        {
            get { return ans1; }
            set { ans1 = value; }
        }

        private string ans2;

        public string Ans2
        {
            get { return ans2; }
            set { ans2 = value; }
        }

        private string ans3;

        public string Ans3
        {
            get { return ans3; }
            set { ans3 = value; }
        }

        private string correct;

        public string Correct
        {
            get { return correct; }
            set { correct = value; }
        }

    }
}
