using GOTO.BigDataAccess.DBSql;
using GOTO.BigDataAccess.DBSql.Manager;
using GOTO.BigDataAccess.DBSql.Model;
using GOTO.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GOTO.ConsoleTest
{
    public static class TestDB
    {
        private static BigDataAccess.DBSql.XmlModel.SqlBaseItemXml basemodel = DBConfig.GetBaseXmlConfig(1112);
        /// <summary>
        /// 分离数据库
        /// </summary>
        public static void databasedetach()
        {
            var t1 = new ManagerDatabase().DatabaseDetachAll(basemodel);
            Console.WriteLine("分离数据库个数:" + t1);
        }
        /// <summary>
        /// 附加数据库
        /// </summary>
        public static void databaseattach()
        {
            var t2 = new ManagerDatabase().DatabaseAttachAll(basemodel);
            Console.WriteLine("附加数据库个数:" + t2);
        }
        /// <summary>
        /// 表添加索引
        /// </summary>
        public static void tableindexadd()
        {
            new ManagerTable().TableAddIndexAll(basemodel);
        }
        /// <summary>
        /// 表删除索引
        /// </summary>
        public static void tableindexdel()
        {
            new ManagerTable().TableDelIndexAll(basemodel);
        }
        /// <summary>
        /// 删除示例
        /// </summary>
        public static void delete()
        {
            Console.WriteLine(new DBAction().Delete(basemodel, 40001));
        }
        /// <summary>
        /// 清除一组数据对象
        /// </summary>
        public static void sqlbaseclear()
        {
            new DBAction().SqlBaseClear(basemodel);
        }
        /// <summary>
        /// 更新记录示例
        /// </summary>
        public static void update()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("str11", "dddddddddd");
            dic.Add("num1", 11);
            dic.Add("num3", 0.11111111);
            Console.WriteLine(new DBAction().Update(basemodel, 40002, dic));
        }
        /// <summary>
        /// 数据写入示列
        /// </summary>
        public static void addrow()
        {
            new DBAction().testAddRow(basemodel);
        }
        /// <summary>
        ///查询所有数据示列
        /// </summary>
        public static void getrowallcount()
        {
            long sqlnum1 = 0;
            DBRead serverread1 = new DBRead();
            var d1 = serverread1.GetAllCount2(basemodel, ref sqlnum1);
            Console.WriteLine("返回记录总数:" + d1 + ",执行数据库次数：" + sqlnum1);
        }
        //查询列表示例
        public static void getlistcondition()
        {
            ConditionFieldModel condition = new ConditionFieldModel();
            ConditionFieldItemModel fieldmodel = new ConditionFieldItemModel() { FieldName = "id", ValueMin = 0, ValueMax = 2012199998 };
            condition.List.Add(fieldmodel);
            ConditionFieldItemModel fieldmodel2 = new ConditionFieldItemModel() { FieldName = "num1", ValueMin = 0, ValueMax = 35000 };
            condition.List.Add(fieldmodel2);
            //ConditionFieldItemModel fieldmodel3 = new ConditionFieldItemModel() { FieldName = "num2", ValueMin = 300, ValueMax = 3500 };
            //condition.List.Add(fieldmodel3);
            long sqlnum = 0;
            DBRead serverread = new DBRead();
            //var d1 = serverread.GetCount(condition, ref sqlnum);
            //Console.WriteLine("返回记录总数:" + d1 + ",执行数据库次数：" + sqlnum);
            DataTable ddd2 = serverread.GetList(basemodel, condition, ref sqlnum);
            Console.WriteLine("返回记录数:" + ddd2.Rows.Count + ",执行数据库次数：" + sqlnum);
        }
        /// <summary>
        /// 服务器连接检查
        /// </summary>
        public static void serverconnectioncheck()
        {
            new ManagerServer().ServerConnectionCheckAll(basemodel);
        }
        /// <summary>
        /// 数据库检查
        /// </summary>
        public static void databaseexistsall()
        {
            new ManagerDatabase().DatabaseUseCheckAll(basemodel);
        }
        /// <summary>
        /// 数据表检查
        /// </summary>
        public static void tableexistsall()
        {
            new ManagerTable().TableUseCheckAll(basemodel);
        }
        public static void servercacheclear()
        {
            Console.WriteLine("请输入服务器编号(all:所有服务器)");
            string servernumber = CommonHelper.ToStr(Console.ReadLine());
            if (servernumber.ToLower().Trim() == "all")
            {
                new ManagerServer().ServerCacheClearAll(basemodel);
            }
            else
            {
                var item = DBConfig.GetServerXmlConfig(basemodel).SqlServerList.Where(m => m.Number == CommonHelper.ToLong(servernumber)).FirstOrDefault();

                if (item != null)
                {
                    new ManagerServer().ServerCacheClearItem(item);
                }
            }
        }
    }
}