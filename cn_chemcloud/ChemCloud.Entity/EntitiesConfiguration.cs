using EFCache;
using EFCache.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Entity
{
    public class EntitiesConfiguration : DbConfiguration
    {
        internal static readonly ICache Cache;

        static EntitiesConfiguration()
        {
            EfCacheProviderConfigurationSection section = (EfCacheProviderConfigurationSection)ConfigurationManager.GetSection("efCache");
            ProviderSettings setting = section.Providers[section.DefaultProvider];
            string mode = setting.Type;
            if (mode == "1")
            {
                string host = "localhost";
                string port = "6379";
                string dbname = "0";
                string timeout = "60000";
                try
                {
                    host = setting.Parameters["db_host"];
                    port = setting.Parameters["db_port"];
                    dbname = setting.Parameters["db_name"];
                    timeout = setting.Parameters["timeout"];
                }
                catch
                { }

                if (string.IsNullOrEmpty(host))
                {
                    host = "localhost";
                }
                int portInt = 6379;
                int.TryParse(port, out portInt);
                int dbnameInt = 0;
                int.TryParse(dbname, out dbnameInt);
                int timeoutInt = 1000 * 60;
                int.TryParse(timeout, out timeoutInt);

                ConfigurationOptions options = new ConfigurationOptions();
                options.EndPoints.Add(host, portInt);
                options.DefaultDatabase = dbnameInt;
                options.SyncTimeout = options.ResponseTimeout = options.ConnectTimeout = timeoutInt;
                Cache = new RedisCache(options);
            }
            else
            {
                Cache = new InMemoryCache();
            }
        }

        public EntitiesConfiguration()
        {
            var transactionHandler = new CacheTransactionHandler(Cache);

            AddInterceptor(transactionHandler);

            Loaded += (sender, args) => args.ReplaceService<DbProviderServices>((s, _) => new CachingProviderServices(s, transactionHandler));
        }

    }
}
