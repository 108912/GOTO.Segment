using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GOTO.BigDataAccess.DBSql.XmlModel;
using GOTO.BigDataAccess.DBSql.Manager;
using GOTO.BigDataAccess.DBSql.Access;

namespace GOTO.BigDataAccess.DBSql
{
    public class DBAction
    {
        public void testAddRow(SqlBaseItemXml basemodel)
        {
            try
            {
                ManagerConfig servicemanagerconfig = new ManagerConfig();
                int writenum = 0;
                for (int i = 0; i < 100000000; i++)
                {
                    DBPosition position = new DBPosition();
                    var getbool = position.GetPosition(basemodel);
                    servicemanagerconfig.SaveConfigServer(basemodel);
                    servicemanagerconfig.SaveConfigDatabase(basemodel);
                    servicemanagerconfig.SaveConfigTable(basemodel);
                    if (getbool)
                    {
                        var serveritem = DBConfig.GetServerItemXmlConfig(basemodel, position.currentservernumber);
                        DBWrite.AddBulkRow(basemodel, serveritem, position.currentconnstrdatabase, testBulkRowData(basemodel, position.currenttable), position.currentdatabasenumber, position.currenttablenumber, basemodel.FieldIndexList);
                        servicemanagerconfig.SaveConfigField(basemodel);
                        Console.WriteLine("" + (++writenum));
                    }
                    else
                    {
                        Console.WriteLine("请配置更多服务器后写入");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public DataTable testBulkRowData(SqlBaseItemXml basemodel,string tablename)
        {
            DataTable bulkdata = DBConfig.GetTableStruct(basemodel, tablename);
            Random random = new Random();
            long rowmin = 0;
            long rowcurrent = 0;
            long rowmax = 0;
            int rownum = 10000;

            long rowidrrr = DBConfig.GetRowId(basemodel.Number);
            DBConfig.UpdateRowId(basemodel.Number,rownum);
            rowmin = rowidrrr + 1;
            rowmax = rowmin + rownum;
            rowcurrent = rowmin;
            for (int i = 0; i < rownum; i++)
            {
                DataRow newRow;
                newRow = bulkdata.NewRow();
                #region
                newRow["id"] = rowcurrent++;
                newRow["num1"] = rowcurrent / 100000;
                newRow["num2"] = random.Next(10000);
                newRow["num3"] = random.NextDouble();
                newRow["str11"] = tablename + i;
                #endregion
                bulkdata.Rows.Add(newRow);
            }
            return bulkdata;
        }
        public bool Update(SqlBaseItemXml basemodel, long rowid, Dictionary<string, object> updatefieldlist)
        {
            var fieldmodel = DBConfig.GetFieldXmlConfig(basemodel).SqlFieldList.Where(m => m.FieldName == basemodel.PrimarykeyName && m.ValueMin <= rowid && m.ValueMax >= rowid).FirstOrDefault();
            if (fieldmodel == null)
                return false;
            var servermodel = DBConfig.GetServerItemXmlConfig(basemodel, fieldmodel.ServerNumber);
            var databasemodel = DBConfig.GetDatabaseItemXmlConfig(basemodel, fieldmodel.DatabaseNumber);
            var tablemodel = DBConfig.GetTableItemXmlConfig(basemodel, fieldmodel.TableNumber);
            return DBProxyAction.Update(basemodel,rowid,servermodel,databasemodel,tablemodel, updatefieldlist);
        }
        public bool Delete(SqlBaseItemXml basemodel,long rowid)
        {
            bool revalue = false;
            bool changexml = false;
            var fieldmodel = DBConfig.GetFieldXmlConfig(basemodel).SqlFieldList.Where(m => m.FieldName == basemodel.PrimarykeyName && m.ValueMin <= rowid && m.ValueMax >= rowid).FirstOrDefault();
            if (fieldmodel == null)
                return false;
            var servermodel = DBConfig.GetServerItemXmlConfig(basemodel, fieldmodel.ServerNumber);
            var databasemodel = DBConfig.GetDatabaseItemXmlConfig(basemodel, fieldmodel.DatabaseNumber);
            var tablemodel = DBConfig.GetTableItemXmlConfig(basemodel, fieldmodel.TableNumber);
            revalue= DBProxyAction.Delete(basemodel,rowid,servermodel,databasemodel,tablemodel,fieldmodel);
            if (revalue)
            {
                if (fieldmodel.ValueMin == rowid)
                {
                    DBConfig.FieldRemove(basemodel, fieldmodel);
                    fieldmodel.ValueMin = rowid + 1;
                    DBConfig.FieldAdd(basemodel, fieldmodel);
                    changexml = true;
                }
                else if(fieldmodel.ValueMax==rowid)
                {
                    DBConfig.FieldRemove(basemodel, fieldmodel);
                    fieldmodel.ValueMax = rowid - 1;
                    DBConfig.FieldAdd(basemodel, fieldmodel);
                    changexml = true;
                }
            }
            if (changexml)
            {
                new ManagerConfig().SaveConfigField(basemodel);
            }
            return revalue;
        }
        public bool SqlBaseClear(SqlBaseItemXml basemodel)
        {
            bool revalue = false;
            var serverlist = DBConfig.GetServerItemXmlConfigList(basemodel);
            if (serverlist != null)
            {
                foreach (var serveritem in serverlist)
                {
                    string connstr = DBProxyAction.GetConnStr(basemodel, serveritem);
                    var databaselist = DBConfig.GetDatabaseItemXmlConfigList(basemodel,serveritem.Number);
                    if (databaselist != null)
                    {
                        foreach (var databaseitem in databaselist)
                        {
                            DBProxy.GetDBAccess(serveritem.DBType).DatabaseDrop(connstr, databaseitem.DatabaseName);
                        }
                    }
                }
                ManagerConfig servermanagerconfig = new ManagerConfig();
                //servermanagerconfig.InitServerXml(basemodel);
                servermanagerconfig.InitDatabaseXml(basemodel);
                servermanagerconfig.InitTableXml(basemodel);
                servermanagerconfig.InitFieldXml(basemodel);
            }
            return revalue;
        }
    }
}