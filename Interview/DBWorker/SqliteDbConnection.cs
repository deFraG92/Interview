using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interview.DBWorker
{
    public class SqliteDbConnection : IDbConnection
    {
        private bool _isConnected;
        private static SQLiteConnection _sqLiteConnection;
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

        public static SQLiteConnection GetSqlLiteConnection()
        {
            return _sqLiteConnection;
        }
        
        public bool ConnectToDb(string tns)
        {
            try
            {
                _sqLiteConnection = new SQLiteConnection(string.Format("Data Source={0}",tns));
                if (_sqLiteConnection.State != ConnectionState.Open)
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
                using (var command = new SQLiteCommand(query, _sqLiteConnection))
                {
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            return false;
        }

        public DataTable SelectFromDb(string query)
        {
            if (_isConnected)
            {
                using (var command = new SQLiteCommand(query, _sqLiteConnection))
                {
                    var reader = command.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
            return null;
        }

        public object SelectScalarFromDb(string query)
        {
            using (var command = new SQLiteCommand(query, _sqLiteConnection))
            {
                var result = command.ExecuteScalar();
                return result;
            }
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

        public bool IsConnected()
        {
            return _isConnected;
        }

        public SQLiteConnection GetConnection()
        {
            return _sqLiteConnection;
        }
    }
}
