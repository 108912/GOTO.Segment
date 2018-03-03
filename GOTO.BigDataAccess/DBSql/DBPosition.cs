using GOTO.BigDataAccess.DBSql.Access;
using GOTO.BigDataAccess.DBSql.Manager;
using GOTO.BigDataAccess.DBSql.XmlModel;
using System;
using System.Linq;

namespace GOTO.BigDataAccess.DBSql
{
    public class DBPosition
    {
        public string currentconnstrserver = "";
        public string currentconnstrdatabase = "";
        public string currentserver = "";
        public long currentservernumber = 0;
        public string currentdatabase = "";
        public long currentdatabasenumber = 0;
        public string currenttable = "";
        public long currenttablenumber = 0;
        public bool isposition = false;
        public string msg = "请添加更多的服务器资源";
        public bool GetPosition(SqlBaseItemXml basemodel)
        {
            return GetServer(basemodel);
        }
        public bool GetServer(SqlBaseItemXml basemodel)
        {
            var templist = DBConfig.GetServerXmlConfig(basemodel).SqlServerList.Where(m => m.IsUse == true && m.IsWrite == true).ToList();
            foreach (SqlServerItemXml item in templist)
            {
                long currentStorageSize = 0;
                currentserver = item.ServerName;
                currentservernumber = item.Number;
                currentconnstrserver = DBProxyAction.GetConnStr(basemodel,item); 
                try
                {
                    currentStorageSize = DBProxy.GetDBAccess(item.DBType).DatabaseStorageSizeAll(currentconnstrserver, basemodel.DatabasePrev);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error:" + ex.Message);
                    new ManagerConfig().ServerChangeIsUse(item);
                    return false;
                }
                if ((item.StorageSize - item.RetainStorageSize) > currentStorageSize)
                {
                    if (GetDatabase(basemodel,item))
                        return isposition;
                }
                else
                {
                    new ManagerConfig().ServerChangeIsWrite(item);
                }
            }
            return false;
        }
        public bool GetDatabase(SqlBaseItemXml basemodel,SqlServerItemXml item)
        {
            var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel,item.Number);
            var databaselist2 = databaselist.Where(m => m.IsUse == true && m.IsWrite == true).ToList();
            foreach (var item2 in databaselist2)
            {
                currentdatabase = item2.DatabaseName;
                currentdatabasenumber = item2.Number;
                currentconnstrdatabase = DBProxyAction.GetConnStr(basemodel, item, item2);
                var temp2 = DBProxy.GetDBAccess(item.DBType).DatabaseStorageSize(currentconnstrdatabase, item2.DatabaseName);
                var temp2count = DBConfig.GetTableXmlConfig(basemodel).SqlTableList.Where(m => m.DataBaseNumber == item2.Number).Count();
                if (item2.StorageSize > temp2 && item2.TableNum >= temp2count && GetTable(basemodel,item, item2))
                {
                    return isposition;
                }
                else
                {
                    new ManagerConfig().DatabaseChangeIsWrite(item2);
                }
            }
            if (item.DatabaseNum > databaselist.Count() && item.IsCreate)
            {
                var tempdata = DBWrite.AddDatabase(currentconnstrserver, item);
                currentdatabase = tempdata.DatabaseName;
                currentdatabasenumber = tempdata.Number;
                currentconnstrdatabase = DBProxyAction.GetConnStr(basemodel, item, tempdata);
                var temptable = DBWrite.AddTable(currentconnstrdatabase, item, tempdata);
                currenttable = temptable.TableName;
                currenttablenumber = temptable.Number;
                isposition = true;
                return isposition;
            }
            else
            {
                new ManagerConfig().ServerChangeIsWrite(item);
            }

            return isposition;
        }
        public bool GetTable(SqlBaseItemXml basemodel,SqlServerItemXml item, SqlDatabaseItemXml item2)
        {
            var tablelist = DBConfig.GetTableItemXmlConfigList(basemodel,item2.Number);
            var tablelist2 = tablelist.Where(m => m.IsUse == true && m.IsWrite == true).ToList();
            foreach (var tableobjectitem2 in tablelist2)
            {
                var temp3 = DBProxy.GetDBAccess(item.DBType).TableStorageSize(currentconnstrdatabase, tableobjectitem2.TableName);
                var temp3count = DBProxy.GetDBAccess(item.DBType).TableCount(currentconnstrdatabase, tableobjectitem2.TableName);
                if (tableobjectitem2.StorageSize > temp3 && tableobjectitem2.RowNum > temp3count)
                {
                    currenttable = tableobjectitem2.TableName;
                    currenttablenumber = tableobjectitem2.Number;
                    isposition = true;
                    return isposition;
                }
                else
                {
                    new ManagerConfig().TableChangeIsWrite(tableobjectitem2);
                }
            }
            if (item2.TableNum > tablelist.Count() && item2.IsCreate)
            {
                var temptable = DBWrite.AddTable(currentconnstrdatabase, item, item2);
                currenttable = temptable.TableName;
                currenttablenumber = temptable.Number;
                isposition = true;
                return isposition;
            }
            return isposition;
        }
    }
}