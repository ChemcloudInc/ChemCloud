using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping.ShippingProviders
{
    /// <summary>
    ///    定义运费计算的接口
    /// </summary>
    public interface IShippingProvider
    {
        /// <summary>
        ///     名称
        /// </summary>
        string Name { get; }
        /// <summary>
        ///  货运
        /// </summary>
        ShipmentEx Shipment { get; }

        /// <summary>
        ///   计算运费
        /// </summary>
        void GetRates();
    }
}
