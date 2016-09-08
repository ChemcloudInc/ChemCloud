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
    public class ShipManager
    {

        private readonly ArrayList _providers;

        /// <summary>
        ///   创建对象
        /// </summary>
        public ShipManager()
        {
            _providers = new ArrayList();

        }

        /// <summary>
        ///    添加计算方式
        /// </summary>
        public void AddProvider(AbstractCreateShipProvider provider)
        {
            _providers.Add(provider);
        }



        public List<ShipReply> CreateShips()
        {
            List<ShipReply> result = new List<ShipReply>();
            var threads = new ArrayList(_providers.Count);

            foreach (AbstractCreateShipProvider provider in _providers)
            {
                //  provider.Shipment = shipment;

                var thread = new Thread(provider.CreateShipment);

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

            foreach (AbstractCreateShipProvider provider in _providers)
            {
                if (provider.ShipReplyEx != null)
                {
                     
                    result.Add(provider.ShipReplyEx);
                }
               
            }

            return result;
        }



    }
}
