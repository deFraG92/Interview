﻿using System;
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
        private Graphics _graphics;
        private Image _img;

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
            if (Options.HavePictures)
            {
                _baseControl.Paint += new PaintEventHandler(_baseControl_Paint);
                _graphics = _baseControl.CreateGraphics();
            }
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
            if (Options.HavePictures)
            {
                _img = InterView.GetImageOnCurrQuestion();
                _baseControl.Invalidate();
            }
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
            if ( (!Options.CanMoveWithoutAnswer) & (text == null) )
            {
                MessageBox.Show("Выберите ответ!");
                return false;
            }
            InterView.SetAnswer(text);
            return true;
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
            var spaceBetweenQuestionAndPicture = Options.PictureLocation;
            //only vertical dependence
            spaceBetweenQuestionAndPicture.X = 0;
            Point pictureSize;
            if (!Options.HavePictures)
            {
                spaceBetweenQuestionAndPicture = new Point(0, 0);
            }
            if (_img != null)
            {
                pictureSize = new Point(_img.Width, _img.Height);
            }
            else
            {
                pictureSize = new Point(0, 0);
            }
            //
            foreach (var element in answerList)
            {
                var radionButton = new RadioButton()
                {
                    Location = new Point(_questionCoords.X + spaceBetweenQuestionAndPicture.X + spaceBetweenQuestionAndAnswers.X,
                                         _questionCoords.Y + spaceBetweenQuestionAndPicture.Y + pictureSize.Y +
                                          spaceBetweenQuestionAndAnswers.Y),
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

        

        private void _baseControl_Paint(object sender, PaintEventArgs e)
        {
            if (_img != null)
            {
                var newLocation = new Point(_questionCoords.X + Options.PictureLocation.X,
                    _questionCoords.Y + Options.PictureLocation.Y);
                _graphics.DrawImage(_img, newLocation);
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
