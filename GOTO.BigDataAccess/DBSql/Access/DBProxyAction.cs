using GOTO.BigDataAccess.DBSql.Model;
using GOTO.BigDataAccess.DBSql.XmlModel;
using GOTO.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOTO.BigDataAccess.DBSql.Access
{
    public static class DBProxyAction
    {
        public static string GetConnStr(SqlBaseItemXml basemodel, SqlServerItemXml servermodel)
        {
            return string.Format(basemodel.TemplateSqlConnServer, servermodel.ServerName, servermodel.ServerUserName, servermodel.ServerUserPwd);
        }
        public static string GetConnStr(SqlBaseItemXml basemodel, SqlServerItemXml item, SqlDatabaseItemXml item2)
        {
            return string.Format(basemodel.TemplateSqlConnDatabase, item.ServerName, item2.DatabaseName, item.ServerUserName, item.ServerUserPwd);
        }
        public static string GetSqlWhere(SqlServerItemXml servermodel, ConditionFieldModel conditionfield)
        {
            StringBuilder sqlwhere = new StringBuilder();
            if (conditionfield != null)
            {
                int i = 0;
                foreach (var item in conditionfield.List)
                {
                    if (i++ > 0)
                    {
                        sqlwhere.Append(" and ");
                    }
                    sqlwhere.AppendFormat(" {0} between {1} and  {2} ", item.FieldName, item.ValueMin, item.ValueMax);
                }
            }
            return sqlwhere.ToString();
        }
        public static string GetSqlList(SqlServerItemXml servermodel, MatchServerList item, ConditionFieldModel conditionfield, long getmaxnum = 500)
        {
            return string.Format(@"select top {3} * from {0}.dbo.{1} where {2} ", item.DatabaseNumber, item.TableNumber, GetSqlWhere(servermodel,conditionfield), getmaxnum);
        }
        public static string GetSqlUpdate(SqlBaseItemXml basemodel, Dictionary<string, object> updatefieldlist)
        {
            StringBuilder sqlstr = new StringBuilder();
            if (updatefieldlist != null)
            {
                foreach (var tempitem in updatefieldlist)
                {
                    if (basemodel.FieldUpdateList.Contains(tempitem.Key))
                    {
                        if (sqlstr.Length > 0)
                        {
                            sqlstr.Append(',');
                        }
                        if (tempitem.Value is int || tempitem.Value is long || tempitem.Value is float || tempitem.Value is double || tempitem.Value is decimal)
                        {
                            sqlstr.AppendFormat("{0}={1} ", tempitem.Key, tempitem.Value.ToString());
                        }
                        else
                        {
                            sqlstr.AppendFormat("{0}='{1}' ", tempitem.Key, tempitem.Value.ToString());
                        }
                    }
                }
            }
            return sqlstr.ToString();
        }
        public static string GetSqlCount(SqlServerItemXml servermodel, MatchServerList item, ConditionFieldModel conditionfield)
        {
            return string.Format(@"select COUNT(0) from {0}.dbo.{1} where {2}", item.DatabaseNumber, item.TableNumber, GetSqlWhere(servermodel, conditionfield));
        }
        public static bool Delete(SqlBaseItemXml basemodel, long rowid, SqlServerItemXml servermodel, SqlDatabaseItemXml databasemodel, SqlTableItemXml tablemodel, SqlFieldItemXml fieldmodel)
        {
            bool revalue = false;
            string connstr = GetConnStr(basemodel,servermodel);
            string sqlstr = string.Format("delete from {0}.dbo.{1} where {2}={3}", databasemodel.DatabaseName, tablemodel.TableName, basemodel.PrimarykeyName, rowid);

            long temp = CommonHelper.ToLong(DBProxy.GetDBHelper(servermodel.DBType).ExecuteSql(connstr, sqlstr));
            if (temp > 0)
            {
                revalue = true;
            }
            return revalue;
        }
        public static bool Update(SqlBaseItemXml basemodel, long rowid, SqlServerItemXml servermodel, SqlDatabaseItemXml databasemodel, SqlTableItemXml tablemodel, Dictionary<string, object> updatefieldlist)
        {
            bool revalue = false;
            string connstr = GetConnStr(basemodel, servermodel);
            string sqlstr = string.Format("update {0}.dbo.{1} set {4} where {2}={3}", databasemodel.DatabaseName, tablemodel.TableName, basemodel.PrimarykeyName, rowid, GetSqlUpdate(basemodel,updatefieldlist));

            long temp = CommonHelper.ToLong(DBProxy.GetDBHelper(servermodel.DBType).ExecuteSql(connstr, sqlstr));
            if (temp > 0)
            {
                revalue = true;
            }
            return revalue;
        }
        public static long GetAllCount(SqlBaseItemXml basemodel, ref long sqlnum)
        {
            sqlnum = 0;
            long rownum = 0;
            List<TaskDataParam> taskdataparam = new List<TaskDataParam>();
            var databaselist = DBConfig.GetDatabaseXmlConfig(basemodel).SqlDatabaseList.Where(m => m.IsUse == true).ToList();
            foreach (var item in databaselist)
            {
                var servermodel = DBConfig.GetServerItemXmlConfig(basemodel,item.ServerNumber);
                if (servermodel != null)
                {
                    TaskDataParam tempparam = new TaskDataParam();
                    tempparam.servername = servermodel.ServerName;
                    tempparam.dbtype = servermodel.DBType;
                    tempparam.connstr = GetConnStr(basemodel, servermodel,item);
                    tempparam.sqlstr = "select  sum(b.rows) as 记录条数 from sysobjects a,sysindexes b where a.id=b.id and a.xtype='u' and b.indid=1 ";
                    taskdataparam.Add(tempparam);
                }
            }
            DBTask servicetask = new DBTask();
            rownum = servicetask.SyncThreadPoolManagerSum(taskdataparam, 100, false);
            sqlnum = servicetask.runnumcurrent;
            return rownum;
        }
        public static long GetAllCount2(SqlBaseItemXml basemodel, ref long sqlnum)
        {
            sqlnum = 0;
            long rownum = 0;
            List<TaskDataParam> taskdataparam = new List<TaskDataParam>();
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                StringBuilder sqlstr = new System.Text.StringBuilder();
                var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel, item.Number);
                foreach (var item2 in databaselist)
                {
                    if (sqlstr.Length > 0)
                    {
                        sqlstr.Append(" union all ");
                    }
                    sqlstr.AppendFormat(" select  sum(b.rows) as 记录条数 from {0}.sys.sysobjects a,{0}.sys.sysindexes b where a.id=b.id and a.xtype='u' and b.indid=1 ", item2.DatabaseName);
                }
                if (sqlstr.Length > 0)
                {
                    TaskDataParam tempparam = new TaskDataParam();
                    tempparam.servername = item.ServerName;
                    tempparam.dbtype = item.DBType;
                    tempparam.connstr = GetConnStr(basemodel, item);
                    tempparam.sqlstr = string.Format("select SUM([记录条数]) from ({0})t1", sqlstr.ToString());
                    taskdataparam.Add(tempparam);
                }
                sqlstr.Clear();
            }
            DBTask servicetask = new DBTask();
            rownum = servicetask.SyncTaskManagerSum(taskdataparam, 100, false);
            sqlnum = servicetask.runnumcurrent;
            return rownum;
        }
    }
}
