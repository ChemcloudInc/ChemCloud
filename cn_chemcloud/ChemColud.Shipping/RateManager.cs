using ChemColud.Shipping.ShippingProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    /// <summary>
    ///   负责协调利率从指定供应商的检索指定的装运。
    /// </summary>
    public class RateManager
    {
        private readonly IList<IRateAdjuster> _adjusters;
        private readonly ArrayList _providers;

        /// <summary>
        ///   创建对象
        /// </summary>
        public RateManager()
        {
            _providers = new ArrayList();
            _adjusters = new List<IRateAdjuster>();
        }

        /// <summary>
        ///    添加计算方式
        /// </summary>
        public void AddProvider(IShippingProvider provider)
        {
            _providers.Add(provider);
        }

        public void AddRateAdjuster(IRateAdjuster adjuster)
        {
            _adjusters.Add(adjuster);
        }

        /// <summary>
        /// 获得费率
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        private ShipmentEx getRates(ref ShipmentEx shipment)
        {
            //AbstractShippingProvider provider = _provider as AbstractShippingProvider;
            //provider.Shipment = shipment;
            //provider.GetRates();

            //return shipment;

          
            var threads = new ArrayList(_providers.Count);
            
            foreach (AbstractShippingProvider provider in _providers)
            {
               
                provider.Shipment = shipment;
               
                var thread = new Thread(provider.GetRates);
               
                thread.Name = provider.Name;
               
                thread.Start();
              
                threads.Add(thread);
            }
          
            while (threads.Count > 0)
            {
               
                for (var x = (threads.Count - 1); x > -1; x--)
                {
                   
                    if (((Thread)threads[x]).ThreadState == ThreadState.Stopped)
                    {
                        ((Thread)threads[x]).Abort();
                        threads.RemoveAt(x);
                    }
                }
                Thread.Sleep(1);
            }
            return shipment;
        }

       
        public ShipmentEx GetRates(Address originAddress, Address destinationAddress, Package package)
        {
            return GetRates(originAddress, destinationAddress, new List<Package> { package });
        }

        public ShipmentEx GetRates(Address originAddress, Address destinationAddress, List<Package> packages)
        {
            var shipment = new ShipmentEx(originAddress, destinationAddress, packages) { RateAdjusters = _adjusters };
            return getRates(ref shipment);
        }
    }
}
