using ChemColud.Shipping.ShippingProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    public class RateManagerFactory
    {
      
        public static RateManager Build()
        {
            var providers = Assembly.GetAssembly(typeof(IShippingProvider)).GetTypes().Where(x => x.BaseType == typeof(AbstractShippingProvider));

            var rateManager = new RateManager();

            foreach (var provider in providers)
            {
                var instance = Activator.CreateInstance(provider) as IShippingProvider;

                if (instance == null)
                {
                    continue;
                }

                rateManager.AddProvider(instance);
            }

            return rateManager;
        }
    }
}
