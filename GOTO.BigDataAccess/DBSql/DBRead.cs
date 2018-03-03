using GOTO.BigDataAccess.DBSql.Access;
using GOTO.BigDataAccess.DBSql.Model;
using GOTO.BigDataAccess.DBSql.XmlModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GOTO.BigDataAccess.DBSql
{
    public class DBRead
    {
        public long GetAllCount(SqlBaseItemXml basemodel,ref long sqlnum)
        {
            return DBProxyAction.GetAllCount(basemodel,ref sqlnum);
        }
        public long GetAllCount2(SqlBaseItemXml basemodel,ref long sqlnum)
        {
            return DBProxyAction.GetAllCount2(basemodel, ref sqlnum);
        }
        public long GetCount(SqlBaseItemXml basemodel,ConditionFieldModel condition, ref long sqlnum)
        {
            sqlnum = 0;
            long rownum = 0;
            DataTable dt = new DataTable();
            DBRule serverrule=new DBRule();
            List<MatchServerList> serverlist = serverrule.GetMatchServer(basemodel,condition);
            if (serverlist != null && serverlist.Count > 0)
            {
                List<TaskDataParam> taskdataparam = new List<TaskDataParam>();
                foreach (var item in serverlist)
                {
                    var servermodel = DBConfig.GetServerItemXmlConfig(basemodel,item.ServerNumber);
                    if (servermodel != null)
                    {
                        TaskDataParam tempparam = new TaskDataParam();
                        tempparam.servername = servermodel.ServerName;
                        tempparam.dbtype = servermodel.DBType;
                        tempparam.connstr = DBProxyAction.GetConnStr(basemodel, servermodel);
                        tempparam.sqlstr = DBProxyAction.GetSqlCount(servermodel, item, condition);
                        taskdataparam.Add(tempparam);
                    }
                }
                DBTask servicetask = new DBTask();
                Console.WriteLine("满足条件的数据库表:"+taskdataparam.Count);
                rownum = servicetask.SyncTaskManagerSum(taskdataparam, 50,false);
                sqlnum = servicetask.runnumcurrent;
            }
            return rownum;
        }
        public DataTable GetList(SqlBaseItemXml basemodel,ConditionFieldModel condition, ref long sqlnum)
        {
            sqlnum = 0;
            long getmaxnum = 500;
            long getrowcount = 0;
            DataTable dt = new DataTable();
            DBRule serverrule = new DBRule();
            List<MatchServerList> serverlist = serverrule.GetMatchServer(basemodel, condition);

            if (serverlist != null && serverlist.Count > 0)
            {
                List<TaskDataParam> taskdataparam = new List<TaskDataParam>();
                foreach (var item in serverlist)
                {
                    var servermodel = DBConfig.GetServerItemXmlConfig(basemodel, item.ServerNumber);
                    if (servermodel != null)
                    {
                        TaskDataParam tempparam = new TaskDataParam();
                        tempparam.servername = servermodel.ServerName;
                        tempparam.dbtype = servermodel.DBType;
                        tempparam.connstr = DBProxyAction.GetConnStr(basemodel,servermodel);
                        tempparam.sqlstr = DBProxyAction.GetSqlList(servermodel, item, condition, getmaxnum);
                        taskdataparam.Add(tempparam);
                    }
                }
                Console.WriteLine("满足条件的数据库表:" + taskdataparam.Count);
                foreach (var itemparam in taskdataparam)
                {
                    Console.WriteLine("访问服务器:"+itemparam.servername);
                    var dttemp =DBProxy.GetDBHelper(itemparam.dbtype).GetDataDable(itemparam.connstr, itemparam.sqlstr);
                    sqlnum++;
                    if (dttemp != null && dttemp.Rows.Count>0)
                    {
                        var dttempcount = dttemp.Rows.Count;
                        if (getrowcount>0)
                        {
                            foreach (DataRow dtrow in dttemp.Rows)
                            {
                                if (getrowcount >= getmaxnum)
                                {
                                    return dt;
                                }
                                getrowcount++;
                                DataRow r = dt.NewRow();
                                r.ItemArray = dtrow.ItemArray;
                                dt.Rows.Add(r);

                            }
                        }
                        else
                        {
                            getrowcount = dttemp.Rows.Count;
                            dt = dttemp;
                        }
                    }
                    if (getrowcount >= getmaxnum)
                    {
                        return dt;
                    }
                }
            }
            return dt;
        }
    }
}
