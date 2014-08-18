using System;
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
        private static Dictionary<Question, Answer> _questionsList;
        private static Dictionary<Question, int> _resultScoreList;
        private static IEnumerator _enumerator;
        private static string _interviewThemeName;
        private static QuestionLoader _questionLoader;
        private static string _respondentName;
       
        public static void Start()
        {
            _questionLoader =
                new QuestionLoaderFromSqliteDb(@"E:\Projects\C#(Windows Forms)\InterView\Interview\Interview");
        }

        public static void Init()
        {
            _questionsList = new Dictionary<Question, Answer>();
            _resultScoreList = new Dictionary<Question, int>();
            QuestionsAndAnswersInit();
            _enumerator = _questionsList.GetEnumerator();
            
        }

        public static string GetNextQuestion()
        {
            if (!_questionsList.Equals(null))
            {
                try
                {
                    var moveNext = _enumerator.MoveNext();
                    if (moveNext)
                    {
                        var result = ((KeyValuePair<Question, Answer>) _enumerator.Current).Key.Name;
                        return result;
                    }
                    return null;
                }

                catch (Exception exp)
                {
                    throw new Exception(exp.ToString());
                }
            }
            throw new ArgumentNullException();
        }

        public static void SetAnswer(object answer)
        {
            try
            {
                var currAnswer = ((KeyValuePair<Question, Answer>) _enumerator.Current);
                                var result = currAnswer.Value.ScoreList[answer];
                _resultScoreList.Add(currAnswer.Key, result);
                _questionLoader.SetDataTable(SetDataType.AnswerResult);
                 
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        public static IEnumerable GetAnswersOnQuestion(object answer)
        {
            var currAnswer = ((KeyValuePair<Question, Answer>) _enumerator.Current);

            var result = currAnswer.Value.ScoreList.Keys.ToList();
            return result;
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

        public static Dictionary<Question, Answer> GetQuestionsAndAnswers()
        {
            return _questionsList;
        }

        public static Answer GetAnswerByQuestion(Question question)
        {
            if (_questionsList.ContainsKey(question))
            {
                return _questionsList[question];
            }
            return null;
        }

        public static KeyValuePair<Question, Answer> GetCurrentQuestionAndAnswer()
        {
            return ((KeyValuePair<Question, Answer>)_enumerator.Current);
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

        private static void QuestionsAndAnswersInit()
        {
            if ((_interviewThemeName != null) & (_interviewThemeName != string.Empty))
            {
                var dataTable = _questionLoader.GetDataTable(GetDataType.QuestionsAnswersScores);
                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    var question = new Question() {Name = dataTable.Rows[i][0].ToString()};
                    var answer = new Answer() {ScoreList = new Dictionary<object, int>()};

                    if (!_questionsList.ContainsKey(question))
                    {
                        answer.ScoreList.Add(dataTable.Rows[i][1], (int) dataTable.Rows[i][2]);
                        _questionsList.Add(question, answer);
                    }
                    else
                    {
                        _questionsList[question].ScoreList.Add(dataTable.Rows[i][1], (int) dataTable.Rows[i][2]);
                    }
                }
            }
        }
    }
}
