using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWebAppCore.Database
{
    class SqliteHandler
    {
        private static ILogger _logger;

        public static string Initialize(ILoggerFactory factory)
        {
            string sErr = "";
            try
            {
                _logger = factory.CreateLogger("SqliteHelper");
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:
            return sErr;
        }

        public static async Task<Tuple<int, string, DataTable>> GetDataTable(string connectionStr, string sqlStr, params SQLiteParameter[] ps)
        {
            string sErr = "";
            int affectedRows = -1;
            DataTable dataTable = null;
            try
            {
                if (string.IsNullOrEmpty(connectionStr)) throw new ArgumentNullException(nameof(connectionStr));
                if (string.IsNullOrEmpty(sqlStr)) throw new ArgumentNullException(nameof(sqlStr));

                using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionStr))
                {
                    if (sqliteConnection.State != ConnectionState.Open)
                        await sqliteConnection.OpenAsync();
                    using (SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sqlStr, sqliteConnection))
                    {
                        sQLiteDataAdapter.SelectCommand.Parameters.AddRange(ps);
                        dataTable = new DataTable();
                        affectedRows = sQLiteDataAdapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:
            if (sErr.Length > 0)
            {
                if (_logger != null)
                    _logger.LogError(sErr);
            }

            return new Tuple<int, string, DataTable>(affectedRows, sErr, dataTable);
        }
    }
}
