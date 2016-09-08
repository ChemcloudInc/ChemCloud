using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace ChemCloud.DBUtility
{
    public class PubConstant
    {
        static string _connectionString;
        /// <summary>
        /// 获取连接字符串
        /// </summary>

        public static string ConnectionString
        {
            get
            {
                return GetConfigString("ConnectionString");
            }
            set
            {
                _connectionString = value;
            }
        }
        public static string GetConfigString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object cache = GetCache(cacheKey);
            if (cache == null)
            {
                cache = ConfigurationManager.AppSettings[key];
                if (cache != null)
                {
                    SetCache(cacheKey, cache, DateTime.Now.AddMinutes(180.0), TimeSpan.Zero);
                }
            }
            return cache.ToString();
        }
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string cacheKey)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }
    }
}
