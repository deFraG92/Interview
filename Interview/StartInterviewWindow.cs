using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Interview.InterviewWorker;

namespace Interview
{
    public partial class StartInterviewWindow : Form
    {
        private readonly QuestionMaker _questionMaker;
        public StartInterviewWindow(QuestionMaker questionMaker)
        {
            InitializeComponent();
            _questionMaker = questionMaker;
            ContinueInterview_But.Enabled = _questionMaker.GetInterviewCompleteness();
        }

        private void StartNewInterview_But_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ContinueInterview_But_Click(object sender, EventArgs e)
        {
            _questionMaker.QuestionPositionInit();
            Close();
        }
    }
}
