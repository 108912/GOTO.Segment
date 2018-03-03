using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GOTO.BigDataAccess.DBSql.XmlModel;
using GOTO.Common;
using GOTO.BigDataAccess.DBSql.Access;

namespace GOTO.BigDataAccess.DBSql
{
    public class DBWrite
    {
        public static SqlDatabaseItemXml AddDatabase(string currentconnstrserver, SqlServerItemXml item)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            SqlDatabaseItemXml databasetempmodel = new SqlDatabaseItemXml();
            databasetempmodel.BaseNumber = item.BaseNumber;
            databasetempmodel.ServerNumber = item.Number;
            databasetempmodel.Number = DBConfig.GetDatabaseNumber;
            databasetempmodel.DatabaseName = basemodel.DatabasePrev + databasetempmodel.Number;
            databasetempmodel.StorageSize = ((item.StorageSize - item.StorageSize / 5) - item.RetainStorageSize) / 10;
            databasetempmodel.TableNum = basemodel.DefaultTableNum;
            databasetempmodel.IsWrite = true;
            databasetempmodel.IsUse = true;
            databasetempmodel.IsCreate = true;
            databasetempmodel.StorageDirectory = item.StorageDirectory;
            DBConfig.DatabaseAdd(basemodel, databasetempmodel);

            DBProxy.GetDBAccess(item.DBType).DatabaseCreate(currentconnstrserver, databasetempmodel.DatabaseName, databasetempmodel.StorageDirectory);
            return databasetempmodel;
        }
        public static SqlTableItemXml AddTable(string currentconnstr, SqlServerItemXml item, SqlDatabaseItemXml item2)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            SqlTableItemXml tabletempmodel = new SqlTableItemXml();
            tabletempmodel.BaseNumber = item.BaseNumber;
            tabletempmodel.ServerNumber = item.Number;
            tabletempmodel.DataBaseNumber = item2.Number;
            tabletempmodel.Number = DBConfig.GetTableNumber;
            tabletempmodel.TableName = basemodel.TablePrev + tabletempmodel.Number;
            tabletempmodel.IsWrite = true;
            tabletempmodel.IsUse = true;
            tabletempmodel.RowNum = basemodel.DefaultTableRowNum;
            tabletempmodel.StorageSize = (item2.StorageSize - item2.StorageSize / 5) / item2.TableNum;
            DBConfig.TableAdd(basemodel, tabletempmodel);
            DBProxy.GetDBHelper(item.DBType).ExecuteSql(currentconnstr, DBConfig.GetTableSql(basemodel, tabletempmodel.TableName));
            return tabletempmodel;
        }

        public static void AddBulkRow(SqlBaseItemXml basemodel, SqlServerItemXml servermodel, string connstr, DataTable bulkdata, long databasenumber, long tablenumber, List<string> fieldtotal)
        {
            if (fieldtotal != null && fieldtotal.Count > 0)
            {
                var fieldlist = DBConfig.GetFieldXmlConfig(basemodel);
                foreach (var fieldname in fieldtotal)
                {
                    string currentfieldname = fieldname;
                    long rowmin = CommonHelper.ToLong(bulkdata.Compute("min(" + currentfieldname + ")", ""));
                    long rowmax = CommonHelper.ToLong(bulkdata.Compute("max(" + currentfieldname + ")", ""));
                    var fileldobjecttemp = fieldlist.SqlFieldList.Where(m => m.FieldName == currentfieldname && m.TableNumber == tablenumber).FirstOrDefault();
                    if (fileldobjecttemp != null)
                    {
                        DBConfig.FieldRemove(basemodel, fileldobjecttemp);
                        if (fileldobjecttemp.ValueMin > rowmin)
                        {
                            fileldobjecttemp.ValueMin = rowmin;
                        }
                        if (fileldobjecttemp.ValueMax < rowmax)
                        {
                            fileldobjecttemp.ValueMax = rowmax;
                        }
                        DBConfig.FieldAdd(basemodel, fileldobjecttemp);
                    }
                    else
                    {
                        SqlFieldItemXml fileldmodel = new SqlFieldItemXml();
                        fileldmodel.BaseNumber = servermodel.BaseNumber;
                        fileldmodel.ServerNumber = servermodel.Number;
                        fileldmodel.DatabaseNumber = databasenumber;
                        fileldmodel.TableNumber = tablenumber;
                        fileldmodel.Number = DBConfig.GetFieldNumber;
                        fileldmodel.FieldName = currentfieldname;
                        fileldmodel.ValueMin = rowmin;
                        fileldmodel.ValueMax = rowmax;
                        DBConfig.FieldAdd(basemodel, fileldmodel);
                    }
                }
            }
            DBProxy.GetDBHelper(servermodel.DBType).WriteBlockDataToDB(connstr, bulkdata);
        }
    }
}
