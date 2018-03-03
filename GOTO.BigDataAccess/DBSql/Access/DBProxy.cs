
namespace GOTO.BigDataAccess.DBSql.Access
{
    public static class DBProxy
    {
        public static DBAccessFactory GetDBAccess(string dbtype)
        {
            DBAccessFactory dbservice = new SqlServerAccess();
            switch (dbtype)
            {
                case "sqlserver": return new SqlServerAccess();
            }
            return dbservice;
        }
        public static DBHelperFactory GetDBHelper(string dbtype)
        {
            switch (dbtype)
            {
                case "sqlserver": return new SqlServerHelper();
            }
            return new SqlServerHelper();
        }
    }
}
