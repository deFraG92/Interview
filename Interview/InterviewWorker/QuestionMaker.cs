using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Interview.InterviewWorker
{
    public enum QuestionMove
    {
        Forward,
        BackWard
    }
    
    public class QuestionMaker
    {
        private Control _baseControl;
        private Point _questionCoords;

        public QuestionMaker()
        {
           InterView.Start();
        }

        public bool GetInterviewCompleteness()
        {
            return InterView.GetInterviewCompleteness();
        }

        public void SetInterviewCompleteness()
        {
            InterView.SetInterviewCompleteness();
        }

        public void QuestionPositionInit()
        {
            InterView.QuestionPositionInit();
        }

        public void QuestionsAndAnswersInit(Control baseControl)
        {
            _baseControl = baseControl;
            InterView.InterviewInit();
        }

        public DataTable GetThemeNames()
        {
            return InterView.GetInterviewThemes();
        }

        public bool SetInterviewThemeName(string themeName)
        {
            if (themeName != null)
            {
                InterView.SetInterviewTheme(themeName);
                return true;
            }
            MessageBox.Show("Выберите тему!");
            return false;
        }

        public object GetQuestion(QuestionMove questionMove)
        {
            DeleteCurrentQuestionAndSetDefaults();
            var question = InterView.GetQuestion(questionMove);
            return question;
        }

        public void MakeAnswers(object question, QuestionMove questionMove)
        {
            try
            {
                var list = InterView.GetAnswersOnQuestion(question);
                SetAnswerSettings(list, questionMove);
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        public bool SetAnswer()
        {
            var text = ChooseAnswerForQuestion();
            if (text != null)
            {
                InterView.SetAnswer(text);
                return true;
            }
            return false;
        }

        public bool SetRespondentName(string respondentName)
        {
            if (CheckForValidRespondentName(respondentName))
            {
                InterView.SetRespondentName(respondentName);
                return true;
            }
            MessageBox.Show("Некорректное Ф.И.О.!");
            return false;
        }

        public void SetBirthDate(DateTime time)
        {
            var month = (time.Month.ToString().Length == 1) ? "0" + time.Month : time.Month.ToString();
            var day = (time.Day.ToString().Length == 1) ? "0" + time.Day : time.Day.ToString();
            var date = string.Format("{0}-{1}-{2}", time.Year, month, day);
            InterView.SetBirthDate(date);
        }

        public void ChangeQuestionCoords(Point questionCoords)
        {
            _questionCoords = questionCoords;
        }

        private object ChooseAnswerForQuestion()
        {
            try
            {
                foreach (var control in _baseControl.Controls)
                {
                    var button = control as RadioButton;
                    if (button != null)
                    {
                        var radioControl = button;
                        if (radioControl.Checked)
                        {
                            return radioControl.Text;
                        }
                    }
                }
                MessageBox.Show("Выберите ответ!");
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
            return null;
        }

        private void DeleteCurrentQuestionAndSetDefaults()
        {
            var collection = new List<Control>();
            foreach (var control in _baseControl.Controls)
            {
                var button = control as RadioButton;
                if (button != null)
                {
                    collection.Add(button);
                }
            }
            foreach (var elem in collection)
            {
                if (_baseControl.Controls.Contains(elem))
                {
                    _baseControl.Controls.Remove(elem);
                }
            }
        }

        private void SetAnswerSettings(IEnumerable answerList, QuestionMove questionMove)
        {
            //admintools
            var spaceBetweenQuestionAndAnswers = Options.SpaceBetweenQuestionAndAnswers;
            var spaceBetweenAnswers = Options.SpaceBetweenAnswers;
            //
            foreach (var element in answerList)
            {
                var radionButton = new RadioButton()
                {
                    Location = new Point(_questionCoords.X + spaceBetweenQuestionAndAnswers.X, _questionCoords.Y + spaceBetweenQuestionAndAnswers.Y),
                    Text = element.ToString(),
                    Size = new Size(200, 25),
                    Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                    Anchor = AnchorStyles.Bottom
                };
                if (element == InterView.GetAnswerFromResultScoreList())
                {
                        radionButton.Checked = true;
                }
                spaceBetweenQuestionAndAnswers.X += spaceBetweenAnswers.X;
                spaceBetweenQuestionAndAnswers.Y += spaceBetweenAnswers.Y;
                _baseControl.Controls.Add(radionButton);
            }
        }



        private bool CheckForValidRespondentName(string respondentName)
        {
            //const string pattern = @"^\w{2,}\s\w{2,}|w{1}.\s\w{2,}|w{1}.";
            //respondentName = respondentName.Trim();
            //var isMatch = Regex.IsMatch(respondentName, pattern);
            return true;
        }

        
    }
}
