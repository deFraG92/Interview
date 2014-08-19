using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private int _deltaX;
        private int _deltaY;
        private Point _questionCoords;

        public QuestionMaker()
        {
           InterView.Start();
        }

        public void QuestionMakerInit(Control baseControl, Point questionCoords)
        {
            _baseControl = baseControl;
            _deltaX = questionCoords.X;
            _deltaY = questionCoords.Y;
            _questionCoords = questionCoords;
            InterView.Init();
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
            var question = InterView.GetQuestion(questionMove);
            return question;
        }

        public void MakeAnswers(object question, QuestionMove questionMove)
        {
            try
            {
                DeleteCurrentQuestionAndSetDefaults();
                var list = InterView.GetAnswersOnQuestion(question);
                SetAnswerSettings(list, questionMove);
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        public void SetAnswer(out bool isTrue)
        {
            isTrue = false;
            var text = ChooseAnswerForQuestion();
            if (text != null)
            {
                DeleteCurrentQuestionAndSetDefaults();
                InterView.SetAnswer(text);
                isTrue = true;
            }
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
            var month = (time.Month.ToString().Length == 1) ? "0" + time.Month.ToString() : time.Month.ToString();
            var date = string.Format("{0}-{1}-{2}", time.Year, month, time.Day);
            InterView.SetBirthDate(date);
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
            foreach (var control in _baseControl.Controls)
            {
                var button = control as RadioButton;
                if (button != null)
                {
                    _baseControl.Controls.Remove((RadioButton) control);
                }
            }
            _deltaX = _questionCoords.X;
            _deltaY = _questionCoords.Y;
        }

        private void SetAnswerSettings(IEnumerable answerList, QuestionMove questionMove)
        {
            foreach (var element in answerList)
            {
                _deltaY += 25;
                var radionButton = new RadioButton()
                {
                    Location = new Point(_deltaX, _deltaY),
                    Text = element.ToString(),
                    Size = new Size(200, 25),
                    Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                    Anchor = AnchorStyles.Bottom
                };
                if (questionMove == QuestionMove.BackWard)
                {
                    if (element == InterView.GetAnswerFromResultScoreList())
                    {
                        radionButton.Checked = true;
                    }
                }
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
