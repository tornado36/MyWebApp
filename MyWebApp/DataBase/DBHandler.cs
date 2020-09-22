using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Threading;


namespace MyWebApp.DataBase
{
    public class DBHandler
    {
        public static string ConnectToDB(string DBPath, out SQLiteConnection cn)
        {
            string sErr = "";

            cn = null;

            try
            {
                if (File.Exists(DBPath) == false)
                {
                    sErr = string.Format("File ({0}) does not exist.", DBPath);
                    goto Get_Out;
                }

                cn = new SQLiteConnection(string.Format("Data Source={0}", DBPath));
                cn.Open();
            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr != "")
            {
                sErr += "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }

        public static string CloseDBConnection(ref SQLiteConnection cn)
        {
            string sErr = "";

            try
            {
                if (cn != null)
                {
                    cn.Close();
                    cn.Dispose();
                    cn = null;
                }
                cn = null;
            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                sErr = sErr + "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }

        public static string GetDataBySQL(SQLiteConnection cn, string sSQL, out DataTable rs)
        {
            string sErr = "";
            SQLiteDataAdapter xAd = null;
            rs = null;

            try
            {
                xAd = new SQLiteDataAdapter(sSQL, cn);
                rs = new DataTable();
                xAd.Fill(rs);
            }
            catch (Exception ex)
            {
                sErr = string.Format("Error encountered.  sSQL: {0}", sSQL)
                    + "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (xAd != null)
            {
                xAd.Dispose();
                xAd = null;
            }

            if (sErr != "")
            {
                sErr += "\r\nSQL=" + sSQL
                    + "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }

        public static void ClearDataTable(ref DataTable dt)
        {
            string sErr = "";

            try
            {
                if (dt != null)
                {
                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr.Length > 0)
            {
                sErr = sErr + "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
                throw new Exception(sErr);
            }

            return;
        }

        public static string CreateDB(string DBPath)
        {
            string sErr = string.Empty;
            try
            {
                if (File.Exists(DBPath))
                {
                    File.SetAttributes(DBPath, FileAttributes.Archive);
                    File.Delete(DBPath);
                }
                SQLiteConnection.CreateFile(DBPath);

            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:
            if (sErr.Length > 0)
            {
                sErr = sErr + "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }
            return sErr;
        }

        public static string CopyDB(string originalPath, string destinationPath)
        {

            string sErr = string.Empty;
            try
            {
                if (File.Exists(destinationPath))
                {
                    File.SetAttributes(destinationPath, FileAttributes.Archive);
                    File.SetAttributes(originalPath, FileAttributes.Archive);
                    File.Delete(destinationPath);
                }
                File.Copy(originalPath, destinationPath, true);
            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:
            if (sErr.Length > 0)
            {
                sErr = sErr + "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }
            return sErr;
        }

        public static string ExecuteNonQuery(string sSQL, SQLiteConnection cn)
        {
            string sErr = "";

            try
            {
                if (string.IsNullOrEmpty(sSQL) == true)
                {
                    sErr = "sSQL is empty";
                    goto Get_Out;
                }

                if (!sSQL.Contains("COUNT(*)"))
                {
                    if (sSQL.ToLower().Contains("select") || sSQL.ToLower().Contains("update"))
                    {
                        sSQL = sSQL.Replace(";", "");
                        sSQL = sSQL + " COLLATE NOCASE;";
                    }
                    else
                    {

                    }
                }
                else
                {

                }

                SQLiteCommand cmd = new SQLiteCommand(sSQL, cn);
                cmd.CommandText = sSQL;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;

            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr != "")
            {
                sErr += "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }
        public static string ExecuteNonQueryWithoutCollation(string sSQL, SQLiteConnection cn)
        {
            string sErr = "";

            try
            {
                if (string.IsNullOrEmpty(sSQL) == true)
                {
                    sErr = "sSQL is empty";
                    goto Get_Out;
                }

                SQLiteCommand cmd = new SQLiteCommand(sSQL, cn);
                cmd.CommandText = sSQL;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;

            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr != "")
            {
                sErr += "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }

        public static string GetConfigInfo(SQLiteConnection cn, string sSQL, out object xObj)
        {
            string sErr = "";

            SQLiteCommand cmd = null;
            xObj = DBNull.Value;

            try
            {
                if (string.IsNullOrEmpty(sSQL) == true)
                {
                    sErr = "sSQL is empty";
                    goto Get_Out;
                }

                if (!sSQL.Contains("COUNT(*)"))
                {
                    if (sSQL.ToLower().Contains("select") || sSQL.ToLower().Contains("update"))
                    {
                        sSQL = sSQL.Replace(";", "");
                        sSQL = sSQL + " COLLATE NOCASE;";
                    }
                    else
                    {

                    }
                }
                else
                {

                }

                cmd = new SQLiteCommand(sSQL, cn);
                xObj = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                sErr = string.Format("Error encountered.  sSQL: {0}", sSQL)
                    + "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:
            if (cmd != null)
            {
                cmd.Dispose();
                cmd = null;
            }

            if (sErr != "")
            {
                sErr += "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }

        public static string ChangeAndConfirm(SQLiteConnection cn, string sSQLChange, string sSQLConfirm, int nCountTarget)
        {
            string sErr = "";
            int nCount = -1;
            int MaxTry = 50;

            try
            {

                if (sSQLChange.Length > 0)
                {
                    sErr = ExecuteNonQuery(sSQLChange, cn);
                    if (sErr.Length > 0) goto Get_Out;
                }

                if (sSQLConfirm.Length == 0) goto Get_Out;

                for (int i = 0; i < MaxTry; i++)
                {
                    sErr = CountRs(cn, sSQLConfirm, out nCount);
                    if (sErr.Length > 0) goto Get_Out;

                    if (nCount == nCountTarget) goto Get_Out;

                    Thread.Sleep(250);
                }
                sErr = string.Format("Failed to confirm after {0} trials.", MaxTry.ToString());
                goto Get_Out;
            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr != "")
            {
                sErr += "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString()
                    + "\r\nwhere sSQLCommit = (" + sSQLChange + ")."
                    + "\r\n      sSQLConfirm = (" + sSQLConfirm + ")."
                    + "\r\n      nCountTarget = " + nCountTarget.ToString() + "."
                    + "\r\n      nCount = " + nCount.ToString() + ".";
            }

            return sErr;
        }

        public static string CountRs(SQLiteConnection cn, string sSQL, out int nCount)
        {
            string sErr = "";
            object xObj;

            nCount = -1;

            try
            {
                sErr = GetConfigInfo(cn, sSQL, out xObj);
                if (sErr.Length > 0) goto Get_Out;

                nCount = Convert.ToInt32(xObj);
            }
            catch (Exception ex)
            {
                sErr = ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (sErr != "")
            {
                sErr += "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }

        public static string GetTableNames(SQLiteConnection cn, ref List<string> tableNames)
        {
            string sErr = "";
            string sSQL = "";
            tableNames = null;
            DataTable rs = null;
            try
            {
                rs = new DataTable();
                tableNames = new List<string>();

                sSQL = "select name from sqlite_master where type='table' order by name;";
                sErr = GetDataBySQL(cn, sSQL, out rs);

                foreach (DataRow row in rs.Rows)
                {
                    tableNames.Add(row["name"].ToString());
                }
            }
            catch (Exception ex)
            {
                sErr = string.Format("Error encountered.  sSQL: {0}", sSQL)
                    + "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:
            if (rs != null)
            {
                rs.Clear();
                rs = null;
            }

            if (sErr != "")
            {
                sErr += "\r\nSQL=" + sSQL
                    + "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }

        public static string IsTableNameExisted(SQLiteConnection cn, string tableName, ref bool isExisted)
        {
            string sErr = "";
            DataTable rs = null;
            string sSQL = "";
            string data = "";
            isExisted = false;
            try
            {
                rs = new DataTable();
                sSQL = "select name from sqlite_master where type='table' order by name;";
                sErr = GetDataBySQL(cn, sSQL, out rs);

                foreach (DataRow row in rs.Rows)
                {
                    data = row["name"].ToString();
                    if (string.Equals(data, tableName))
                    {
                        isExisted = true;
                        goto Get_Out;
                    }
                }
            }
            catch (Exception ex)
            {
                sErr = string.Format("Error encountered.  sSQL: {0}", sSQL)
                    + "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }

        Get_Out:

            if (rs != null)
            {
                rs.Clear();
                rs = null;
            }

            if (sErr != "")
            {
                sErr += "\r\nSQL=" + sSQL
                    + "\r\nRoutine=" + MethodInfo.GetCurrentMethod().ReflectedType.Name + "." + MethodInfo.GetCurrentMethod().ToString();
            }

            return sErr;
        }
    }
}
