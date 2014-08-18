using System;
using System.Data;
using IDbConnection = Interview.DBWorker.IDbConnection;
using Interview.DBWorker;

namespace Interview.InterviewWorker
{
    public class QuestionLoaderFromSqliteDb : QuestionLoader
    {
        private readonly IDbConnection _dbConnection;
        private readonly bool _isConnected;
        public QuestionLoaderFromSqliteDb(string tns)
        {
            try
            {
                _dbConnection = SqliteDbConnection.GetSqliteDbWorker();
                _isConnected = _dbConnection.ConnectToDb(tns);
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        public override DataTable GetDataTable(GetDataType getDataType)
        {
            if (_isConnected)
            {
                if ((getDataType == GetDataType.Answers) |
                    (getDataType == GetDataType.Questions) |
                    (getDataType == GetDataType.QuestionsAnswersScores) |
                    (getDataType == GetDataType.Theme))
                {
                    return GetQuestionsAnswersOrResults(getDataType);
                }
                if (getDataType == GetDataType.RespondentId)
                {
                    return CheckRespondentAndGetId(InterView.GetRespondentName());
                }
            }
            return null;

        }

        public override void SetDataTable(SetDataType setDataType)
        {
            if (_isConnected)
            {
                if (setDataType == SetDataType.AnswerResult)
                {
                    InsertAnswerResult();
                }
            }
        }


        private DataTable GetQuestionsAnswersOrResults(GetDataType getDataType)
        {
            var selectionField = string.Empty;
            // SELECT BLOCK
            var query = "Select distinct ";
            switch (getDataType)
            {
                case GetDataType.Answers:
                    selectionField = "main.Answers.Name";
                    break;
                case GetDataType.Questions:
                    selectionField = "main.Questions.Name";
                    break;
                case GetDataType.QuestionsAnswersScores:
                    selectionField = "main.Questions.Name, main.Answers.Name, main.Interview.Score";
                    break;
                case GetDataType.Theme:
                    selectionField = "main.Themes.Name";
                    break;
            }
            if (selectionField != string.Empty)
            {
                // FROM BLOCK
                query += selectionField + " from main.Interview, " +
                         "main.Questions, " +
                         "main.Answers, " +
                         "main.Themes ";
                // WHERE BLOCK    
                query += "where main.Interview.QUESTION_ID = main.Questions.ID " +
                         "and main.Interview.ANSWER_ID = main.Answers.ID " +
                         "and main.Interview.theme_id = main.Themes.ID";

                if (getDataType != GetDataType.Theme)
                {
                    query += " and main.Themes.Name = " + "'" + InterView.GetInterviewTheme() + "'";
                }
                query += " order by main.Questions.ID, main.Answers.ID ";
                try
                {
                    var result = _dbConnection.SelectFromDb(query);
                    return result;
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.ToString());
                }
            }
            return null;
                        
        }

        private DataTable GetInteviewId(Question question, int score)
        {
            var query = "select distinct main.Interview.id " +
                        " from main.Interview, main.Themes, main.Questions ";

            query += " where main.Questions.Name = '" + question.Name + "'" +
                     " and main.Interview.score = '" + score + "'" +
                     " and main.Themes.Name = '" + InterView.GetInterviewTheme() + "'" + 
                     " and main.Interview.theme_id = main.Themes.Id " +
                     " and main.Interview.Question_id = main.Questions.id ";

            try
            {
                var result = _dbConnection.SelectFromDb(query);
                return result;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        private void InsertAnswerResult()
        {
            var currQuestion = InterView.GetCurrentQuestionAndAnswer().Key;
            var interviewIdRow = GetInteviewId(currQuestion, InterView.GetScoreByQuestion(currQuestion));
            if (interviewIdRow.Rows.Count > 0)
            {
                var interviewId = interviewIdRow.Rows[0][0];
                var newAnswerResultId = GetNewTableIndexId("main.AnswerResults");
                var newRespondentId = CheckOrInsertRespondentAndGetId();
                var query = "insert into main.AnswerResults(id, respondent_id, interview_id)" +
                            " values('" + newAnswerResultId + "', '" + newRespondentId + "', '" + interviewId + "')";
                _dbConnection.DmlOperation(query);
            }
        }

        private DataTable CheckRespondentAndGetId(string respondentName)
        {
            var query = "select main.Respondents.Id " +
                        " from main.Respondents " +
                        " where trim(main.Respondents.FIO) = '" + respondentName + "'";
            var result = _dbConnection.SelectFromDb(query);
            return result;

        }

        private int CheckOrInsertRespondentAndGetId()
        {
            var respondentIdRow = CheckRespondentAndGetId(InterView.GetRespondentName());
            if (respondentIdRow.Rows.Count == 0)
            {
                var respondentId = GetNewTableIndexId("main.Respondents");
                if (respondentId != 0)
                {
                    var query = "insert into main.Respondents(id, FIO) values( '" + respondentId +
                                "', '" + InterView.GetRespondentName() + "')";
                    _dbConnection.DmlOperation(query);
                    return respondentId;
                }
            }
            return Convert.ToInt16(respondentIdRow.Rows[0][0]);

        }

        private int GetNewTableIndexId(string tableName)
        {
            var query = "select max(id) from " + tableName;
            var result = _dbConnection.SelectFromDb(query).Rows[0][0];
            if (result.ToString() != "")
                return Convert.ToInt16(result) + 1;
            return 1;
        }

    }

}
