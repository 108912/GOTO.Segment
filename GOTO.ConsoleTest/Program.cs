using System;
using GOTO.Common;
using System.Diagnostics;

namespace GOTO.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int readvalue = 0;
            Console.WriteLine("当前时间:" + DateTime.Now);
            while (true)
            {
                Console.WriteLine("请输入操作选项:获取所有记录数(1),根据条件查询(2),数据写入(3),分离数据库(4),附加数据库(5),表添加索引(6),表删除索引(7),服务器缓存清除(9),检查服务器连接(11),检查数据库连接(12),检查表连接(13),删除记录(14),修改记录(15),清空某一数据结构存储(16),退出(q)");
                string getread = Console.ReadLine();
                if (getread.ToLower().Trim() == "q")
                {
                    System.Environment.Exit(0); 
                }
                readvalue = CommonHelper.ToInt(getread);
                sw.Restart();
                switch (readvalue)
                {
                    case 1: TestDB.getrowallcount(); break;
                    case 2: TestDB.getlistcondition(); break;
                    case 3: TestDB.addrow(); break;
                    case 4: TestDB.databasedetach(); break;
                    case 5: TestDB.databaseattach(); break;
                    case 6: TestDB.tableindexadd(); break;
                    case 7: TestDB.tableindexdel(); break;
                    case 9: TestDB.servercacheclear(); break;
                    case 11: TestDB.serverconnectioncheck(); break;
                    case 12: TestDB.databaseexistsall(); break;
                    case 13: TestDB.tableexistsall(); break;
                    case 14: TestDB.delete(); break;
                    case 15: TestDB.update(); break;
                    case 16: TestDB.sqlbaseclear(); break;
                }
                sw.Stop();
                Console.WriteLine(" 执行时间:" + sw.ElapsedMilliseconds+"毫秒");
            }

            Console.WriteLine("当前时间:"+DateTime.Now);
            Console.ReadKey();
        }
       
    }

}