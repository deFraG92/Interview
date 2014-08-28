using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.IO;

namespace Interview.DBWorker
{
    public static class DbImageLoader
    {
        private static IDbConnection _dbConnection;
        private static bool _isConnected;
        public static bool ConnectToDb(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            if (_dbConnection.IsConnected())
            {
                _isConnected = true;
            }
            return _isConnected;
        }

        public static bool ConnectToDb(string tns)
        {
            try
            {
                if (_dbConnection == null)
                {
                    _dbConnection = SqliteDbConnection.GetSqliteDbWorker();
                    _isConnected = _dbConnection.ConnectToDb(tns);
                }
            }
            catch (Exception exp)
            {
                throw new Exception("ConnectToDb " + exp);
            }
            return _isConnected;
        }
        
        public static byte[] GetBytePictureFromFile(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(stream);
                var fileInfo = new FileInfo(filePath).Length;
                var imgBytes = reader.ReadBytes((Int32) fileInfo);
                stream.Close();
                reader.Close();
                return imgBytes;
            }
        }

        public static byte[] GetBytePictureFromDb(string query)
        {
            if (_isConnected)
            {
                var imgTable = _dbConnection.SelectFromDb(query);
                if (imgTable.Rows.Count > 0)
                {
                    var img = imgTable.Rows[0][0];
                    try
                    {
                        var imgBytes = (Byte[]) img;
                        return imgBytes;
                    }
                    catch (Exception exp)
                    {
                        throw new Exception("GetBytePictureFromDb " + exp);
                    }
                }
            }
            return null;
        }

        public static void SetBytePictureToDb(string query, string fieldName, byte[] imgBytes)
        {
            if (_isConnected)
            {
                try
                {
                    var connect = _dbConnection.GetConnection();
                    using (var command = connect.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.Add("@" + fieldName, DbType.Binary, imgBytes.Length).Value = imgBytes;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("SetBytePictureToDb " + exp);
                }
            }
        }

        public static Image GetImageFromImgBytes(byte[] imgBytes, Size imgSize)
        {
            using (var stream = new MemoryStream(imgBytes))
            {
                var img = Image.FromStream(stream);
                if ((img.Size.Width > imgSize.Width) | (img.Size.Height > imgSize.Height))
                {
                    var sizedImg = new Bitmap(img, imgSize);
                    return sizedImg;
                }
                return img;
            }
        }
    }
}
