using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChemCloud.Model;
using ChemColud.Shipping;

namespace ChemCloud.Web.Areas.Web.Models
{
    public class MarginBillModel
    {
        public string Bill { get; set; }

        public string Ship { get; set; }
    }

    public class ShopDeliver
    {
        public ShopDeliver()
        {

        }
        public long ShopId { get; set; }

        public string DeliverType { get; set; }

        public decimal DeliverCost { get; set; }
    }

    public class BillShopDeliver
    {
        public List<ShopDeliver> Shops { get; set; }
    }

    public class ShopCartCost
    {
        public long ShopId { get; set; }
      
        public int DestId { get; set; }

        public string FreightType { get; set; }
    }
}