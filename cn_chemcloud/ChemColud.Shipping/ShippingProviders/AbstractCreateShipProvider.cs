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
    public abstract class AbstractCreateShipProvider 
    {

        public ShipReply ShipReplyEx { get; set; }
        public virtual void CreateShipment()
        {
            
        }

        public string Url { get; set; }
        public string Name { get; set; }
        public ShipmentEx Shipment { get; set; }

        public string ErrorMessage { get; set; }

      
       
    }
}
