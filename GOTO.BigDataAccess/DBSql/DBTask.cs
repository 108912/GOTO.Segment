using GOTO.BigDataAccess.DBSql.Access;
using GOTO.BigDataAccess.DBSql.Model;
using GOTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GOTO.BigDataAccess.DBSql
{
    public class DBTask
    {
        int runnumold = 0;
        public int runnumcurrent = 0;
        private volatile int ThreadRunNum;
        object ThreadRunNumlockThis = new object();
        public int Threadrunnum
        {
            get { return ThreadRunNum; }
            set
            {
                lock (ThreadRunNumlockThis)
                {
                    ThreadRunNum = value;
                }
            }
        }
        private int ThreadNum;
        object ThreadNumlockThis = new object();
        public int Threadnum
        {
            get { return ThreadNum; }
            set
            {
                lock (ThreadNumlockThis)
                {
                    ThreadNum = value;
                }
            }
        }
        public long rowcount = 0;
        public long SyncTaskManagerSum(List<TaskDataParam> taskdata, int tasknum = 20, bool IsFreeCache=true)
        {
            List<Task<long>> listtask = new List<Task<long>>();
            runnumold = taskdata.Count;
            runnumcurrent = 0;
            while (runnumold > runnumcurrent)
            {
                listtask.Clear();
                if (IsFreeCache)
                {
                    var tempmodel = taskdata[runnumcurrent];
                    DBProxy.GetDBAccess(tempmodel.dbtype).ServerCacheClear(tempmodel.connstr);
                }
                for (int i = 1; (i < tasknum) && (runnumold > runnumcurrent); i++)
                {
                    Task<long> t = new Task<long>(n => SyncTaskSum((TaskDataParam)n), taskdata[runnumcurrent++]);
                    t.Start();
                    listtask.Add(t);
                }
                rowcount += listtask.Sum(m => m.Result);
            }
            Console.WriteLine("执行完成:" + rowcount);
            return rowcount;
        }
        public long SyncTaskSum(TaskDataParam model)
        {
            long revalue = CommonHelper.ToLong(DBProxy.GetDBHelper(model.dbtype).GetSingle(model.connstr, model.sqlstr));
            if (revalue < 1)
            {
                ;
            }
            Threadrunnum++;
            Console.WriteLine("线程执行" + Threadrunnum + ",总数:" + revalue + ",服务器名:" + model.servername);
            return revalue;
        }
        public long SyncThreadPoolManagerSum(List<TaskDataParam> taskdata, int tasknum = 20,bool IsFreeCache=true)
        {
            runnumold = taskdata.Count;
            runnumcurrent = 0;
            while (runnumold > runnumcurrent)
            {
                if (IsFreeCache)
                {
                    var tempmodel = taskdata[runnumcurrent];
                    DBProxy.GetDBAccess(tempmodel.dbtype).ServerCacheClear(tempmodel.connstr);
                }
                for (int i = 1; (i < tasknum) && (runnumold > runnumcurrent); i++)
                {
                    Threadnum++;
                    ThreadPool.QueueUserWorkItem(SyncThreadPoolSum, taskdata[runnumcurrent++]);
                }
                while (Threadrunnum < (Threadnum - 1))
                {
                    Thread.Sleep(50);
                }
            }

            Console.WriteLine("执行完成:" + rowcount);
            return rowcount;
        }
        private void SyncThreadPoolSum(object model)
        {
            TaskDataParam dataparam = model as TaskDataParam;
            long revalue = CommonHelper.ToLong(DBProxy.GetDBHelper(dataparam.dbtype).GetSingle(dataparam.connstr, dataparam.sqlstr));
            Threadrunnum++;
            Console.WriteLine("线程执行" + Threadrunnum + ",总数:" + revalue + "," + dataparam.sqlstr);
            rowcount += revalue;
        }
    }
}