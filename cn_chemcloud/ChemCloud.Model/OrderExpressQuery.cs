using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class OrderExpressQuery
    {
        public OrderExpressQuery() { }


        private long _id;

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }



        public string ShipOrderNumber
        {
            get;
            set;
        }

        public string ExpressContentCN
        {
            get;
            set;
        }

        public int? ShipStatus
        {
            get;
            set;
        }

        public string ExpressContentEN { get; set; }
    }
}
