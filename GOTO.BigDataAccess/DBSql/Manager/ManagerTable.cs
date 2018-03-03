using GOTO.BigDataAccess.DBSql.Access;
using GOTO.BigDataAccess.DBSql.XmlModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOTO.BigDataAccess.DBSql.Manager
{
    public class ManagerTable
    {
        public void TableAddIndexAll(SqlBaseItemXml basemodel)
        {
            List<Task<int>> listtask = new List<Task<int>>();
            int revalue = 0;
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                Task<int> t = new Task<int>(n => TableAddIndexServer((SqlServerItemXml)n), item);
                t.Start();
                listtask.Add(t);
            }
            revalue = listtask.Sum(m => m.Result);
            Console.WriteLine("添加索引执行完成,影响服务器数:" + revalue);
        }
        public int TableAddIndexServer(object model)
        {
            int changetablenum = 0;
            int changetableindexnum = 0;
            var item = model as SqlServerItemXml;
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            if (item != null)
            {
                var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel,item.Number);
                foreach (var item2 in databaselist)
                {
                    var tablelist = DBConfig.GetTableItemXmlConfigList(basemodel, item2.Number);
                    string connstr = DBProxyAction.GetConnStr(basemodel, item);
                    foreach (var item3 in tablelist)
                    {
                        int validnum = 0;
                        foreach (var itemfield in basemodel.FieldIndexList)
                        {
                            string indexname = basemodel.IndexPrev + item3.TableName + itemfield;
                            if (!DBProxy.GetDBAccess(item.DBType).TableIndexExists(connstr, item2.DatabaseName, item3.TableName, indexname))
                            {
                                validnum++;
                                changetableindexnum++;
                                DBProxy.GetDBAccess(item.DBType).TableIndexAdd(connstr, item2.DatabaseName, item3.TableName, itemfield, indexname);
                                Console.WriteLine("添加索引服务器:" + item.ServerName + ",数据库:" + item2.DatabaseName + ",表：" + item3.TableName);
                            }
                        }
                        if (validnum > 0)
                        {
                            changetablenum++;
                        }
                    }
                }
            }
            Console.WriteLine("服务器:" + item.ServerName + ",涉及表总数:" + changetablenum + ",添加索引总数:" + changetableindexnum);
            return 1;
        }
        public void TableDelIndexAll(SqlBaseItemXml basemodel)
        {
            List<Task<int>> listtask = new List<Task<int>>();
            int revalue = 0;
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                Task<int> t = new Task<int>(n => TableDelIndexServer((SqlServerItemXml)n), item);
                t.Start();
                listtask.Add(t);
            }
            revalue = listtask.Sum(m => m.Result);
            Console.WriteLine("删除索引执行完成,影响服务器数:" + revalue);
        }
        public int TableDelIndexServer(object model)
        {
            int changetablenum = 0;
            int changetableindexnum = 0;
            var item = model as SqlServerItemXml;
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            if (item != null)
            {
                var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel, item.Number);
                foreach (var item2 in databaselist)
                {
                    var tablelist = DBConfig.GetTableItemXmlConfigList(basemodel, item2.Number);
                    DBProxyAction.GetConnStr(basemodel, item,item2);
                    string conndatabasestr = DBProxyAction.GetConnStr(basemodel, item, item2);
                    foreach (var item3 in tablelist)
                    {
                        int validnum = 0;
                        foreach (var itemfield in basemodel.FieldIndexList)
                        {
                            string indexname = basemodel.IndexPrev + item3.TableName + itemfield;
                            if (!DBProxy.GetDBAccess(item.DBType).TableIndexExists(conndatabasestr, item2.DatabaseName, item3.TableName, indexname))
                            {
                                validnum++;
                                changetableindexnum++;

                                DBProxy.GetDBAccess(item.DBType).TableIndexDel(conndatabasestr, item3.TableName, indexname);
                                Console.WriteLine("删除索引服务器:" + item.ServerName + ",数据库:" + item2.DatabaseName + ",表：" + item3.TableName);
                            }
                        }
                        if (validnum > 0)
                        {
                            changetablenum++;
                        }
                    }
                }
            }
            Console.WriteLine("服务器:" + item.ServerName + ",涉及表总数:" + changetablenum + ",添加索引总数:" + changetableindexnum);
            return 1;
        }
        public void TableUseCheckAll(SqlBaseItemXml basemodel)
        {
            List<Task<int>> listtask = new List<Task<int>>();
            int revalue = 0;
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            foreach (var item in serverlist)
            {
                Task<int> t = new Task<int>(n => TableUseCheckServer((SqlServerItemXml)n), item);
                t.Start();
                listtask.Add(t);
            }
            revalue = listtask.Sum(m => m.Result);
            new ManagerConfig().SaveConfig(basemodel);
            Console.WriteLine("检查表,涉及服务器数:" + revalue);
        }
        public int TableUseCheckServer(object model)
        {
            int changetablenum = 0;
            var item = model as SqlServerItemXml;
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            if (item != null)
            {
                ManagerConfig serverconfig = new ManagerConfig();
                var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel, item.Number);
                string connstr = DBProxyAction.GetConnStr(basemodel, item);
                foreach (var item2 in databaselist)
                {
                    var tablelist = DBConfig.GetTableItemXmlConfigList(basemodel, item2.Number);
                    foreach (var item3 in tablelist)
                    {
                        var restatus = DBProxy.GetDBAccess(item.DBType).TableExists(connstr, item2.DatabaseName, item3.TableName);
                        if (item3.IsUse != restatus)
                        {
                            changetablenum++;
                            serverconfig.TableChangeIsUse(item3, restatus);
                        }
                        Console.WriteLine("检查数据表连接,服务器：" + item.ServerName + ",数据库:" + item2.DatabaseName + ",表:"+item3.TableName+",status:" + restatus);
                    }
                }
            }
            Console.WriteLine("服务器:" + item.ServerName + ",涉及表总数:" + changetablenum);
            return 1;
        }
    }
}