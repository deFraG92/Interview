using System;
using System.Drawing;
using System.Windows.Forms;
using Interview.InterviewWorker;


namespace Interview
{
    public partial class InterviewWindow : Form
    {
        private readonly QuestionMaker _questionMaker;
        public InterviewWindow(QuestionMaker questionMaker)
        {
            InitializeComponent();
            _questionMaker = questionMaker;
            _questionMaker.QuestionsAndAnswersInit(this);
            Question_Lbl.Location = Options.QuestionLocation;
            Prev_But.Visible = Options.HaveBackward;
            GetQuestionAndAnswers(QuestionMove.Forward);
        }

        private void Next_But_Click(object sender, EventArgs e)
        {
            var isTrue = _questionMaker.SetAnswer();
            if (isTrue)
            {
                GetQuestionAndAnswers(QuestionMove.Forward);
            }
        }

        private void Prev_BUT_Click(object sender, EventArgs e)
        {
            GetQuestionAndAnswers(QuestionMove.BackWard);
        }

        private void GetQuestionAndAnswers(QuestionMove questionMove)
        {
            var question = _questionMaker.GetQuestion(questionMove);
            if (question == null)
            {
                FactorAnalize.FactorAnalizeInit("");
                Close();
            }
            else
            {
                Question_Lbl.Text = question.ToString();
                var questionLocation = new Point(Question_Lbl.Location.X, Question_Lbl.Location.Y + Question_Lbl.Height);
                _questionMaker.ChangeQuestionCoords(questionLocation);
                _questionMaker.MakeAnswers(question, questionMove);
            }
        }
    }
}
