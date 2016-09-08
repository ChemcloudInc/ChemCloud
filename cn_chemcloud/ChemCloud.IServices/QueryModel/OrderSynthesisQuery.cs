using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class OrderSynthesisQuery : QueryBase
    {
        public string productName { get; set; }

        public int status { get; set; }
        public long userId { get; set; }

        public string OrderNumber { get; set; }

        public string ShopId { get; set; }

        public OrderSynthesisQuery()
        {

        }
    }
}
