using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;
using System.Data;
using UnityEngine.Networking;
using System.IO;
namespace XrCode
{
    public class SQLiteHelper : Singleton<SQLiteHelper>, ILoad, IDispose
    {
        private SqliteConnection dbConnection;          //数据库连接定义
        private SqliteCommand dbCommand;                //SQL命令定义
        private SqliteDataReader dataReader;            //数据读取定义

        // 初始化连接
        public void InitConnection(string connectionString)
        {
#if UNITY_EDITOR
            try
            {
                D.Log($"[Config]: config path {connectionString}");
                //构造数据库连接   ”URI=file:”
                dbConnection = new SqliteConnection("data source = " + connectionString);
                //打开数据库
                dbConnection.Open();
            }
            catch (Exception e)
            {
                D.Log(e.Message);
            }

#elif UNITY_WEBGL
        Game.Instance.StartCoroutine(LoadConfDataFromStreamintAssets(connectionString));
#endif

        }

        private IEnumerator LoadConfDataFromStreamintAssets(string filePath)
        {
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            D.Error($"[ConfPath]: UnityWebRequest Load start");
            yield return request.SendWebRequest();

            if (request.isDone)
            {
                D.Error($"[ConfPath]: 下载完成 {request.downloadHandler.data.Length}");
            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                byte[] data = request.downloadHandler.data;

                //将二进制文件写入临时文件
                string tempPath = Path.Combine(Application.persistentDataPath, "ConfData.bytes");
                File.WriteAllBytes(tempPath, data);

                //打开数据库链接
                string connectionString = $"Data Source={tempPath};";
                D.Error($"[ConfPath]: {tempPath}");
                dbConnection = new SqliteConnection(connectionString);
                dbConnection.Open();

            }
            D.Error($"[ConfPath]: UnityWebRequest Load end");
        }


        public void Load() { }
        public void Dispose()
        {
            CloseConnection();
        }

        // 执行SQL命令
        public SqliteDataReader ExecuteQuery(string queryString)
        {
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = queryString;
            dataReader = dbCommand.ExecuteReader();
            return dataReader;
        }

        // 读取单行数据
        public SqliteDataReader ReadDataFromTable(string tableName, int id)
        {
            string queryString = "SELECT * FROM " + tableName + " WHERE sn = " + id;
            return ExecuteQuery(queryString);
        }

        public SqliteDataReader ReadDataFromTable(string tableName, string id)
        {
            string queryString = "SELECT * FROM " + tableName + " WHERE sn = " + id;
            return ExecuteQuery(queryString);
        }

        //public DataTable GetDataTable(string tableName)
        //{
        //    string queryString = "SELECT * FROM " + tableName;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dbCommand = dbConnection.CreateCommand();
        //        dbCommand.CommandText = queryString;
        //        SQLiteDataAdapter ad = new SQLiteDataAdapter(dbCommand);
        //        ad.Fill(dt);
        //    }
        //    catch (Exception e)
        //    {
        //        Log(e.Message);
        //    }
        //    return dt;
        //}

        // 读取整张数据表
        public SqliteDataReader ReadFullTable(string tableName)
        {
            string queryString = "SELECT * FROM " + tableName;
            return ExecuteQuery(queryString);
        }

        // 向指定数据表中插入数据
        public SqliteDataReader InsertValues(string tableName, string[] values)
        {
            //获取数据表中字段数目
            int fieldCount = ReadFullTable(tableName).FieldCount;
            //当插入的数据长度不等于字段数目时引发异常
            if (values.Length != fieldCount)
            {
                throw new SqliteException("values.Length!=fieldCount");
            }

            string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
            for (int i = 1; i < values.Length; i++)
            {
                queryString += ", " + values[i];
            }
            queryString += " )";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="key">关键字</param>
        /// <param name="value">关键字对应的值</param>
        public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length)
            {
                throw new SqliteException("colNames.Length!=colValues.Length");
            }

            string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += ", " + colNames[i] + "=" + colValues[i];
            }
            queryString += " WHERE " + key + operation + value;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += "OR " + colNames[i] + operations[0] + colValues[i];
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += " AND " + colNames[i] + operations[i] + colValues[i];
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 创建数据表
        /// </summary> +
        /// <returns>The table.</returns>
        /// <param name="tableName">数据表名</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colTypes">字段名类型</param>
        public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += "  ) ";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Reads the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <param name="tableName">Table name.</param>
        /// <param name="items">Items.</param>
        /// <param name="colNames">Col names.</param>
        /// <param name="operations">Operations.</param>
        /// <param name="colValues">Col values.</param>
        public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
        {
            string queryString = "SELECT " + items[0];
            for (int i = 1; i < items.Length; i++)
            {
                queryString += ", " + items[i];
            }
            queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
            for (int i = 0; i < colNames.Length; i++)
            {
                queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
            }
            return ExecuteQuery(queryString);
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            //销毁Command
            if (dbCommand != null)
            {
                dbCommand.Cancel();
            }
            dbCommand = null;

            //销毁Reader
            if (dataReader != null)
            {
                dataReader.Close();
            }
            dataReader = null;

            //销毁Connection
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
            dbConnection = null;
        }
    }
}