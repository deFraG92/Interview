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

        public override DataTable GetDataTable(GetDataType getDataType)
        {
            if (_isConnected)
            {
                try
                {
                    var query = "select * from main.AdminTools";
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

        public override void SetDataTable(SetDataType setDataType)
        {
            throw new NotImplementedException();
        }
    }
}
