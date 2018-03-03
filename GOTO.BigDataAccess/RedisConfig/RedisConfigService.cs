
namespace GOTO.BigDataAccess.RedisConfig
{
    public static class RedisConfigService
    {
        public static string Get(string key)
        {
            return RedisConfigBase.redis.StringGet(key);
        }
        public static bool Set(string key, string value)
        {
            return RedisConfigBase.redis.StringSet(key, value);
        }
    }
}
