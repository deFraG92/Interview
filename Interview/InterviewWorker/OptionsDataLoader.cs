using System;
using System.Collections.Generic;
using System.Data;
using IDbConnection = Interview.DBWorker.IDbConnection;
using Interview.DBWorker;

namespace Interview.InterviewWorker
{
    public class OptionsDataLoader : DataLoader
    {
        private readonly IDbConnection _dbConnection;
        private readonly bool _isConnected;
        public OptionsDataLoader()
        {
            try
            {
                _dbConnection = SqliteDbConnection.GetSqliteDbWorker();
                _isConnected = _dbConnection.IsConnected();
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        public override DataTable GetDataTable(Enum option)
        {
            if (_isConnected)
            {
                var optionEnum = (OptionName)option;
                switch (optionEnum)
                {
                    case OptionName.QuestionLocation:
                        return GetAdminTools(new[] {"QUESTION_LOCATION_X", "QUESTION_LOCATION_Y"});
                    case OptionName.SpaceBetweenQuestionAndAnswers:
                        return GetAdminTools(new[] {"ANSWER_DELTA_X", "ANSWER_DELTA_Y"});
                    case OptionName.SpaceBetweenAnswers:
                        return GetAdminTools(new[] {"ANSWER_BETWEEN_DISTANCE_X", "ANSWER_BETWEEN_DISTANCE_Y"});
                    case OptionName.HaveBackWard:
                        return GetAdminTools(new[] {"HAVE_BACKWARD"});
                    case OptionName.HaveQuestionsHistory:
                        return GetAdminTools(new[] {"HAVE_QUESTIONS_HISTORY"});
                    case OptionName.HaveFactorsHistory:
                        return GetAdminTools(new[] {"HAVE_FACTORS_HISTORY"});
                    case OptionName.HavePicture:
                        return GetAdminTools(new[] {"HAVE_PICTURES"});
                    case OptionName.PictureLocation:
                        return GetAdminTools(new[] {"PICTURE_LOCATION_X", "PICTURE_LOCATION_Y"});
                    case OptionName.PictureSize:
                        return GetAdminTools(new[] {"PICTURE_SIZE_X", "PICTURE_SIZE_Y"});
                    case OptionName.CanMoveWithoutAnswer:
                        return GetAdminTools(new[] {"CAN_MOVE_WITHOUT_ANSWER"});
                }
            }
            return null;

        }

        public override void SetDataTable(Enum setDataType)
        {
            throw new NotImplementedException();
        }

        private DataTable GetAdminTools(string[] fieldsName)
        {
            try
            {
                var query = "select main.AdminTools.Value " +
                            " from main.AdminTools where ";
                for (int i = 0; i < fieldsName.Length; i ++)
                {
                    query += " main.AdminTools.Option = '" + fieldsName[i] + "'";
                    if (i < fieldsName.Length - 1)
                    {
                        query += " or ";
                    }
                }
                var result = _dbConnection.SelectFromDb(query);
                return result;
            }
            catch (Exception exp)
            {
                throw new Exception("OptionsDataLoader.GetDataTable " + exp);
            }

        }
    }
}
