using GOTO.BigDataAccess.DBSql.XmlModel;
using GOTO.Common;
using System.Collections.Generic;
using System.Text;

namespace GOTO.BigDataAccess.DBSql.Manager
{
    public class ManagerConfig
    {
        public void InitServerXml(SqlBaseItemXml basemodel)
        {
            SqlServerXml modelserver = new SqlServerXml();
            FileHelper.WriteText(basemodel.ServerFilePath, XMLSerilizable.ObjectToXML(modelserver));
            DBConfig.ReloadServerXmlConfig(basemodel);
        }
        public void InitDatabaseXml(SqlBaseItemXml basemodel)
        {
            SqlDatabaseXml modeldatabase = new SqlDatabaseXml();
            FileHelper.WriteText(basemodel.DatabaseFilePath, XMLSerilizable.ObjectToXML(modeldatabase));
            DBConfig.ReloadDatabaseXmlConfig(basemodel);
        }
        public void InitTableXml(SqlBaseItemXml basemodel)
        {
            SqlTableXml modeltable = new SqlTableXml();
            FileHelper.WriteText(basemodel.TableFilePath, XMLSerilizable.ObjectToXML(modeltable));
            DBConfig.ReloadTableXmlConfig(basemodel);
        }
        public void InitFieldXml(SqlBaseItemXml basemodel)
        {
            SqlFieldXml modelfield = new SqlFieldXml();
            FileHelper.WriteText(basemodel.FieldFilePath, XMLSerilizable.ObjectToXML(modelfield));
            DBConfig.ReloadFieldXmlConfig(basemodel);
        }
        public void SaveConfig(SqlBaseItemXml basemodel)
        {
            SaveConfigServer(basemodel);
            SaveConfigDatabase(basemodel);
            SaveConfigTable(basemodel);
            SaveConfigField(basemodel);
        }
        public void SaveConfigServer(SqlBaseItemXml basemodel)
        {
            FileHelper.WriteText(basemodel.ServerFilePath, XMLSerilizable.ObjectToXML(DBConfig.GetServerXmlConfig(basemodel), Encoding.UTF8));
        }
        public void SaveConfigDatabase(SqlBaseItemXml basemodel)
        {
            FileHelper.WriteText(basemodel.DatabaseFilePath, XMLSerilizable.ObjectToXML(DBConfig.GetDatabaseXmlConfig(basemodel), Encoding.UTF8));
        }
        public void SaveConfigTable(SqlBaseItemXml basemodel)
        {
            FileHelper.WriteText(basemodel.TableFilePath, XMLSerilizable.ObjectToXML(DBConfig.GetTableXmlConfig(basemodel), Encoding.UTF8));
        }
        public void SaveConfigField(SqlBaseItemXml basemodel)
        {
            FileHelper.WriteText(basemodel.FieldFilePath, XMLSerilizable.ObjectToXML(DBConfig.GetFieldXmlConfig(basemodel), Encoding.UTF8));
        }
        public void ServerChangeIsWrite(SqlServerItemXml item, bool iswrite = false)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            DBConfig.ServerRemove(basemodel,item);
            item.IsWrite = iswrite;
            DBConfig.ServerAdd(basemodel, item);
        }
        public void ServerChangeIsUse(SqlServerItemXml item, bool isuse = false)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            DBConfig.ServerRemove(basemodel, item);
            item.IsUse = isuse;
            DBConfig.ServerAdd(basemodel, item);
        }
        public void DatabaseChangeIsWrite(SqlDatabaseItemXml item, bool iswrite = false)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            DBConfig.DatabaseRemove(basemodel, item);
            item.IsWrite = iswrite;
            DBConfig.DatabaseAdd(basemodel, item);
        }
        public void DatabaseChangeIsUse(SqlDatabaseItemXml item, bool isuse = false)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            DBConfig.DatabaseRemove(basemodel, item);
            item.IsUse = isuse;
            DBConfig.DatabaseAdd(basemodel, item);
        }
        public void TableChangeIsWrite(SqlTableItemXml item, bool iswrite = false)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            DBConfig.TableRemove(basemodel, item);
            item.IsWrite = iswrite;
            DBConfig.TableAdd(basemodel, item);
        }
        public void TableChangeIsUse(SqlTableItemXml item, bool isuse = false)
        {
            var basemodel = DBConfig.GetBaseXmlConfig(item.BaseNumber);
            DBConfig.TableRemove(basemodel, item);
            item.IsUse = isuse;
            DBConfig.TableAdd(basemodel, item);
        }
    }
}