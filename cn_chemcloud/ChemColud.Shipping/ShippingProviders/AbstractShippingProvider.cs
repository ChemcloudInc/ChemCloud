using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping.ShippingProviders
{
    /// <summary>
    ///     运费计算抽象类
    ///
    /// </summary>
    public abstract class AbstractShippingProvider : IShippingProvider
    {
        public virtual void GetRates()
        {
        }

        public string Name { get; set; }
        public ShipmentEx Shipment { get; set; }

       

        protected void AddRate(string providerCode, string name, decimal totalCharges, DateTime? delivery)
        {
            AddRate(new Rate(Name, providerCode, name, totalCharges, delivery));
        }

        protected void AddRate(Rate rate)
        {
            if (Shipment.RateAdjusters != null)
            {
                rate = Shipment.RateAdjusters.Aggregate(rate, (current, adjuster) => adjuster.AdjustRate(current));
            }
            Shipment.Rates.Add(rate);
        }
    }
}
