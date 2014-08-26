﻿using System;
using System.Data;
using System.Data.Odbc;
using IDbConnection = Interview.DBWorker.IDbConnection;
using Interview.DBWorker;

namespace Interview.InterviewWorker
{
    public class BaseDataLoader : DataLoader
    {
        private readonly IDbConnection _dbConnection;
        private readonly bool _isConnected;
        private int _respondentId = 0;
        private int _interviewThemeId;
        private int _nextInterviewNum = 0;
        public BaseDataLoader(string tns)
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

        public override DataTable GetDataTable(Enum getDataEnum)
        {
            if (_isConnected)
            {
                var getDataType = (GetDataType) getDataEnum;
                if ((getDataType == GetDataType.Answers) |
                    (getDataType == GetDataType.Questions) |
                    (getDataType == GetDataType.QuestionsAnswersScores) |
                    (getDataType == GetDataType.Theme))
                {
                    return GetQuestionsAnswersOrResults(getDataType);
                }
                if (getDataType == GetDataType.InterviewCompleteness)
                {
                    return CheckForInterviewCompleteness();
                }
                if (getDataType == GetDataType.FirstQuestionId)
                {
                    return GetFirstQuestionId();
                }
                if (getDataType == GetDataType.AnswerScores)
                {
                    return GetScoresFromAnswerResults();
                }
            }
            return null;

        }

        public override void SetDataTable(Enum setDataEnum)
        {
            if (_isConnected)
            {
                var setDataType = (SetDataType) setDataEnum;
                if (setDataType == SetDataType.BaseOptionsInit)
                {
                    BaseOptionsInit();
                }
                if ( (setDataType == SetDataType.AnswerResultInsert)   |
                    (setDataType == SetDataType.AnswerResultUpdate) )
                {
                    InsertAnswerResult(setDataType);
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

        private void InsertAnswerResult(SetDataType dataType)
        {
            var currQuestion = InterView.GetCurrentQuestionAndAnswer().Key;
            var interviewIdRow = GetInteviewId(currQuestion, InterView.GetScoreByQuestion(currQuestion));
            if (interviewIdRow.Rows.Count > 0)
            {
                var interviewId = interviewIdRow.Rows[0][0];
                if (dataType == SetDataType.AnswerResultInsert)
                {
                    AddAnswerResult(interviewId);
                }
                else
                {
                    UpdateAnswerResult(currQuestion, interviewId);
                }
            }
        }

        private void AddAnswerResult(object interviewId)
        {
            var newAnswerResultId = GetNewTableIndexId("main.AnswerResults");
            var query = "insert into main.AnswerResults(id, respondent_id, interview_id, interview_number)" +
                        " values('" + newAnswerResultId + "', '" + _respondentId + "', '" + interviewId + "', '" + _nextInterviewNum + "')";
            try
            {
                _dbConnection.DmlOperation(query);
            }
            catch (Exception exp)
            {
                throw new Exception("AddAnswerResult " + exp);
            }
        }

        private void UpdateAnswerResult(Question currQuestion, object interviewId)
        {
            var query = "select max(AnswerResults.id) " +
                        " from main.AnswerResults, " +
                        " main.Interview," +
                        " main.Questions " +
                        " where Interview.theme_id = '" + _interviewThemeId + "'" +
                        " and AnswerResults.respondent_id = '" + _respondentId + "' " +
                        " and Interview.id = AnswerResults.interview_id " +
                        " and Interview.question_id = Questions.id " +
                        " and Questions.Name = '" + currQuestion.Name + "'";
            try
            {
                var answerResultRow = _dbConnection.SelectFromDb(query);
                if (answerResultRow.Rows[0][0].ToString() != "")
                {
                    var answerResultId = answerResultRow.Rows[0][0];
                    if (InterView.GetHaveHistory())
                    {
                        query = " update main.AnswerResults" +
                                " set Interview_id = '" + interviewId + "'" +
                                " where id = '" + answerResultId + "'";
                    }
                    else
                    {
                        var nextInterviewNum = InterView.GetInterviewCompleteness() ? _nextInterviewNum : _nextInterviewNum + 1;
                            query = " update main.AnswerResults" +
                                    " set Interview_id = '" + interviewId + "'" +
                                    ", interview_number = '" + nextInterviewNum + "'" +
                                    " where id = '" + answerResultId + "'";
                    }
                    _dbConnection.DmlOperation(query);
                }
                else
                {
                    AddAnswerResult(interviewId);
                }
            }
            catch (Exception exp)
            {
                throw new Exception("UpdateAnswerResult" + exp);
            }
        }

        private DataTable CheckRespondentAndGetId(string respondentName, string birthDay)
        {
            var query = "select main.Respondents.Id " +
                        " from main.Respondents " +
                        " where trim(main.Respondents.FIO) = '" + respondentName + "'" +
                        " and main.Respondents.birthday = '" + birthDay + "'";
            try
            {
                var result = _dbConnection.SelectFromDb(query);
                return result;
            }
            catch (Exception exp)
            {
                throw new Exception("CheckRespondentAndGetId " + exp);
            }
        }
        
        private int CheckOrInsertRespondentAndGetId()
        {
            var respondentIdRow = CheckRespondentAndGetId(InterView.GetRespondentName(), InterView.GetBirthDate());
            if (respondentIdRow.Rows.Count == 0)
            {
                var respondentId = GetNewTableIndexId("main.Respondents");
                if (respondentId != 0)
                {
                    var query = "insert into main.Respondents(id, FIO, birthday) values( '" + respondentId +
                                "', '" + InterView.GetRespondentName() + "', '" + InterView.GetBirthDate() + "')";
                    _dbConnection.DmlOperation(query);
                    return respondentId;
                }
            }
            return Convert.ToInt16(respondentIdRow.Rows[0][0]);

        }

        private int GetNewTableIndexId(string tableName)
        {
            var query = "select max(id) from " + tableName;
            try
            {
                var result = _dbConnection.SelectFromDb(query).Rows[0][0];
                if (result.ToString() != "")
                    return Convert.ToInt16(result) + 1;
                return 1;
            }
            catch (Exception exp)
            {
                throw new Exception("GetNewTableIndexId " + exp);
            }
            
        }

        private int GetMaxInterviewNum()
        {
            var query = " select max(AnswerResults.Interview_number) " +
                                " from main.AnswerResults," +
                                     " main.Interview " +
                                " where Interview.theme_id = '" + _interviewThemeId + "'" +
                                      " and AnswerResults.respondent_id = '" + _respondentId + "' " +
                                      " and Interview.id = AnswerResults.interview_id ";
            try
            {
                var maxInterviewNum = _dbConnection.SelectFromDb(query);
                if (maxInterviewNum.Rows[0][0].ToString() != "")
                {
                    return Convert.ToInt32(maxInterviewNum.Rows[0][0]);
                }
                
            }
            catch (Exception exp)
            {
                throw new Exception("GetMaxInterviewNum " + exp);
            }
            return 0;
        }

        private int GetInterviewThemeId(string interviewTheme)
        {
            var query = " select Themes.Id " +
                          " from main.Themes " +
                          " where Themes.Name = '" + interviewTheme + "'";
            try
            {
                var themeRow = _dbConnection.SelectFromDb(query);
                if (themeRow.Rows.Count > 0)
                {
                    return Convert.ToInt32(themeRow.Rows[0][0]);
                }
                return 0;
            }
            catch (Exception exp)
            {
                throw new Exception("GetInterviewThemeId: " + exp);
            }
        }

        private void BaseOptionsInit()
        {
            _respondentId = CheckOrInsertRespondentAndGetId();
            _interviewThemeId = GetInterviewThemeId(InterView.GetInterviewTheme());
            var maxInterviewNum = GetMaxInterviewNum();
            _nextInterviewNum = maxInterviewNum == 0 ? 1 : maxInterviewNum;
        }


        private DataTable CheckForInterviewCompleteness()
        {
            try
            {
                var query = "select AnswerResults.id " +
                            " from main.AnswerResults, " +
                            " main.Interview " +
                            " where Interview.theme_id = '" + _interviewThemeId + "'" +
                            " and AnswerResults.respondent_id = '" + _respondentId + "' " +
                            " and Interview.id = AnswerResults.interview_id ";
                var check = _dbConnection.SelectFromDb(query);
                if (check.Rows.Count == 0)
                {
                    check.Columns.Add(new DataColumn() {DataType = typeof (int)});
                    var newRow = check.NewRow();
                    newRow[0] = 0;
                    check.Rows.Add(newRow);
                    return check;
                }
                query = " select " +
                        "( " +
                        " select count(distinct Interview.question_id) " +
                        " from main.Interview " +
                        " where Interview.theme_id = '" + _interviewThemeId + "'" +
                        ") = " +
                        "( " +
                        " select count(AnswerResults.interview_id) " +
                        " from main.AnswerResults, " +
                        " main.Interview " +
                        " where AnswerResults.interview_id = Interview.id " +
                        " and Interview.theme_id = '" + _interviewThemeId + "'" +
                        " and AnswerResults.respondent_id = '" + _respondentId + "' " +
                        " and AnswerResults.Interview_number = '" + _nextInterviewNum + "'" +
                        " )";
                var identityRow = _dbConnection.SelectFromDb(query);
                var identity = Convert.ToInt32(identityRow.Rows[0][0]);
                if (identity == 0)
                {
                    identityRow.Rows[0][0] = 1;
                }
                else
                {
                    identityRow.Rows[0][0] = 0;
                    if (InterView.GetHaveHistory())
                    {
                        _nextInterviewNum++;
                    }
                }
                return identityRow;
            }
            catch (Exception exp)
            {
                throw new Exception("GetLastQuestionId " + exp);
            }
        }

        private DataTable GetFirstQuestionId()
        {
            var query = "select max(Interview.question_id) " +
                        " from main.AnswerResults, " +
                             " main.Interview " +
                        " where AnswerResults.interview_id = Interview.id " +
                              " and Interview.theme_id = '" + _interviewThemeId + "'" +
                              " and AnswerResults.respondent_id = '" + _respondentId + "' " +
                              " and AnswerResults.interview_number = '" + _nextInterviewNum + "'";
            try
            {
                var result = _dbConnection.SelectFromDb(query);
                return result;
            }
            catch (Exception exp)
            {
                throw new Exception("GetFirstQuestionId " + exp);
            }
        }

        private DataTable GetScoresFromAnswerResults()
        {
            var query = " select Questions.Name, Interview.Score " +
                        " from main.AnswerResults, " +
                             " main.Interview, " +
                             " main.Questions " +
                        " where AnswerResults.interview_id = Interview.id " +
                               " and Interview.question_id = Questions.id " +
                               " and Interview.theme_id = '" + _interviewThemeId + "'" +
                               " and AnswerResults.respondent_id = '" + _respondentId + "' " +
                               " and AnswerResults.interview_number = '" + _nextInterviewNum + "'";
            try
            {
                var result = _dbConnection.SelectFromDb(query);
                return result;
            }
            catch (Exception exp)
            {
                throw new Exception("GetScoresFromAnswerResults " + exp);
            }
        }

    }

}
