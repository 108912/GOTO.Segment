using GOTO.BigDataAccess.DBSql.Access;
using GOTO.BigDataAccess.DBSql.XmlModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOTO.BigDataAccess.DBSql.Manager
{
    public class ManagerDatabase
    {
        public int DatabaseAttachAll(SqlBaseItemXml basemodel)
        {
            List<Task<int>> listtask = new List<Task<int>>();
            int revalue = 0;
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                Task<int> t = new Task<int>(n => AttachTask((SqlServerItemXml)n), item);
                t.Start();
                listtask.Add(t);
            }
            revalue = listtask.Sum(m => m.Result);
            Console.WriteLine("执行完成,附加数据库:" + revalue);
            return revalue;
        }
        public int AttachTask(object model)
        {
            var item = model as SqlServerItemXml;
            int revalue = 0;
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel,item.Number);
            foreach (var item2 in databaselist)
            {
                string connstr =DBProxyAction.GetConnStr(basemodel, item);
                DBProxy.GetDBAccess(item.DBType).DatabaseSPAttach(connstr, item2.DatabaseName, item2.StorageDirectory);
                revalue++;
                Console.WriteLine("附加服务器:" + item.ServerName + ",数据库:" + item2.DatabaseName);
            }
            return revalue;
        }
        public int DatabaseDetachAll(SqlBaseItemXml basemodel)
        {
            List<Task<int>> listtask = new List<Task<int>>();
            int revalue = 0;
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                Task<int> t = new Task<int>(n => DetachTask((SqlServerItemXml)n), item);
                t.Start();
                listtask.Add(t);
            }
            revalue = listtask.Sum(m => m.Result);
            Console.WriteLine("执行完成,分离数据库:" + revalue);
            return revalue;
        }
        public int DetachTask(object model)
        {
            var item = model as SqlServerItemXml;
            int revalue = 0;
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel, item.Number);
            foreach (var item2 in databaselist)
            {
                string connstr = DBProxyAction.GetConnStr(basemodel, item);
                DBProxy.GetDBAccess(item.DBType).DatabaseSPDetach(connstr, item2.DatabaseName);
                revalue++;
                Console.WriteLine("分离服务器:" + item.ServerName + ",数据库:" + item2.DatabaseName);
            }
            return revalue;
        }
        public void DatabaseUseCheckAll(SqlBaseItemXml basemodel)
        {
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            int servernum = serverlist.Count;
            int databasenum = 0;
            ManagerConfig serverconfig=new ManagerConfig();
            foreach (var item in serverlist)
            {
                var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel, item.Number);
                string connstr = DBProxyAction.GetConnStr(basemodel, item);
                foreach (var item2 in databaselist)
                {
                    var restatus = DBProxy.GetDBAccess(item.DBType).DatabaseExists(connstr, item2.DatabaseName);
                    if (item2.IsUse != restatus)
                    {
                        databasenum++;
                        serverconfig.DatabaseChangeIsUse(item2, restatus);
                    }
                    Console.WriteLine("检查数据库连接,服务器：" + item.ServerName+",数据库:"+item2.DatabaseName+",status:"+restatus);
                }
            }
            new ManagerConfig().SaveConfig(basemodel);
            Console.WriteLine("测试连接执行完成,操作服务器：" + servernum+",操作数据库:"+databasenum);
        }
    }
}