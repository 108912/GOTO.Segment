using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GOTO.BigDataAccess.DBSql.Access
{
    public abstract class DBHelperFactory
    {
        #region 公用方法
        public abstract bool ConnectionIsUse(string connectionString);
        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        public abstract bool ColumnExists(string connectionString, string tableName, string columnName);
        public abstract int GetMaxID(string connectionString, string FieldName, string TableName);
        public abstract bool Exists(string connectionString, string strSql);
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public abstract bool TabExists(string connectionString, string TableName);
        public abstract bool Exists(string connectionString, string strSql, params SqlParameter[] cmdParms);
        #endregion
        #region  执行简单SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSql(string connectionString, string SQLString);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public abstract void ExecuteSqlTran(string connectionString, ArrayList SQLStringList);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>	
        /// <param name="ParamList">SQL语句对应的参数</param>	
        public abstract int ExecuteSqlTran(string connectionString, ArrayList SQLStringList, List<SqlParameter[]> ParamList);
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSql(string connectionString, string SQLString, string content);
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSqlInsertImg(string connectionString, string strSQL, byte[] fs);

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public abstract object GetSingle(string connectionString, string SQLString);
        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public abstract SqlDataReader ExecuteReader(string connectionString, string strSQL);
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public abstract DataSet Query(string connectionString, string strSQLString);
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public abstract DataTable GetDataDable(string connectionString, string strSQLString);


        #endregion
        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public abstract int ExecuteSql(string connectionString, string SQLString, params SqlParameter[] cmdParms);


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public abstract void ExecuteSqlTran(string connectionString, Hashtable SQLStringList);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public abstract int ExecuteSqlTran1(string connectionString, Hashtable SQLStringList);

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public abstract object GetSingle(string connectionString, string SQLString, params SqlParameter[] cmdParms);

        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public abstract SqlDataReader ExecuteReader(string connectionString, string SQLString, params SqlParameter[] cmdParms);

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public abstract DataSet Query(string connectionString, string SQLString, params SqlParameter[] cmdParms);

        #endregion
        #region 数据大批量的插入数据库中
        /// <summary>
        /// 将Datatable中的数据大批量的插入数据库中
        /// </summary>
        /// <param name="dt"></param>
        public abstract void WriteBlockDataToDB(string connectionString, DataTable dt);
        #endregion
        #region 存储过程操作

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public abstract SqlDataReader RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public abstract DataSet RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters, string tableName);


        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public abstract int RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters, out int rowsAffected);
        /// <summary>
        /// 执行存储过程，返回第一行第一列		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">第一行第一列值</param>
        /// <returns></returns>
        public abstract string RunProcedure(string connectionString, string storedProcName, IDataParameter[] parameters, out string rowsAffected);

        #endregion
        #region 执行SQL语句，实现数据库事务
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public abstract int ExecuteSqlTran(string connectionString, List<String> SQLStringList);

        /// <summary>
        /// 执行1条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public abstract int ExecuteSqlTran(string connectionString, string SQLString);
        #endregion
    }
}
