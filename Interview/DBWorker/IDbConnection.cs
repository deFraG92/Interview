using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace Interview.DBWorker
{
    public interface IDbConnection
    {
        bool ConnectToDb(string tns);
        bool DmlOperation(string query);
        DataTable SelectFromDb(string query);
        bool DisconnectFromDb();
        bool IsConnected();
    }
}
