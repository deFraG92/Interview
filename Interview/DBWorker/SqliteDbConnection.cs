using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interview.DBWorker
{
    public class SqliteDbConnection : IDbConnection
    {
        private bool _isConnected;
        private SQLiteConnection _sqLiteConnection;
        private static SqliteDbConnection _sqliteDbConnection;
        
        private SqliteDbConnection()
        {

        }

        public static SqliteDbConnection GetSqliteDbWorker()
        {
            if (_sqliteDbConnection == null)
            {
                _sqliteDbConnection = new SqliteDbConnection();
            }
            return _sqliteDbConnection;
        }

        public bool ConnectToDb(string tns)
        {
            try
            {
                _sqLiteConnection = new SQLiteConnection(string.Format("Data Source={0}",tns));
                _sqLiteConnection.Open();
                _isConnected = true;
            }
            catch (Exception exp)
            {
                _isConnected = false;
            }
            return _isConnected;
        }

        public bool DmlOperation(string query)
        {
            if (_isConnected)
            {
                try
                {
                    var command = new SQLiteCommand(query, _sqLiteConnection);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.ToString());
                }
            }
            return false;
        }

        public DataTable SelectFromDb(string query)
        {
            if (_isConnected)
            {
                try
                {
                    var command = new SQLiteCommand(query, _sqLiteConnection);
                    var reader = command.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.ToString());
                }
            }
            return null;
        }

        public bool DisconnectFromDb()
        {
            if (_isConnected)
            {
                try
                {
                    _sqLiteConnection.Close();
                    return true;
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.ToString());
                }

            }
            return false;
        }
    }
}
