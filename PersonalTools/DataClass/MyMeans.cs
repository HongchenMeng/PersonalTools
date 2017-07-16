using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.IO;

namespace PersonalTools.DataClass
{
    class MyMeans
    {
        #region  全局变量
        public static string DataSource = @"Data Source =Data/db_PT.db";// "Data Source=ADMIN-PC;Database=db_PWMS;User id=sa;PWD=sa123456";//sql服务器名称默与本机计算机名称相同ADMIN-PC，或者使用英文.也行，127.0.0.1
        public static string PassWord = "PersonalTools@Admin!gu520lu";
        #endregion


        #region 返回第一行第一列
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="SQLString">返回第一行第一列</param>
        /// <returns>返回字符串</returns>
        public string ReturnFirstRowCols(string SQLString)
        {
            //string SQLString = "Select Count(*) from " + TableName;
            using (SQLiteConnection connection = new SQLiteConnection(DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.SetPassword(PassWord);
                        connection.Open();
                        string obj = cmd.ExecuteScalar().ToString();
                        return obj;
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        #endregion
        #region 执行SQL语句，返回影响的记录数
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int getsqlcom(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.SetPassword(PassWord);
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }
        #endregion
        #region  创建DataSet对象 +3重载
        /// <summary>
        /// 创建一个DataSet对象
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public DataSet getDataSet(string SQLString, string TableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataSource))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.SetPassword(PassWord);
                    connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connection);
                    command.Fill(ds, TableName);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        public DataSet getDataSet(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataSource))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet,设置命令的执行等待时间
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public DataSet getDataSet(string SQLString, int Times)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DataSource))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        #endregion
        #region  执行查询语句，返回SQLiteDataReader(使用该方法切记要手工关闭SQLiteDataReader和连接)
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader(使用该方法切记要手工关闭SQLiteDataReader和连接)
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public SQLiteDataReader ExecuteReader(string strSQL)
        {
            SQLiteConnection connection = new SQLiteConnection(DataSource);
            SQLiteCommand cmd = new SQLiteCommand(strSQL, connection);
            try
            {
                connection.SetPassword(PassWord);
                connection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }
            //finally //不能在此关闭，否则，返回的对象将无法使用
            //{
            //    cmd.Dispose();
            //    connection.Close();
            //}    
        }
        #endregion
    }
}
