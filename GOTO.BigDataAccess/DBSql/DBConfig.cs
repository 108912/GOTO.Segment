using GOTO.BigDataAccess.DBSql.XmlModel;
using GOTO.BigDataAccess.RedisConfig;
using GOTO.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace GOTO.BigDataAccess.DBSql
{
    public static class DBConfig
    {
        public static string sqlbasexmlpath = CommonHelper.ToStr(ConfigurationManager.AppSettings["SqlBaseConfigPath"]);
        private static SqlBaseXml listbasexmlconfig = null;
        private static Dictionary<long, SqlServerXml> listserverxmlconfig = new Dictionary<long, SqlServerXml>();
        private static Dictionary<long, SqlDatabaseXml> listdatabasexmlconfig = new Dictionary<long, SqlDatabaseXml>();
        private static Dictionary<long, SqlTableXml> listtablexmlconfig = new Dictionary<long, SqlTableXml>();
        private static Dictionary<long, SqlFieldXml> listfieldxmlconfig = new Dictionary<long, SqlFieldXml>();
        #region getobject
        public static SqlBaseItemXml GetBaseXmlConfig(long number, string DatabasePrev=null)
        {
            SqlBaseItemXml model =null;
            if (listbasexmlconfig == null)
            {
                listbasexmlconfig = LoadSqlBaseXmlConfig(sqlbasexmlpath);
            }
            if (number > 0)
            {
                model = listbasexmlconfig.SqlBaseList.Where(m => m.Number == number).FirstOrDefault();
            }
            if (model==null&&!string.IsNullOrEmpty(DatabasePrev))
            {
                model = listbasexmlconfig.SqlBaseList.Where(m => m.DatabasePrev == DatabasePrev).FirstOrDefault();
            }
            return model;
        }
        public static SqlServerXml GetServerXmlConfig(SqlBaseItemXml basemodel)
        {
            if (!listserverxmlconfig.ContainsKey(basemodel.Number))
            {
                listserverxmlconfig.Add(basemodel.Number, LoadSqlServerXmlConfig(basemodel.ServerFilePath));
            }
            return listserverxmlconfig[basemodel.Number];
        }
        public static SqlDatabaseXml GetDatabaseXmlConfig(SqlBaseItemXml basemodel)
        {
            if (!listdatabasexmlconfig.ContainsKey(basemodel.Number))
            {
                listdatabasexmlconfig.Add(basemodel.Number, LoadSqlDatabaseXmlConfig(basemodel.DatabaseFilePath));
            }
            return listdatabasexmlconfig[basemodel.Number];
        }
        public static SqlTableXml GetTableXmlConfig(SqlBaseItemXml basemodel)
        {
            if (!listtablexmlconfig.ContainsKey(basemodel.Number))
            {
                listtablexmlconfig.Add(basemodel.Number, LoadSqlTableXmlConfig(basemodel.TableFilePath));
            }
            return listtablexmlconfig[basemodel.Number];
        }
        public static SqlFieldXml GetFieldXmlConfig(SqlBaseItemXml basemodel)
        {
            if (!listfieldxmlconfig.ContainsKey(basemodel.Number))
            {
                listfieldxmlconfig.Add(basemodel.Number, LoadSqlFieldXmlConfig(basemodel.FieldFilePath));
            }
            return listfieldxmlconfig[basemodel.Number];
        }

        public static List<SqlServerItemXml> GetServerItemXmlConfigList(SqlBaseItemXml basemodel, bool IsUse = true)
        {
            return DBConfig.GetServerXmlConfig(basemodel).SqlServerList.Where(m => m.IsUse == IsUse).ToList();
        }
        public static List<SqlDatabaseItemXml> GetDatabaseItemXmlConfigList(SqlBaseItemXml basemodel, long ServerNumber, bool IsUse = true)
        {
            return DBConfig.GetDatabaseXmlConfig(basemodel).SqlDatabaseList.Where(m => m.IsUse == IsUse && m.ServerNumber == ServerNumber).ToList();
        }
        public static List<SqlTableItemXml> GetTableItemXmlConfigList(SqlBaseItemXml basemodel,long DatabaseNumber, bool IsUse = true)
        {
            return DBConfig.GetTableXmlConfig(basemodel).SqlTableList.Where(m => m.IsUse == IsUse&&m.DataBaseNumber==DatabaseNumber).ToList();
        }
        public static List<SqlFieldItemXml> GetFieldItemXmlConfigList(SqlBaseItemXml basemodel,long TableNumber, bool IsDelete = false)
        {
            return DBConfig.GetFieldXmlConfig(basemodel).SqlFieldList.Where(m => m.IsDelete == IsDelete && m.TableNumber == TableNumber).ToList();
        }

        public static SqlServerItemXml GetServerItemXmlConfig(SqlBaseItemXml basemodel,long Number, bool IsUse = true)
        {
            return DBConfig.GetServerXmlConfig(basemodel).SqlServerList.Where(m => m.IsUse == IsUse && m.Number == Number).FirstOrDefault();
        }
        public static SqlDatabaseItemXml GetDatabaseItemXmlConfig(SqlBaseItemXml basemodel, long Number, bool IsUse = true)
        {
            return DBConfig.GetDatabaseXmlConfig(basemodel).SqlDatabaseList.Where(m => m.IsUse == IsUse && m.Number == Number).FirstOrDefault();
        }
        public static SqlTableItemXml GetTableItemXmlConfig(SqlBaseItemXml basemodel, long Number, bool IsUse = true)
        {
            return DBConfig.GetTableXmlConfig(basemodel).SqlTableList.Where(m => m.IsUse == IsUse && m.Number == Number).FirstOrDefault();
        }
        public static SqlFieldItemXml GetFieldItemXmlConfig(SqlBaseItemXml basemodel, long Number, bool IsDelete = false)
        {
            return DBConfig.GetFieldXmlConfig(basemodel).SqlFieldList.Where(m => m.IsDelete == IsDelete && m.Number == Number).FirstOrDefault();
        }
        #endregion



        #region Reloadobject
        public static SqlServerXml ReloadServerXmlConfig(SqlBaseItemXml basemodel)
        {
            listserverxmlconfig[basemodel.Number] = LoadSqlServerXmlConfig(basemodel.ServerFilePath);
            return listserverxmlconfig[basemodel.Number];
        }
        public static SqlDatabaseXml ReloadDatabaseXmlConfig(SqlBaseItemXml basemodel)
        {
            listdatabasexmlconfig[basemodel.Number] = LoadSqlDatabaseXmlConfig(basemodel.DatabaseFilePath);
            return listdatabasexmlconfig[basemodel.Number];
        }
        public static SqlTableXml ReloadTableXmlConfig(SqlBaseItemXml basemodel)
        {
            listtablexmlconfig[basemodel.Number] = LoadSqlTableXmlConfig(basemodel.TableFilePath);
            return listtablexmlconfig[basemodel.Number];
        }
        public static SqlFieldXml ReloadFieldXmlConfig(SqlBaseItemXml basemodel)
        {
            listfieldxmlconfig[basemodel.Number] = LoadSqlFieldXmlConfig(basemodel.FieldFilePath);
            return listfieldxmlconfig[basemodel.Number];
        }
        #endregion
        #region loadobject
        public static SqlBaseXml LoadSqlBaseXmlConfig(string filepath)
        {
            var servercontent = FileHelper.FileToString(filepath, Encoding.UTF8);
            return XMLSerilizable.XMLToObject<SqlBaseXml>(servercontent, Encoding.UTF8);
        }
        public static SqlServerXml LoadSqlServerXmlConfig(string filepath)
        {
            var servercontent = FileHelper.FileToString(filepath, Encoding.UTF8);
            return XMLSerilizable.XMLToObject<SqlServerXml>(servercontent, Encoding.UTF8);
        }
        public static SqlDatabaseXml LoadSqlDatabaseXmlConfig(string filepath)
        {
            var databasecontent = FileHelper.FileToString(filepath, Encoding.UTF8);
            return XMLSerilizable.XMLToObject<SqlDatabaseXml>(databasecontent, Encoding.UTF8);
        }
        public static SqlTableXml LoadSqlTableXmlConfig(string filepath)
        {
            var tablecontent = FileHelper.FileToString(filepath, Encoding.UTF8);
            return XMLSerilizable.XMLToObject<SqlTableXml>(tablecontent, Encoding.UTF8);
        }
        public static SqlFieldXml LoadSqlFieldXmlConfig(string filepath)
        {
            var fieldcontent = FileHelper.FileToString(filepath, Encoding.UTF8);
            return XMLSerilizable.XMLToObject<SqlFieldXml>(fieldcontent, Encoding.UTF8);
        }
        #endregion
        #region getnumber
        public static long GetRowId(long basenumber)
        {
            return CommonHelper.ToLong(RedisConfigService.Get(RedisConfigKey.keysqlserverdatarowid+basenumber));
        }
        public static bool UpdateRowId(long basenumber, int updaterownum)
        {
            long rowidrrr = GetRowId(basenumber);
            return RedisConfigService.Set(RedisConfigKey.keysqlserverdatarowid + basenumber, (rowidrrr + updaterownum).ToString());
        }

        public static long GetBaseNumber
        {
            get { return DateTime.Now.Ticks; }
        }
        public static long GetServerNumber
        {
            get { return DateTime.Now.Ticks; }
        }
        public static long GetDatabaseNumber
        {
            get { return DateTime.Now.Ticks; }
        }
        public static long GetTableNumber
        {
            get { return DateTime.Now.Ticks; }
        }
        public static long GetFieldNumber
        {
            get { return DateTime.Now.Ticks; }
        }
        #endregion
        public static string GetTableSql(SqlBaseItemXml basemodel,string tablename)
        {
            string restr = "";
            //StringBuilder sqlstr = new StringBuilder();
            //sqlstr.Append("CREATE TABLE {0}(");
            //sqlstr.Append("	[id] [bigint] primary key ,");
            //sqlstr.Append("	[num1] [int] NULL,");
            //sqlstr.Append("	[num2] [bigint] NULL,");
            //sqlstr.Append("	[num3] [float] NULL,");
            //sqlstr.Append("	[str11] [nvarchar](50) NULL,");
            //sqlstr.Append("	[createdate] [datetime] default getdate(),");
            //sqlstr.Append("	[isdelete] [bit] default 0);");
            //FileHelper.WriteText(SqlServerConfig.SqlBaseXmlConfig.TableSqlFilePath, sqlstr.ToString());
            string strcontent = FileHelper.FileToString(basemodel.TableSqlFilePath, Encoding.UTF8);
            restr = string.Format(strcontent, tablename);
            return restr;
        }
        public static DataTable GetTableStruct(SqlBaseItemXml basemodel, string tablename)
        {
            DataTable bulkdata = new DataTable();
            //bulkdata.TableName = tablename;
            //#region
            //bulkdata.Columns.Add("id", typeof(long));
            //bulkdata.Columns.Add("num1", typeof(int));
            //bulkdata.Columns.Add("num2", typeof(long));
            //bulkdata.Columns.Add("num3", typeof(double));
            //bulkdata.Columns.Add("str11", typeof(string));
            //#endregion
            //var t1 = XMLSerilizable.ObjectToXML(bulkdata);
            //FileHelper.WriteText(SqlServerConfig.SqlBaseXmlConfig.TableStructFilePath, t1);

            var fieldcontent = FileHelper.FileToString(basemodel.TableStructFilePath, Encoding.UTF8);
            bulkdata = XMLSerilizable.XMLToObject<DataTable>(fieldcontent, Encoding.UTF8);
            bulkdata.TableName = tablename;
            return bulkdata;
        }
        #region changelist
        public static void ServerRemove(SqlBaseItemXml basemodel, SqlServerItemXml item)
        {
            var templist = DBConfig.listserverxmlconfig[basemodel.Number];
            templist.SqlServerList.Remove(item);
            DBConfig.listserverxmlconfig[basemodel.Number] = templist;
        }
        public static void ServerAdd(SqlBaseItemXml basemodel, SqlServerItemXml item)
        {
            var templist = DBConfig.listserverxmlconfig[basemodel.Number];
            templist.SqlServerList.Add(item);
            DBConfig.listserverxmlconfig[basemodel.Number] = templist;
        }
        public static void DatabaseRemove(SqlBaseItemXml basemodel, SqlDatabaseItemXml item)
        {
            var templist = DBConfig.listdatabasexmlconfig[basemodel.Number];
            templist.SqlDatabaseList.Remove(item);
            DBConfig.listdatabasexmlconfig[basemodel.Number] = templist;
        }
        public static void DatabaseAdd(SqlBaseItemXml basemodel, SqlDatabaseItemXml item)
        {
            var templist = DBConfig.listdatabasexmlconfig[basemodel.Number];
            templist.SqlDatabaseList.Add(item);
            DBConfig.listdatabasexmlconfig[basemodel.Number] = templist;
        }
        public static void TableRemove(SqlBaseItemXml basemodel, SqlTableItemXml item)
        {
            var templist = DBConfig.listtablexmlconfig[basemodel.Number];
            templist.SqlTableList.Remove(item);
            DBConfig.listtablexmlconfig[basemodel.Number] = templist;
        }
        public static void TableAdd(SqlBaseItemXml basemodel, SqlTableItemXml item)
        {
            var templist = DBConfig.listtablexmlconfig[basemodel.Number];
            templist.SqlTableList.Add(item);
            DBConfig.listtablexmlconfig[basemodel.Number] = templist;
        }

        public static void FieldRemove(SqlBaseItemXml basemodel, SqlFieldItemXml item)
        {
            var templist = DBConfig.listfieldxmlconfig[basemodel.Number];
            templist.SqlFieldList.Remove(item);
            DBConfig.listfieldxmlconfig[basemodel.Number] = templist;
        }
        public static void FieldAdd(SqlBaseItemXml basemodel, SqlFieldItemXml item)
        {
            var templist = DBConfig.listfieldxmlconfig[basemodel.Number];
            templist.SqlFieldList.Add(item);
            DBConfig.listfieldxmlconfig[basemodel.Number] = templist;
        }
        #endregion
    }
}