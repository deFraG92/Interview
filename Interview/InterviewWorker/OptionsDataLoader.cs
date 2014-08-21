using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
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
            var optionEnum = (OptionName)option;
            switch (optionEnum)
            {
                case OptionName.QuestionLocation:
                    return GetDatableByName(new[] { "QUESTION_LOCATION_X", "QUESTION_LOCATION_Y" });
                case OptionName.SpaceBetweenQuestionAndAnswers:
                    return GetDatableByName(new[] { "ANSWER_DELTA_X", "ANSWER_DELTA_Y" });
                case OptionName.SpaceBetweenAnswers:
                    return GetDatableByName(new[] {"ANSWER_BETWEEN_DISTANCE_X", "ANSWER_BETWEEN_DISTANCE_Y"});
            }
            return null;

        }

        public override void SetDataTable(Enum setDataType)
        {
            throw new NotImplementedException();
        }

        private DataTable GetAllAdminTools()
        {
            if (_isConnected)
            {
                try
                {
                    const string query = "select * from main.AdminTools";
                    var result = _dbConnection.SelectFromDb(query);
                    return result;
                }
                catch (Exception exp)
                {
                    throw new Exception("OptionsDataLoader.GetDataTable " + exp);
                }
            }
            return null;
        }

        private DataTable GetDatableByName(string[] name)
        {
            if (name.Length > 0)
            {
                try
                {
                    var resultTable = new DataTable();
                    var dataColumn = new DataColumn() {DataType = typeof(string)};
                    resultTable.Columns.Add(dataColumn);
                    foreach (string elem in name)
                    {
                        var needElem = _adminTools.Rows.Find(elem)[1];
                        var newRow = resultTable.NewRow();
                        newRow[0] = needElem;
                        resultTable.Rows.Add(newRow);
                    }
                    return resultTable;
                }
                catch (Exception exp)
                {
                    throw new Exception("GetDatableByName " + exp);
                }
            }
            return null;
        }

    }
}
