using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interview.InterviewWorker;

namespace Interview
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var questionMaker = new QuestionMaker();
            Application.Run(new RespondentWindow(questionMaker));
            Application.Run(new StartInterviewWindow(questionMaker));
            Application.Run(new InterviewWindow(questionMaker));
        }
    }
}
