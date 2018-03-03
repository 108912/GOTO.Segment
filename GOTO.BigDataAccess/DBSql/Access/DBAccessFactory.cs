namespace GOTO.BigDataAccess.DBSql.Access
{
    public abstract class DBAccessFactory
    {
        #region 索引
        public abstract bool TableIndexExists(string connstr, string databasename, string tablename, string indexname);
        public abstract bool TableIndexAdd(string connstr, string databasename, string tablename, string columnname, string indexname);
        public abstract bool TableIndexDel(string connstr, string tablename, string indexname);
        #endregion
        #region 操作表
        public abstract long TableStorageSize(string connstr, string tablename);
        public abstract bool TableExists(string connstr, string dbname, string tablename);
        public abstract long TableCount(string connstr, string tablename);
        #endregion
        #region 操作数据库
        public abstract long DatabaseStorageSizeAll(string connstr, string databaseprev);
        public abstract long DatabaseStorageSize(string connstr, string databasename);
        public abstract bool DatabaseExists(string connstr, string dbname);
        public abstract bool DatabaseCreate(string connstr, string dbname, string dirpath);
        public abstract void DatabaseDrop(string connstr, string dbname);
        public abstract void DatabaseSHRINK(string connstr, string dbname);
        public abstract void DatabaseClearLog(string connstr, string dbname);
        public abstract void DatabaseSPAttach(string connstr, string dbname, string dbpath);
        public abstract void DatabaseSPDetach(string connstr, string dbname);
        #endregion
        #region 操作服务器
        public abstract bool ServerConnectionCheck(string connstr, string servername, string username, string userpwd, string databasename = "");
        public abstract bool DirectoryExists(string connstr, string dirpath);
        public abstract bool DirectoryCreate(string connstr, string dirpath);
        public abstract bool DirectoryDrop(string connstr, string dirpath);
        public abstract void ServerCacheClear(string connstr);
        #endregion
    }
}
