using GOTO.Common;
using StackExchange.Redis;
using System;
using System.Configuration;

namespace GOTO.BigDataAccess.RedisConfig
{
    public static class RedisConfigBase
    {
        private static string redisconstr =CommonHelper.ToStr(ConfigurationManager.AppSettings["RedisConstr"]);
        public static string Redisconstr
        {
            get { return redisconstr; }
            set { redisconstr = value; }
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(redisconstr);
        });

        public static IDatabase redis
        {
            get
            {
                return lazyConnection.Value.GetDatabase();
            }
        }
    }
}
