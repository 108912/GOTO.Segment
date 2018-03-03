using System.Text;
using GOTO.Common;

namespace GOTO.BigDataAccess.DBSql.Access
{
    public class SqlServerAccess:DBAccessFactory
    {   
        #region 索引
        public override bool TableIndexExists(string connstr,string databasename, string tablename, string indexname)
        {
            bool revalue = false;
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.AppendFormat(" select 1 from {0}.dbo.sysindexes where id=object_id('{0}.dbo.{1}') and name='{2}' ", databasename,tablename, indexname);
                int temp = CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr,sqlstr.ToString()));
                if (temp > 0)
                {
                    revalue = true;
                }
            }
            catch
            {
            }
            return revalue;
        }
        public override bool TableIndexAdd(string connstr, string databasename, string tablename, string columnname, string indexname)
        {
            bool revalue = false;
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.AppendFormat(" create index {0} on {1}.dbo.{2}({3}) ", indexname,databasename, tablename, columnname);
                int temp = CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr.ToString()));
                if (temp > 0)
                {
                    revalue = true;
                }
            }
            catch
            {
            }
            return revalue;
        }
        public override bool TableIndexDel(string connstr, string tablename, string indexname)
        {
            bool revalue = false;
            try
            {
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.AppendFormat(" DROP INDEX {0}.{1} ", tablename, indexname);
                int temp = CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr.ToString()));
                if (temp > 0)
                {
                    revalue = true;
                }
            }
            catch
            {
            }
            return revalue;
        }
        #endregion
        #region 操作表
        public override long TableStorageSize(string connstr, string tablename)
        {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append("select SUM(used_pages)*8 size from ");
            sqlstr.AppendFormat("	(select object_id from sys.objects where name='{0}') t1", tablename);
            sqlstr.Append("	left join(select partition_id,object_id from sys.partitions) t2");
            sqlstr.Append("	on t1.object_id=t2.object_id");
            sqlstr.Append("	left join(select container_id,used_pages from sys.allocation_units) t3");
            sqlstr.Append("	on t2.partition_id=t3.container_id");
            return CommonHelper.ToLong(new SqlServerHelper().GetSingle(connstr, sqlstr.ToString()));
        }
        public override bool TableExists(string connstr, string dbname, string tablename)
        {
            bool revalue = false;
            string sqlstr = string.Format(@"select COUNT(0) from {0}.dbo.sysobjects  where name='{1}'", dbname, tablename);
            if (CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr)) > 0)
            {
                revalue = true;
            }
            return revalue;
        }
        public override long TableCount(string connstr, string tablename)
        {
            long revalue = 0;
            string sqlstr = string.Format(" select  sum(b.rows) as 记录条数 from sys.sysobjects a,sys.sysindexes b where a.name='{0}' and a.id=b.id and a.xtype='u' ", tablename);
            revalue = CommonHelper.ToLong(new SqlServerHelper().GetSingle(connstr, sqlstr));
            return revalue;
        }
        #endregion
        #region 操作数据库
        public override long DatabaseStorageSizeAll(string connstr, string databaseprev)
        {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.AppendFormat("select sum(size)*8 size from master..sysaltfiles where name like '{0}%'", databaseprev);
            return CommonHelper.ToLong(new SqlServerHelper().GetSingle(connstr, sqlstr.ToString()));
        }
        public override long DatabaseStorageSize(string connstr, string databasename)
        {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.AppendFormat("select sum(size)*8 size from master..sysaltfiles where name like '{0}%'", databasename);
            return CommonHelper.ToLong(new SqlServerHelper().GetSingle(connstr, sqlstr.ToString()));
        }
        public override bool DatabaseExists(string connstr, string dbname)
        {
            bool revalue = false;
            string sqlstr = string.Format(@"select dbid from sysdatabases where name='{0}'", dbname);
            if (CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr)) > 0)
            {
                revalue = true;
            }
            return revalue;
        }
        public override bool DatabaseCreate(string connstr, string dbname, string dirpath)
        {
            bool revalue = false;
            string sqlstr = string.Format(@"create database {0} 
on  primary(
    name='{0}',
    filename='{1}\{0}.mdf',size=5mb,maxsize=10gb,filegrowth=10%
)
log on(
name='{0}_log',
    filename='{1}\{0}_log.ldf',size=2mb,filegrowth=1mb
);", dbname, dirpath);
            if (CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr)) > 0)
            {
                revalue = true;
            }
            return revalue;
        }
        public override void DatabaseDrop(string connstr, string dbname)
        {
            string sqlstr = string.Format(@"drop database {0}", dbname);
            new SqlServerHelper().ExecuteSql(connstr, sqlstr);
        }
        public override void DatabaseSHRINK(string connstr, string dbname)
        {
            string sqlstr = string.Format(@"DBCC SHRINKDATABASE({0})", dbname);
            new SqlServerHelper().GetSingle(connstr, sqlstr);
        }
        public override void DatabaseClearLog(string connstr, string dbname)
        {
            string sqlstr = string.Format(@"DBCC SHRINKFILE (N'{0}_Log' , 11, TRUNCATEONLY)", dbname);
            new SqlServerHelper().GetSingle(connstr, sqlstr);
        }
        public override void DatabaseSPAttach(string connstr, string dbname, string dbpath)
        {
            string sqlstr = string.Format("EXEC sp_attach_db @dbname = '{0}', @filename1 = '{1}\\{0}.mdf',@filename2= '{1}\\{0}_log.LDF'", dbname, dbpath);
            new SqlServerHelper().GetSingle(connstr, sqlstr);
        }
        public override void DatabaseSPDetach(string connstr, string dbname)
        {
            string sqlstr = string.Format("EXEC sp_detach_db @dbname = '{0}'", dbname);
            new SqlServerHelper().GetSingle(connstr, sqlstr);
        }
        #endregion
        #region 操作服务器
        public override bool ServerConnectionCheck(string connstr, string servername, string username, string userpwd, string databasename = "")
        {
            bool revalue = false;
            string conditiondb = "";
            if (!string.IsNullOrEmpty(databasename))
            {
                conditiondb = "-d " + databasename;
            }
            string sqlstr = string.Format(@"
EXEC sp_configure 'show advanced options', 1;RECONFIGURE;
EXEC sp_configure 'xp_cmdshell', 1;RECONFIGURE; 
declare @result int  
 exec @result = master..xp_cmdshell 'osql -S {0}  {3}  -U {1} -P {2}',no_output
select @result  
EXEC sp_configure 'show advanced options', 1;RECONFIGURE;
EXEC sp_configure 'xp_cmdshell', 0;RECONFIGURE;
", servername, username, userpwd, conditiondb);
            if (CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr)) == 0)
            {
                revalue = true;
            }
            return revalue;
        }
        public override bool DirectoryExists(string connstr, string dirpath)
        {
            bool revalue = false;
            string sqlstr = string.Format(@"declare @cmdstr nvarchar(1000)  
set @cmdstr='dir {0}'
exec sp_configure 'show advanced options', 1 
    reconfigure
    exec sp_configure 'xp_cmdshell', 1
    reconfigure  
declare @n int; 
exec @n=xp_cmdshell @cmdstr,NO_OUTPUT 
IF (@n=0) 
select 1
else 
select 0
exec sp_configure 'show advanced options', 0  
    reconfigure", dirpath);
            if (CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr)) == 1)
            {
                revalue = true;
            }
            return revalue;
        }
        public override bool DirectoryCreate(string connstr, string dirpath)
        {
            bool revalue = false;
            string sqlstr = string.Format(@"declare @cmdstr nvarchar(1000)  
set @cmdstr='mkdir {0}'
exec sp_configure 'show advanced options', 1 
    reconfigure
    exec sp_configure 'xp_cmdshell', 1
    reconfigure  
declare @n int; 
exec @n=xp_cmdshell @cmdstr,NO_OUTPUT 
IF (@n=0) 
select 1
else 
select 0
exec sp_configure 'show advanced options', 0  
    reconfigure", dirpath);
            if (CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr)) == 1)
            {
                revalue = true;
            }
            return revalue;
        }
        public override bool DirectoryDrop(string connstr, string dirpath)
        {
            bool revalue = false;
            string sqlstr = string.Format(@"declare @cmdstr nvarchar(1000)  
set @cmdstr='rd {0}'
exec sp_configure 'show advanced options', 1 
    reconfigure
    exec sp_configure 'xp_cmdshell', 1
    reconfigure  
declare @n int; 
exec @n=xp_cmdshell @cmdstr,NO_OUTPUT 
IF (@n=0) 
select 1
else 
select 0
exec sp_configure 'show advanced options', 0  
    reconfigure", dirpath);
            if (CommonHelper.ToInt(new SqlServerHelper().GetSingle(connstr, sqlstr)) == 1)
            {
                revalue = true;
            }
            return revalue;
        }
        public override void ServerCacheClear(string connstr)
        {
            string sqlstr = ("DBCC DROPCLEANBUFFERS;DBCC FREESESSIONCACHE;DBCC FREEPROCCACHE;DBCC FREESYSTEMCACHE('ALL');");
            new SqlServerHelper().GetSingle(connstr, sqlstr);
        }
        #endregion
    }
}
