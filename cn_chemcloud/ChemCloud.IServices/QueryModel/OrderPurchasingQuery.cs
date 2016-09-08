using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class OrderPurchasingQuery : QueryBase
    {
        public string orderNum { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
        public int status { get; set; }
        public long userId { get; set; }

        public string ShopId { get; set; }
        public OrderPurchasingQuery()
        {

        }

    }
}
