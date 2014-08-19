﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace Interview.InterviewWorker
{
    public static class InterView
    {
        private static OrderedDictionary _questionsList;
        private static Dictionary<Question, int> _resultScoreList;
        private static string _interviewThemeName;
        private static DataLoader _questionLoader;
        private static string _respondentName;
        private static string _respondentBirthDate;
        private static Question[] _questions;
        private static int _questionNumber = -1;

        public static void Start()
        {
            _questionLoader =
                new BaseDataLoader(@"E:\Projects\C#(Windows Forms)\Interview(GIT)\Interview\Interview");
        }

        public static void Init()
        {
            _questionsList = new OrderedDictionary();
            _resultScoreList = new Dictionary<Question, int>();
            QuestionsAndAnswersInit();
        }

        public static string GetQuestion(QuestionMove questionMove)
        {
            try
            {
                if (questionMove == QuestionMove.Forward)
                    _questionNumber++;
                if (questionMove == QuestionMove.BackWard)
                    _questionNumber--;
                return (_questions.Length > _questionNumber) & (_questionNumber >= 0) ? _questions[_questionNumber].Name : null;
            }
            catch (Exception exp)
            {
                throw new Exception("GetQuestion: " + exp);
            }
        }

        public static IEnumerable GetAnswersOnQuestion(object answer)
        {
            try
            {
                return ((Answer)_questionsList[_questionNumber]).ScoreList.Keys.ToList();
            }
            catch (Exception exp)
            {
                throw new Exception("GetAnswersOnQuestion: " + exp);
            }
        }

        public static void SetAnswer(object answer)
        {
            try
            {
                var answerScore = ((Answer) _questionsList[_questionNumber]).ScoreList[answer];
                if (!_resultScoreList.ContainsKey(_questions[_questionNumber]))
                {
                    _resultScoreList.Add(_questions[_questionNumber], answerScore);
                    //_questionLoader.SetDataTable(SetDataType.AnswerResult);
                }
                else
                {
                    if (_resultScoreList[_questions[_questionNumber]] != answerScore)
                    {
                        _resultScoreList[_questions[_questionNumber]] = answerScore;
                        //update
                    }
                }
            }
            catch (Exception exp)
            {
                throw new Exception("SetAnswer: " + exp);
            }
        }

        public static DataTable GetInterviewThemes()
        {
            return _questionLoader.GetDataTable(GetDataType.Theme);
        }

        public static void SetInterviewTheme(string themeName)
        {
            _interviewThemeName = themeName;
        }

        public static string GetInterviewTheme()
        {
            return _interviewThemeName;
        }

        public static IEnumerable GetQuestionsAndAnswers()
        {
            return _questionsList;
        }

        public static Answer GetAnswerByQuestion(Question question)
        {
            if (_questionsList.Contains(question))
            {
                return (Answer)_questionsList[question];
            }
            return null;
        }

        public static KeyValuePair<Question, Answer> GetCurrentQuestionAndAnswer()
        {
            return new KeyValuePair<Question, Answer>(_questions[_questionNumber], (Answer)_questionsList[_questionNumber]);
        }

        public static int GetScoreByQuestion(Question question)
        {
            if (_resultScoreList.ContainsKey(question))
            {
                return _resultScoreList[question];
            }
            return -1;
        }

        public static string GetRespondentName()
        {
            return _respondentName;
        }

        public static void SetRespondentName(string respondentName)
        {
            _respondentName = respondentName;
        }

        public static int GetQuestionIndex()
        {
            return _questionNumber;
        }

        public static object GetAnswerFromResultScoreList()
        {
            var answerScore = _resultScoreList[_questions[_questionNumber]];
            var answerScoreList = ((Answer) _questionsList[_questionNumber]).ScoreList;
            foreach (var score in answerScoreList.Keys)
            {
                if (answerScoreList[score] == answerScore)
                {
                    return score;
                }
            }
            return null;
        }

        public static string GetBirthDate()
        {
            return _respondentBirthDate;
        }
        
        public static void SetBirthDate(string birthDate)
        {
            _respondentBirthDate = birthDate;
        }


        private static void QuestionsAndAnswersInit()
        {
            if ((_interviewThemeName != null) & (_interviewThemeName != string.Empty))
            {
                var dataTable = _questionLoader.GetDataTable(GetDataType.QuestionsAnswersScores);
                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    var question = new Question() {Name = dataTable.Rows[i][0].ToString()};
                    var answer = new Answer() {ScoreList = new Dictionary<object, int>()};

                    if (!_questionsList.Contains(question))
                    {
                        answer.ScoreList.Add(dataTable.Rows[i][1], (int) dataTable.Rows[i][2]);
                        _questionsList.Add(question, answer);
                    }
                    else
                    {
                        ((Answer)_questionsList[question]).ScoreList.Add(dataTable.Rows[i][1], (int) dataTable.Rows[i][2]);
                    }
                }
                _questions = new Question[_questionsList.Count];
                _questionsList.Keys.CopyTo(_questions, 0);
            }
        }
    }
}
