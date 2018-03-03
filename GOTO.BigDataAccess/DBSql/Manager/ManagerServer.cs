using GOTO.BigDataAccess.DBSql.Access;
using GOTO.BigDataAccess.DBSql.XmlModel;
using GOTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOTO.BigDataAccess.DBSql.Manager
{
    public class ManagerServer
    {
        public void ServerCacheClearAll(SqlBaseItemXml basemodel)
        {
            List<Task<int>> listtask = new List<Task<int>>();
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                Task<int> t = new Task<int>(n => ServerCacheClearItem((SqlServerItemXml)n), item);
                t.Start();
                listtask.Add(t);
            }
            int revalue = listtask.Sum(m => m.Result);
            Console.WriteLine("测试连接执行完成,操作服务器数:" + revalue);
        }

        public int ServerCacheClearItem(object model)
        {
            var item = model as SqlServerItemXml;
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            string connstr = DBProxyAction.GetConnStr(basemodel, item);
            Console.WriteLine("操作服务器开始:" + item.ServerName);
            DBProxy.GetDBAccess(item.DBType).ServerCacheClear(connstr);
            Console.WriteLine("操作服务器完成:" + item.ServerName);
            return 1;
        }
        public void ServerConnectionCheckAll(SqlBaseItemXml basemodel)
        {
            List<Task<int>> listtask = new List<Task<int>>();
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                Task<int> t = new Task<int>(n => ServerConnectionCheckItem((SqlServerItemXml)n), item);
                t.Start();
                listtask.Add(t);
            }
            int revalue = listtask.Sum(m => m.Result);
            new ManagerConfig().SaveConfig(basemodel);
            Console.WriteLine("测试连接执行完成,操作服务器数：" + revalue);
        }
        public int ServerConnectionCheckItem(object model)
        {
            var item = model as SqlServerItemXml;
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            bool connectionstatus = SocketHelper.TestConnection(item.ServerName, item.ServerPort, basemodel.SocketMillisecondsTimeout);
            if (connectionstatus)
            {
                string connstr = DBProxyAction.GetConnStr(basemodel, item);
                connectionstatus = DBProxy.GetDBHelper(item.DBType).ConnectionIsUse(connstr);
            }
            new ManagerConfig().ServerChangeIsUse(item, connectionstatus);
            Console.WriteLine("服务器:" + item.ServerName + ",isuse:" + connectionstatus);
            return 1;
        }
    }
}